// Native Audio
// 5argon - Exceed7 Experiments
// Problems/suggestions : 5argon@exceed7.com

using System;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace E7.Native
{
    public partial class NativeAudio
    {
        /// <summary>
        /// Several properties about the device asked from the native side that might help you.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct DeviceAudioInformation
        {
#if UNITY_IOS
            /// <summary>
            /// It is from [AVAudioSessionPortDescription](https://developer.apple.com/documentation/avfoundation/avaudiosessionportdescription).
            /// </summary>
            public enum IosAudioPortType
            {
                //---Output---

                /// <summary>
                /// Line-level output to the dock connector.
                /// </summary>
                LineOut = 0,

                /// <summary>
                /// Output to a wired headset.
                /// </summary>
                Headphones = 1,

                /// <summary>
                /// Output to a speaker intended to be held near the ear.
                /// </summary>
                BuiltInReceiver = 2,

                /// <summary>
                /// Output to the device’s built-in speaker.
                /// </summary>
                BuiltInSpeaker = 3,

                /// <summary>
                /// Output to a device via the High-Definition Multimedia Interface (HDMI) specification.
                /// </summary>
                HDMI = 4,

                /// <summary>
                /// Output to a remote device over AirPlay.
                /// </summary>
                AirPlay = 5,

                /// <summary>
                /// Output to a Bluetooth Low Energy (LE) peripheral.
                /// </summary>
                BluetoothLE = 6,

                /// <summary>
                /// Output to a Bluetooth A2DP device.
                /// </summary>
                BluetoothA2DP = 7,

                //---Input---

                /// <summary>
                /// Line-level input from the dock connector.
                /// </summary>
                LineIn = 8,

                /// <summary>
                /// The built-in microphone on a device.
                /// </summary>
                BuiltInMic = 9,

                /// <summary>
                /// A microphone that is built-in to a wired headset.
                /// </summary>
                HeadsetMic = 10,

                //---Input-Output---

                /// <summary>
                /// Input or output on a Bluetooth Hands-Free Profile device.
                /// </summary>
                BluetoothHFP = 11,

                /// <summary>
                /// Input or output on a Universal Serial Bus device.
                /// </summary>
                UsbAudio = 12,

                /// <summary>
                /// Input or output via Car Audio.
                /// </summary>
                CarAudio = 13,

            }

            /// <summary>
            /// [iOS] All audio devices currently active.
            /// </summary>
            public IosAudioPortType[] audioDevices { get; private set; }

            public DeviceAudioInformation(IosAudioPortType[] portArray)
            {
                this.audioDevices = portArray;
            }

            public override string ToString()
            {
                return string.Format("Audio devices : {0}",
                string.Join(", ", this.audioDevices.Select(x => x.ToString()).ToArray()));
            }
#endif

#if UNITY_ANDROID
            /// <summary>
            /// [Android] Only audio matching this sampling rate on a native AudioTrack created with this sampling rate is eligible for fast track playing.
            /// </summary>
            public int nativeSamplingRate { get; private set; }

            /// <summary>
            /// [Android] How large of a buffer that your phone wants to work with.
            /// </summary>
            public int optimalBufferSize { get; private set; }
            
            /// <summary>
            /// [Android] Indicates a continuous output latency of 45 ms or less.
            /// Only valid if the device API >= 23 (6.0, Marshmallow), or else it is always `false`.
            /// </summary>
            public bool lowLatencyFeature { get; private set; }

            /// <summary>
            /// [Android] Indicates a continuous round-trip latency of 20 ms or less.
            /// Only valid if the device API >= 23 (6.0, Marshmallow), or else it is always `false`.
            /// </summary>
            public bool proAudioFeature { get; private set; }

            /// <summary>
            /// [Android] All output devices currently active.
            /// Only valid if the device API >= 23 (6.0, Marshmallow), or else it is always `null`.
            /// </summary>
            public AndroidAudioDeviceType[] audioDevices { get; private set; }

            /// <summary>
            /// I just copied everything from [here](https://developer.android.com/reference/android/media/AudioDeviceInfo.html#constants_2).
            /// </summary>
            public enum AndroidAudioDeviceType
            {
                TYPE_AUX_LINE         = 19,
                TYPE_BLUETOOTH_A2DP   = 8,
                TYPE_BLUETOOTH_SCO    = 7,
                TYPE_BUILTIN_EARPIECE = 1,
                TYPE_BUILTIN_MIC      = 15,
                TYPE_BUILTIN_SPEAKER  = 2,
                TYPE_BUS              = 21,
                TYPE_DOCK             = 13,
                TYPE_FM               = 14,
                TYPE_FM_TUNER         = 16,
                TYPE_HDMI             = 9,
                TYPE_HDMI_ARC         = 10,
                TYPE_HEARING_AID      = 23,
                TYPE_IP               = 20,
                TYPE_LINE_ANALOG      = 5,
                TYPE_LINE_DIGITAL     = 6,
                TYPE_TELEPHONY        = 18,
                TYPE_TV_TUNER         = 17,
                TYPE_UNKNOWN          = 0,
                TYPE_USB_ACCESSORY    = 12,
                TYPE_USB_DEVICE       = 11,
                TYPE_USB_HEADSET      = 22,
                TYPE_WIRED_HEADPHONES = 4,
                TYPE_WIRED_HEADSET    = 3,
            }


            public DeviceAudioInformation(AndroidJavaObject jo)
            {
                AndroidJavaClass versionClass = new AndroidJavaClass("android/os/Build$VERSION");
                int sdkLevel = versionClass.GetStatic<int>("SDK_INT");

                this.nativeSamplingRate = jo.Get<int>("nativeSamplingRate");
                this.optimalBufferSize = jo.Get<int>("optimalBufferSize");
                this.lowLatencyFeature = jo.Get<bool>("lowLatencyFeature");
                this.proAudioFeature = jo.Get<bool>("proAudioFeature");

                if (sdkLevel >= 23)
                {
                    //This one is a Java array, we need to do JNI manually to each elements
                    AndroidJavaObject outputDevicesJo = jo.Get<AndroidJavaObject>("outputDevices");

                    IntPtr outputDevicesRaw = outputDevicesJo.GetRawObject();
                    int outputDeviceAmount = AndroidJNI.GetArrayLength(outputDevicesRaw);

                    this.audioDevices = new AndroidAudioDeviceType[outputDeviceAmount];

                    for (int i = 0; i < outputDeviceAmount; i++)
                    {
                        IntPtr outputDevice = AndroidJNI.GetObjectArrayElement(outputDevicesRaw, i);
                        IntPtr audioDeviceInfoClass = AndroidJNI.GetObjectClass(outputDevice);
                        IntPtr getTypeMethod = AndroidJNIHelper.GetMethodID(audioDeviceInfoClass, "getType");
                        int type = AndroidJNI.CallIntMethod(outputDevice, getTypeMethod, new jvalue[] { });
                        this.audioDevices[i] = (AndroidAudioDeviceType)type;
                    }
                }
                else
                {
                    this.audioDevices = new AndroidAudioDeviceType[0];
                }

                //Debug.Log(this.ToString());
            }

            public override string ToString()
            {
                return string.Format("Native Sampling Rate: {0} | Optimal Buffer Size: {1} | Low Latency Feature: {2} | Pro Audio Feature: {3} | Output devices : {4}",
                nativeSamplingRate, optimalBufferSize, lowLatencyFeature, proAudioFeature,
                string.Join(", ", this.audioDevices.Select(x => x.ToString()).ToArray()));
            }
#endif
        }
    }
}