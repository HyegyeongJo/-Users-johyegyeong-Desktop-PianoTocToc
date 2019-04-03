// Native Audio
// 5argon - Exceed7 Experiments
// Problems/suggestions : 5argon@exceed7.com

using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Linq;

namespace E7.Native
{
    public partial class NativeAudio
    {
        public static bool Initialized { get; private set; }

        private static void AssertInitialized()
        {
            if(!Initialized)
            {
                throw new InvalidOperationException("You cannot use Native Audio while in uninitialized state.");
            }
        }

        /// <summary>
        /// Returns true when not in Editor and on iOS or Android.
        /// </summary>
        public static bool OnSupportedPlatform()
        {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            return true;
#else
            return false;
#endif
        }

        /// <summary>
        /// [iOS] Initializes OpenAL. Then 16 OpenAL sources will be allocated all at once. You have a maximum of 16 concurrency shared for all sounds.
        /// It is not possible to initialize again on iOS. (Nothing will happen)
        /// 
        /// [Android] Initializes OpenSL ES. Then 1 OpenSL ES Engine and a number of AudioPlayer object (and in turn AudioTrack) will be allocated all at once.
        /// See <see cref="NativeAudio.Initialize(InitializationOptions)"> overload how to customize your intialization.
        /// 
        /// (More about this limit : https://developer.android.com/ndk/guides/audio/opensl/opensl-for-android)
        /// (And my own research here : https://gametorrahod.com/androids-native-audio-primer-for-unity-developers-65acf66dd124)
        /// </summary>
        public static void Initialize()
        {
            Initialize(InitializationOptions.defaultOptions);
        }

        /// <summary>
        /// [Android] Undo the <see cref="Initialize">. It doesn't affect any loaded audio, just dispose all the native players.
        /// Disposing twice is safe, it does nothing.
        /// </summary>
        public static void Dispose()
        {
#if UNITY_ANDROID
            if (Initialized)
            {
                NativeAudio.disposeIfAllocated();
                Initialized = false;
            }
#else
            return;
#endif
        }

        /// <summary>
        /// [iOS] Initializes OpenAL. Then 16 OpenAL sources will be allocated all at once. You have a maximum of 16 concurrency shared for all sounds.
        /// It is not possible to initialize again on iOS. (Nothing will happen)
        /// 
        /// [Android] Initializes OpenSL ES. Then 1 OpenSL ES Engine and a number of AudioPlayer object (and in turn AudioTrack) will be allocated all at once.
        /// It is possible on Android to initialize again in order to change <paramref name="initializationOptions">.
        /// 
        /// (More about this limit : https://developer.android.com/ndk/guides/audio/opensl/opensl-for-android)
        /// (And my own research here : https://gametorrahod.com/androids-native-audio-primer-for-unity-developers-65acf66dd124)
        /// </summary>
        public static void Initialize(InitializationOptions initializationOptions)
        {
            //Now it is possible to initialize again with different option on Android. It would dispose and reallocate native sources.
#if UNITY_IOS
            if (Initialized) return;
#endif

#if UNITY_IOS
            int errorCode = _Initialize();
            if (errorCode == -1)
            {
                throw new System.Exception("There is an error initializing Native Audio.");
            }
            //There is also a check at native side but just to be safe here.
            Initialized = true;
#elif UNITY_ANDROID
            int errorCode = AndroidNativeAudio.CallStatic<int>(AndroidInitialize, initializationOptions.androidAudioTrackCount, initializationOptions.androidMinimumBufferSize, initializationOptions.preserveOnMinimize);
            if(errorCode == -1)
            {
                throw new System.Exception("There is an error initializing Native Audio.");
            }
            Initialized = true;
#endif
        }

