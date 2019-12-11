// Native Audio
// 5argon - Exceed7 Experiments
// Problems/suggestions : 5argon@exceed7.com

using System;
using System.Runtime.InteropServices;

namespace E7.Native
{
    public partial class NativeAudio
    {
#if UNITY_IOS

        [DllImport("__Internal")]
        internal static extern int _Initialize();

        [DllImport("__Internal")]
        internal static extern int _SendByteArray(IntPtr byteArrayInput, int byteSize, int channels, int samplingRate, LoadOptions.ResamplingQuality resamplingQuality);

        [DllImport("__Internal")]
        internal static extern int _LoadAudio(string soundUrl, int resamplingQuality);

        [DllImport("__Internal")]
        internal static extern int _PrepareAudio(int bufferIndex);

        [DllImport("__Internal")]
        internal static extern int _PlayAudio(int bufferIndex, int sourceCycle, PlayOptions playOptions);

        [DllImport("__Internal")]
        internal static extern void _PlayAudioWithSourceCycle(int sourceCycle, PlayOptions playOptions);

        [DllImport("__Internal")]
        internal static extern void _UnloadAudio(int bufferIndex);

        [DllImport("__Internal")]
        internal static extern float _LengthBySource(int bufferIndex);

        [DllImport("__Internal")]
        internal static extern void _GetAudioDevices(IntPtr enumArray);

        // -- Operates on sound "source" chosen for a particular audio --
        // ("source" terms of OpenAL is like a speaker, not the "source of data" which is a loaded byte array.)

        [DllImport("__Internal")]
        internal static extern void _StopAudio(int sourceCycle);

        [DllImport("__Internal")]
        internal static extern void _SetVolume(int sourceCycle, float volume);

        [DllImport("__Internal")]
        internal static extern void _SetPan(int sourceCycle, float pan);

        [DllImport("__Internal")]
        internal static extern float _GetPlaybackTime(int sourceCycle);

        [DllImport("__Internal")]
        internal static extern void _SetPlaybackTime(int sourceCycle, float offsetSeconds);

        [DllImport("__Internal")]
        internal static extern void _TrackPause(int sourceCycle);

        [DllImport("__Internal")]
        internal static extern void _TrackResume(int sourceCycle);
#endif
    }
}