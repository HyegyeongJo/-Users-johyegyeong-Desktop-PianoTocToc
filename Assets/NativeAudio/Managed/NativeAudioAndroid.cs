// Native Audio
// 5argon - Exceed7 Experiments
// Problems/suggestions : 5argon@exceed7.com

using UnityEngine;
using System.Runtime.InteropServices;
using System;

namespace E7.Native
{
    public partial class NativeAudio
    {
#if UNITY_ANDROID
        private static AndroidJavaClass androidNativeAudio;
        internal static AndroidJavaClass AndroidNativeAudio
        {
            get
            {
                if (androidNativeAudio == null)
                {
                    androidNativeAudio = new AndroidJavaClass("com.Exceed7.NativeAudio.NativeAudio");
                }
                return androidNativeAudio;
            }
        }

        /// <summary>
        /// [Android] Initialize needs to contact Java as it need the device's native sampling rate and native buffer size to get the "fast path" audio.
        /// </summary>
        internal const string AndroidInitialize = "Initialize";

        /// <summary>
        /// [Android] Load needs to contact Java as it needs to read the audio file sent from `StreamingAssets`,
        /// which could end up in either app persistent space or an another OBB package which we will unpack it and get the content.
        /// </summary>
        internal const string AndroidLoadAudio = "LoadAudio";

        internal const string AndroidGetDeviceAudioInformation = "GetDeviceAudioInformation";

        // -- Operates on an audio file ("source" of data) --

        //The lib name is libnativeaudioe7

        [DllImport("nativeaudioe7")]
        internal static extern int sendByteArray(IntPtr byteArrayInput, int byteSize, int channels, int samplingRate, LoadOptions.ResamplingQuality resamplingQuality);

        [DllImport("nativeaudioe7")]
        internal static extern int playAudio(int sourceIndex, PlayOptions playOptions);
        [DllImport("nativeaudioe7")]
        internal static extern int disposeIfAllocated();
        [DllImport("nativeaudioe7")]
        internal static extern void unloadAudio(int sourceIndex);
        [DllImport("nativeaudioe7")]
        internal static extern float lengthBySource(int sourceIndex);

        // -- Operates on an audio track chosen for a particular audio --

        [DllImport("nativeaudioe7")]
        internal static extern int stopAudio(int playerIndex);
        [DllImport("nativeaudioe7")]
        internal static extern void setVolume(int playerIndex, float volume);
        [DllImport("nativeaudioe7")]
        internal static extern void setPan(int playerIndex, float pan);
        [DllImport("nativeaudioe7")]
        internal static extern float getPlaybackTime(int playerIndex);
        [DllImport("nativeaudioe7")]
        internal static extern void setPlaybackTime(int playerIndex, float offsetSeconds);
        [DllImport("nativeaudioe7")]
        internal static extern void trackPause(int playerIndex);
        [DllImport("nativeaudioe7")]
        internal static extern void trackResume(int playerIndex);

#endif
    }
}