        /// <summary>
        /// Loads by copying Unity-imported <see cref="AudioClip">'s raw audio memory to native side.
        /// You are free to unload the <see cref="AudioClip">'s audio data without affecting what's loaded at the native side after this.
        /// 
        /// Hard requirements : 
        /// - Load type MUST be Decompress On Load so Native Audio could read raw PCM byte array from your compressed audio.
        /// - If you use Load In Background, you must call <see cref="AudioClip.LoadAudioData"> beforehand and ensure that <see cref="AudioClip.loadState"> is <see cref="AudioDataLoadState.Loaded"> before calling <see cref="NativeAudio.Load">. Otherwise it would throw an exception. If you are not using <see cref="AudioClip.loadInBackground"> but also not using <see cref="AudioClip.preloadAudioData">, Native Audio can load for you if not yet loaded.
        /// - Must not be <see cref="AudioClip.ambisonic">.
        /// 
        /// It supports all compression format, force to mono, overriding to any sample rate, and quality slider.
        /// 
        /// Automatically <see cref="NativeAudio.Initialize"> with no option if you didn't do it once yet.
        /// 
        /// [iOS] Loads an audio into OpenAL's output audio buffer. (Max 256) This buffer will be paired to one of 16 OpenAL source when you play it.
        /// 
        /// [Android] Loads an audio into a `short*` array at unmanaged native side. This array will be pushed into one of available `SLAndroidSimpleBufferQueue` when you play it.
        /// The resampling of audio will occur at this moment to match your player's device native rate. The SLES audio player must be created to match the device rate
        /// to enable the special "fast path" audio. What's left is to make our audio compatible with that fast path player, which the resampler will take care of.
        /// 
        /// You can change the sampling quality of SRC (libsamplerate) library per audio basis with the <see cref="NativeAudio.Load(AudioClip, LoadOptions)"> overload.
        /// 
        /// [Editor] This method is a stub and returns `null`.
        /// </summary>
        /// <param name="audioClip">
        /// Hard requirements : 
        /// - Load type MUST be Decompress On Load so Native Audio could read raw PCM byte array from your compressed audio.
        /// - If you use Load In Background, you must call <see cref="AudioClip.LoadAudioData"> beforehand and ensure that <see cref="AudioClip.loadState"> is <see cref="AudioDataLoadState.Loaded"> before calling <see cref="NativeAudio.Load">. Otherwise it would throw an exception. If you are not using <see cref="AudioClip.loadInBackground"> but also not using <see cref="AudioClip.preloadAudioData">, Native Audio can load for you if not yet loaded.
        /// - Must not be <see cref="AudioClip.ambisonic">.
        /// </param>
        /// <returns> An object that stores a number. Native side can pair this number with an actual loaded audio data when you want to play it. You can use <see cref="NativeAudioPointer.Play">, or <see cref="NativeAudioPointer.Unload"> with this object and it would find the correct audio at native side to play.</returns>
        /// <exception cref="Exception">Thrown when some unexpected exception at native side loading occurs.</exception>
        /// <exception cref="NotSupportedException">Thrown when you have prohibited settings on your <see cref="AudioClip">.</exception>
        /// <exception cref="InvalidOperationException">Thrown when you didn't manually load your <see cref="AudioClip"> when it is not set to load in background.</exception>
        public static NativeAudioPointer Load(AudioClip audioClip)
        {
            return Load(audioClip, LoadOptions.defaultOptions);
        }

