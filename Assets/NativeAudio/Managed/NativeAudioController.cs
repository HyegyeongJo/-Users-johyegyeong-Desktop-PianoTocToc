// Native Audio
// 5argon - Exceed7 Experiments
// Problems/suggestions : 5argon@exceed7.com

using System;
using UnityEngine;

namespace E7.Native
{
    /// <summary>
    /// A reference to the native audio source (track) used to play a sound (audio data).
    /// You can for example, <see cref="Stop"> it before it ends or modify various attributes while playing
    /// (Or even immediately after playing so that it starts with those attributes)
    /// 
    /// The native implementation round robins the audio players, so if you hold onto this variable for a long time
    /// it is possible that it will be reused to play other sound. A method like `Replay()` (if exists someday) might produce unexpected result.
    /// </summary>
    public class NativeAudioController
    {
        public int InstanceIndex { get; private set; }

        private void AssertInitialized()
        {
            if (NativeAudio.Initialized == false)
            {
                throw new InvalidOperationException("You cannot use NativeAudioController while Native Audio itself is not in initialized state.");
            }
        }


        /// <param name="InstanceIndex">All actions issued on this <see cref="NativeAudioController"> will be for a specific sound output instance at the native side. (Not source, unlike <see cref="NativeAudioPointer.currentIndex">)</param>
        public NativeAudioController(int InstanceIndex)
        {
            this.InstanceIndex = InstanceIndex;
        }

        /// <summary>
        /// Immediately stop a source that was selected to play the sound.
        /// [iOS] One of all OpenAL sources that was used to play this sound will stop.
        /// [Android] One of all SLAndroidSimpleBufferQueue that was used to play this sound will stop.
        /// </summary>
        public void Stop()
        {
            AssertInitialized();
#if UNITY_IOS
            NativeAudio._StopAudio(InstanceIndex);
#elif UNITY_ANDROID
            NativeAudio.stopAudio(InstanceIndex);
#endif
        }

        /// <summary>
        /// [iOS] Maps to `AL_GAIN`. It is a scalar amplitude multiplier, so the value can go over 1.0 for increasing volume but can be clipped. 
        /// If you put 0.5f, it is attenuated by 6 dB.
        /// 
        /// [Android] Maps to `SLVolumeItf` interface -> `SetVolumeLevel`.
        /// The floating volume parameter will be converted to millibel (20xlog10x100) so that putting 0.5f here results in 6dB attenuation.
        /// </summary>
        public void SetVolume(float volume)
        {
            AssertInitialized();
#if UNITY_IOS
            NativeAudio._SetVolume(InstanceIndex, volume);
#elif UNITY_ANDROID
            NativeAudio.setVolume(InstanceIndex, volume);
#endif
        }

        /// <summary>
        /// -1 for full left, 0 for center, 1 for full right. This pan is based on "balance effect" and not a "constant energy pan". That is
        /// at the center you hear each side fully. (Constant energy pan has 3dB attenuation to both on center.)
        /// 
        /// [iOS] 2D panning in iOS will be emulated in OpenAL's 3D audio engine by splitting your stereo sound into a separated mono sounds, 
        /// then position each one on left and right ear of the listener. When panning, instead of adjusting gain we will just move the source 
        /// further from the listener and the distance attenuation will do the work. (Gain is reserved to the setting volume command, 
        /// so we have 2 stage of gain adjustment this way.
        /// 
        /// [Android] Maps to SLVolumeItf interface -> SetStereoPosition
        /// </summary>
        /// <param name="pan"></param>
        public void SetPan(float pan)
        {
            AssertInitialized();
#if UNITY_IOS
            NativeAudio._SetPan(InstanceIndex, pan);
#elif UNITY_ANDROID
            NativeAudio.setPan(InstanceIndex, pan);
#endif
        }

        /// <summary>
        /// Ask for playback time to a native audio player. It is relative to the start of audio data currently loaded in **seconds**.
        /// The API is very time sensitive and may or may not change the value in the same frame. (depending on where you call it in the script)
        /// [Android] Because of how "stop hack" was implemented, any stopped audio will have a playback time equals to audio's length (not 0)
        /// 
        /// This behaviour is similar to when calling <see cref="AudioSettings.dspTime"> or <see cref="AudioSource.time"> property, those two are in the same update step.
        /// 
        /// Note that <see cref="Time.realTimeSinceStartup"> is not in an update step unlike audio time, and will change every time you call even in 2 consecutive lines of code.
        /// 
        /// [iOS] Get `AL_SEC_OFFSET` attribute. It update in a certain discrete step, and if that step happen in the middle of
        /// the frame this method will return different value depending on where in the script you call it. The update step timing is THE SAME as 
        /// <see cref="AudioSettings.dspTime"> and <see cref="AudioSource.time">.
        /// 
        /// I observed (in iPad 3, iOS 9) that this function sometimes lags on first few calls.
        /// It might help to pre-warm by calling this several times in loading screen or something.
        /// 
        /// [Android] Use GetPosition of SLPlayItf interface. It update in a certain discrete step, and if that step happen in the middle of
        /// the frame this method will return different value depending on where in the script you call it. The update step timing is INDEPENDENT from
        /// <see cref="AudioSettings.dspTime"> and <see cref="AudioSource.time">.
        /// </summary>
        public float GetPlaybackTime()
        {
            AssertInitialized();
#if UNITY_IOS
            return NativeAudio._GetPlaybackTime(InstanceIndex);
#elif UNITY_ANDROID
            return NativeAudio.getPlaybackTime(InstanceIndex);
#else
            return 0;
#endif
        }

        /// <summary>
        /// Set a playback time of this audio player. If the track is in a paused state it is immediately resumed.
        /// You can set it even while the track is playing.
        /// </summary>
        /// <param name="offsetSeconds"></param>
        public void SetPlaybackTime(float offsetSeconds)
        {
            AssertInitialized();
#if UNITY_IOS
            NativeAudio._SetPlaybackTime(InstanceIndex, offsetSeconds);
#elif UNITY_ANDROID
            NativeAudio.setPlaybackTime(InstanceIndex, offsetSeconds);
#endif
        }

        /// <summary>
        /// Pause the underlying audio track chosen for a particular audio.
        /// The track is not protected against being chosen for other audio while pausing, and if that happens the pause status will be cleared out.
        /// </summary>
        public void TrackPause()
        {
            AssertInitialized();
#if UNITY_IOS
            NativeAudio._TrackPause(InstanceIndex);
#elif UNITY_ANDROID
            NativeAudio.trackPause(InstanceIndex);
#endif
        }

        /// <summary>
        /// Resume the underlying audio track chosen for a particular audio.
        /// If by the time you call resume a track has already been used to play other audio, the resume will have no effect since the pause status had already been clreared out.
        /// </summary>
        public void TrackResume()
        {
            AssertInitialized();
#if UNITY_IOS
            NativeAudio._TrackResume(InstanceIndex);
#elif UNITY_ANDROID
            NativeAudio.trackResume(InstanceIndex);
#endif
        }
    }
}