        /// <summary>
        /// Loads by copying Unity-imported <see cref="AudioClip">'s raw audio memory to native side.
        /// You are free to unload the <see cref="AudioClip">'s audio data without affecting what's loaded at the native side after this.
        /// 
        /// Hard requirements : 
        /// - Load type MUST be Decompress On Load so Native Audio could read raw PCM byte array from your compressed audio.
        /// - If you use Load In Background, you must call <see cref="AudioClip.LoadAudioData"> beforehand and ensure that <see cref="AudioClip.loadState"> is <see cref="AudioDataLoadState.Loaded"> before calling <see cref="NativeAudio.Load">. Otherwise it would throw an exception. If you are not using <see cref="AudioClip.loadInBackground"> but also not using <see cref="AudioClip.preloadAudioData">, Native Audio can load for you if not yet loaded.
        /// - Must not be <see cref="AudioClip.ambisonic">.
        /// 
        /// It supports all compression format, force to mono, overriding to any sample rate, and quality slider.
        /// 
        /// Automatically <see cref="NativeAudio.Initialize"> with no option if you didn't do it once yet.
        /// 
        /// [iOS] Loads an audio into OpenAL's output audio buffer. (Max 256) This buffer will be paired to one of 16 OpenAL source when you play it.
        /// 
        /// [Android] Loads an audio into a `short*` array at unmanaged native side. This array will be pushed into one of available `SLAndroidSimpleBufferQueue` when you play it.
        /// The resampling of audio will occur at this moment to match your player's device native rate. The SLES audio player must be created to match the device rate
        /// to enable the special "fast path" audio. What's left is to make our audio compatible with that fast path player, which the resampler will take care of.
        /// 
        /// You can change the sampling quality of SRC (libsamplerate) library per audio basis with the <see cref="NativeAudio.Load(AudioClip, LoadOptions)"> overload.
        /// 
        /// [Editor] This method is a stub and returns `null`.
        /// </summary>
        /// <param name="audioClip">
        /// Hard requirements : 
        /// - Load type MUST be Decompress On Load so Native Audio could read raw PCM byte array from your compressed audio.
        /// - If you use Load In Background, you must call <see cref="AudioClip.LoadAudioData"> beforehand and ensure that <see cref="AudioClip.loadState"> is <see cref="AudioDataLoadState.Loaded"> before calling <see cref="NativeAudio.Load">. Otherwise it would throw an exception. If you are not using <see cref="AudioClip.loadInBackground"> but also not using <see cref="AudioClip.preloadAudioData">, Native Audio can load for you if not yet loaded.
        /// - Must not be <see cref="AudioClip.ambisonic">.
        /// </param>
        /// <returns> An object that stores a number. Native side can pair this number with an actual loaded audio data when you want to play it. You can use <see cref="NativeAudioPointer.Play">, or <see cref="NativeAudioPointer.Unload"> with this object and it would find the correct audio at native side to play.</returns>
        /// <exception cref="Exception">Thrown when some unexpected exception at native side loading occurs.</exception>
        /// <exception cref="NotSupportedException">Thrown when you have prohibited settings on your <see cref="AudioClip">.</exception>
        /// <exception cref="InvalidOperationException">Thrown when you didn't manually load your <see cref="AudioClip"> when it is not set to load in background.</exception>
        public static NativeAudioPointer Load(AudioClip audioClip, LoadOptions loadOptions)
        {
            AssertAudioClip(audioClip);
            AssertInitialized();

#if UNITY_IOS || UNITY_ANDROID
            //We have to wait for GC to collect this big array, or you could do `GC.Collect()` immediately after.
            short[] shortArray = AudioClipToShortArray(audioClip);
            GCHandle shortArrayPinned = GCHandle.Alloc(shortArray, GCHandleType.Pinned);
#endif


#if UNITY_IOS
            int startingIndex = _SendByteArray(shortArrayPinned.AddrOfPinnedObject(), shortArray.Length * 2, audioClip.channels, audioClip.frequency, loadOptions.resamplingQuality);
            shortArrayPinned.Free();

            if (startingIndex == -1)
            {
                throw new Exception("Error loading NativeAudio with AudioClip named : " + audioClip.name);
            }
            else
            {
                float length = _LengthBySource(startingIndex);
                return new NativeAudioPointer(audioClip.name, startingIndex, length);
            }
#elif UNITY_ANDROID

            //The native side will interpret short array as byte array, thus we double the length.
            int startingIndex = sendByteArray(shortArrayPinned.AddrOfPinnedObject(), shortArray.Length * 2, audioClip.channels, audioClip.frequency, loadOptions.resamplingQuality);
            shortArrayPinned.Free();

            if(startingIndex == -1)
            {
                throw new Exception("Error loading NativeAudio with AudioClip named : " + audioClip.name);
            }
            else
            {
                float length = lengthBySource(startingIndex);
                return new NativeAudioPointer(audioClip.name, startingIndex, length);
            }
#else
            //Load is defined on editor so that autocomplete shows up, but it is a stub. If you mistakenly use the pointer in editor instead of forwarding to normal sound playing method you will get a null reference error.
            return null;
#endif
        }

        private static void AssertAudioClip(AudioClip audioClip)
        {
            if(audioClip.loadType != AudioClipLoadType.DecompressOnLoad)
            {
                throw new NotSupportedException(string.Format("Your audio clip {0} load type is not Decompress On Load but {1}. Native Audio needs to read the raw PCM data by that import mode.", audioClip.name, audioClip.loadType));
            }
            if(audioClip.channels != 1 && audioClip.channels != 2)
            {
                throw new NotSupportedException(string.Format("Native Audio only supports mono or stereo. Your audio {0} has {1} channels", audioClip.name, audioClip.channels));
            }
            if(audioClip.ambisonic)
            {
                throw new NotSupportedException("Native Audio does not support ambisonic audio!");
            }
            if(audioClip.loadState != AudioDataLoadState.Loaded && audioClip.loadInBackground)
            {
                throw new InvalidOperationException("Your audio is not loaded yet while having the import settings Load In Background. Native Audio cannot wait for loading asynchronously for you and it would results in an empty audio. To keep Load In Background import settings, call `audioClip.LoadAudioData()` beforehand and ensure that `audioClip.loadState` is `AudioDataLoadState.Loaded` before calling `NativeAudio.Load`, or remove Load In Background then Native Audio could load it for you.");
            }
        }

        private static short[] AudioClipToShortArray(AudioClip audioClip)
        {
            if (audioClip.loadState != AudioDataLoadState.Loaded)
            {
                if (!audioClip.LoadAudioData())
                {
                    throw new Exception(string.Format("Loading audio {0} failed!", audioClip.name));
                }
            }

            float[] data = new float[audioClip.samples * audioClip.channels];
            audioClip.GetData(data, 0);

            //Convert to 16-bit PCM
            short[] shortArray = new short[audioClip.samples * audioClip.channels];
            for(int i = 0; i < shortArray.Length; i++)
            {
                shortArray[i] = (short)(data[i] * short.MaxValue);
            }
            return shortArray;
        }

        /// <summary>
        /// (**ADVANCED**) Loads an audio from `StreamingAssets` folder's desination at runtime. Most of the case you should use the <see cref="NativeAudio.Load(AudioClip)"> overload instead.
        /// It only supports .wav PCM 16-bit format, stereo or mono, in any sampling rate since it will be resampled to fit the device.
        /// 
        /// If this is the first time loading any audio it will call <see cref="NativeAudio.Initialize"> automatically which might take a bit more time.
        /// 
        /// [iOS] Loads an audio into OpenAL's output audio buffer. (Max 256) This buffer will be paired to one of 16 OpenAL source when you play it.
        /// 
        /// [Android] Loads an audio into a `short*` array at unmanaged native side. This array will be pushed into one of available `SLAndroidSimpleBufferQueue` when you play it.
        /// The resampling of audio will occur at this moment to match your player's device native rate. The SLES audio player must be created to match the device rate
        /// to enable the special "fast path" audio. What's left is to make our audio compatible with that fast path player, which the resampler will take care of.
        /// 
        /// You can change the sampling quality of SRC (libsamplerate) library per audio basis with the `LoadOptions` overload.
        /// 
        /// If the audio is not found in the main app's persistent space (the destination of `StreamingAssets`) it will continue to search for the audio 
        /// in all OBB packages you might have. (Often if your game is a split OBB, things in `StreamingAssets` will go there by default even if the main one is not that large.)
        /// 
        /// [Editor] This method is a stub and returns `null`.
        /// </summary>
        /// <param name="streamingAssetsRelativePath">If the file is `SteamingAssets/Hit.wav` use "Hit.wav" (WITH the extension).</param>
        /// <returns> An object that stores a number. Native side can pair this number with an actual loaded audio data when you want to play it. You can use <see cref="NativeAudioPointer.Play">, or <see cref="NativeAudioPointer.Unload"> with this object and it would find the correct audio at native side to play.</returns>
        /// <exception cref="Exception">Thrown when some unexpected exception at native side loading occurs.</exception>
        public static NativeAudioPointer Load(string streamingAssetsRelativePath)
        {
            return Load(streamingAssetsRelativePath, LoadOptions.defaultOptions);
        }

        /// <summary>
        /// (**ADVANCED**) Loads an audio from `StreamingAssets` folder's desination at runtime. Most of the case you should use the <see cref="NativeAudio.Load(AudioClip)"> overload instead.
        /// It only supports .wav PCM 16-bit format, stereo or mono, in any sampling rate since it will be resampled to fit the device.
        /// 
        /// If this is the first time loading any audio it will call <see cref="NativeAudio.Initialize"> automatically which might take a bit more time.
        /// 
        /// [iOS] Loads an audio into OpenAL's output audio buffer. (Max 256) This buffer will be paired to one of 16 OpenAL source when you play it.
        /// 
        /// [Android] Loads an audio into a `short*` array at unmanaged native side. This array will be pushed into one of available `SLAndroidSimpleBufferQueue` when you play it.
        /// The resampling of audio will occur at this moment to match your player's device native rate. The SLES audio player must be created to match the device rate
        /// to enable the special "fast path" audio. What's left is to make our audio compatible with that fast path player, which the resampler will take care of.
        /// 
        /// You can change the sampling quality of SRC (libsamplerate) library per audio basis with the `LoadOptions` overload.
        /// 
        /// If the audio is not found in the main app's persistent space (the destination of `StreamingAssets`) it will continue to search for the audio 
        /// in all OBB packages you might have. (Often if your game is a split OBB, things in `StreamingAssets` will go there by default even if the main one is not that large.)
        /// 
        /// [Editor] This method is a stub and returns `null`.
        /// </summary>
        /// <param name="streamingAssetsRelativePath">If the file is `SteamingAssets/Hit.wav` use "Hit.wav" (WITH the extension).</param>
        /// <returns> An object that stores a number. Native side can pair this number with an actual loaded audio data when you want to play it. You can use <see cref="NativeAudioPointer.Play">, or <see cref="NativeAudioPointer.Unload"> with this object and it would find the correct audio at native side to play.</returns>
        /// <exception cref="Exception">Thrown when some unexpected exception at native side loading occurs.</exception>
        public static NativeAudioPointer Load(string streamingAssetsRelativePath, LoadOptions loadOptions)
        {
            AssertInitialized();

            if (System.IO.Path.GetExtension(streamingAssetsRelativePath).ToLower() == ".ogg")
            {
                throw new NotSupportedException("Loading via StreamingAssets does not support OGG. Please use the AudioClip overload and set the import settings to Vorbis.");
            }

#if UNITY_IOS
            int startingIndex = _LoadAudio(streamingAssetsRelativePath, (int)loadOptions.resamplingQuality);
            if (startingIndex == -1)
            {
                throw new FileLoadException("Error loading audio at path : " + streamingAssetsRelativePath + " Please check if that audio file really exist relative to StreamingAssets folder or not. Remember that you must include the file's extension as well.", streamingAssetsRelativePath);
            }
            else
            {
                float length = _LengthBySource(startingIndex);
                return new NativeAudioPointer(streamingAssetsRelativePath, startingIndex, length);
            }
#elif UNITY_ANDROID
            int startingIndex = AndroidNativeAudio.CallStatic<int>(AndroidLoadAudio, streamingAssetsRelativePath, (int)loadOptions.resamplingQuality);

            if(startingIndex == -1)
            {
                throw new FileLoadException("Error loading audio at path : " + streamingAssetsRelativePath + " Please check if that audio file really exist relative to StreamingAssets folder or not. Remember that you must include the file's extension as well.", streamingAssetsRelativePath);
            }
            else
            {
                float length = lengthBySource(startingIndex);
                return new NativeAudioPointer(streamingAssetsRelativePath, startingIndex, length);
            }
#else
            //Load is defined on editor so that autocomplete shows up, but it is a stub. If you mistakenly use the pointer in editor instead of forwarding to normal sound playing method you will get a null reference error.
            return null;
#endif
        }


        /// <summary>
        /// (**EXPERIMENTAL**) Native Audio will load a small silent wav and perform various stress test for about 1 second.
        /// Your player won't be able to hear anything, but recommended to do it when there's no other workload running because it will also measure FPS.
        /// 
        /// The test will be asynchronous because it has to wait for frame to play the next audio. Yield wait for the result with the returned <see cref="NativeAudioAnalyzer">.
        /// This is a component of a new game object created to run a test coroutine on your scene.
		/// 
		/// If your game is in a yieldable routine, use `yield return new WaitUntil( () => analyzer.Analyzed );' it will wait a frame until that is `true`.
		/// If not, you can do a blocking wait with a `while` loop on `analyzer.Analyzed == false`.
        /// 
        /// You must have initialized Native Audio before doing the analysis or else Native Audio will initialize with default options.
        /// (Remember you cannot initialize twice to fix initialization options)
        /// 
        /// By the analysis result you can see if the frame rate drop while using Native Audio or not. I have fixed most of the frame rate drop problem I found.
        /// But if there are more obscure devices that drop frame rate, this method can check it at runtime and by the returned result you can stop using Native Audio
        /// and return to Unity <see cref="AudioSource">.
        /// </summary>
        public static NativeAudioAnalyzer SilentAnalyze()
        {
            AssertInitialized();
#if UNITY_ANDROID
            var go = new GameObject("NativeAudioAnalyzer");
            NativeAudioAnalyzer sa = go.AddComponent<NativeAudioAnalyzer>();
            sa.Analyze();
            return sa;
#else
            return null;
#endif
        }

        /// <summary>
        /// Ask the phone about its audio capability. The returned class has different properties depending on platform.
        /// 
        /// [Editor] Does not work, returns default value of <see cref="DeviceAudioInformation">.
        /// </summary>
        public static DeviceAudioInformation GetDeviceAudioInformation()
        {
#if UNITY_ANDROID
            var jo = AndroidNativeAudio.CallStatic<AndroidJavaObject>(AndroidGetDeviceAudioInformation);
            return new DeviceAudioInformation(jo);
#elif UNITY_IOS
            int[] portArray = Enumerable.Repeat(-1, 20).ToArray();
            var portArrayHandle = GCHandle.Alloc(portArray, GCHandleType.Pinned);
            _GetAudioDevices(portArrayHandle.AddrOfPinnedObject());
            portArrayHandle.Free();
            return new DeviceAudioInformation(portArray.Where(x => x != -1).Cast<DeviceAudioInformation.IosAudioPortType>().ToArray());
#else
            return default(DeviceAudioInformation);
#endif
        }
    }
}