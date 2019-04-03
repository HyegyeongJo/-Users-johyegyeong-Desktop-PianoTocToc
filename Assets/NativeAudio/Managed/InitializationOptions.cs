// Native Audio
// 5argon - Exceed7 Experiments
// Problems/suggestions : 5argon@exceed7.com

namespace E7.Native
{

    public partial class NativeAudio
    {
        /// <summary>
        /// This class is currently only contains options for Android.
        /// </summary>
        public class InitializationOptions
        {
            public static readonly InitializationOptions defaultOptions = new InitializationOptions();

            /// <summary>
            /// The no argument overload of <see cref="NativeAudioPointer.Play()"> round robin select audio player (`AudioTrack`) for you. 
            /// So this translate to the most concurrent audio amount you can have.
            /// 
            /// If you would like to learn about this AudioTrack limit problem please read !
            /// https://gametorrahod.com/androids-native-audio-primer-for-unity-developers-65acf66dd124
            /// 
            /// The gist :
            /// - You might get less than this number.
            /// - Even if you get this number of AudioTrack, not all of them might be granted fast path since the limit seems to be 7 fast AT per device.
            /// (You phone might took more than one at home screen, Unity take 1 by default, there is an other Unity game open, etc.)
            /// - There is an advanced <see cref="NativeAudioPointer.Play(PlayOptions)"> overload that instead of round robin select the player, 
            /// you directly specify which one you want via <see cref="NativeAudio.PlayOptions.audioPlayerIndex">.
            /// That number of course cannot be more than this number - 1.
            /// - You might not need that much concurrency and letting new sound cut of the old one is actually great for some rapid sounds. Even 1 AT might sound nicer than more.
            /// /// </summary>
            public int androidAudioTrackCount = 3;

            /// <summary>
            /// If -1 it uses buffer size exactly equal to device's native buffer size.
            /// Any number lower than device's native buffer size that is not -1 will be clamped to device's native buffer size as the lowest possible.
            /// 
            /// Any number larger than device's native buffer size, you will **not** get exactly that specified buffer size.
            /// Instead, we increase from device buffer size by multiple of itself until over the specified size, then you get that size. (Hence the name "minimum")
            /// [See the reason of the need to increase by multiple](https://developer.android.com/ndk/guides/audio/audio-latency#buffer-size).
            /// 
            /// Smaller buffer size means better latency.
            /// Therefore -1 means it is the best latency-wise. (Will not modify the buffer size asked from the device)
            /// 
            /// But if you experiences audio glitches, it might be that the device could not write in time 
            /// when the first buffer runs out of data, the "buffer underrun". (Native Audio uses double buffering)
            /// This might be because of device reports a buffer size too low for itself to handle. This is in some Chinese phones apparently.
            /// 
            /// Example : Specified 256
            /// - Xperia Z5 : Native buffer size : 192 -> what you get : 384
            /// - Lenovo A..something : Native buffer size : 620 -> what you get : 620
            /// </summary>
            public int androidMinimumBufferSize = -1;

            /// <summary>
            /// [Android] 
            /// If `true` the allocated audio sources on Android will not be freed when minimize the app. (The Unity ones do freed and request a new one on coming back) 
            /// This make it possible for audio played with Native Audio to play while minimizing the app, and also to not spend time disposing and allocating sources again.
            /// 
            /// However this is not good since it adds "wake lock" to your game.With `adb shell dumpsys power` while your game is minimized after using Native Audio you will see something like ` PARTIAL_WAKE_LOCK 'AudioMix' ACQ=-27s586ms(uid= 1041 ws= WorkSource{ 10331})`. Meaning that the OS have to keep the audio mix alive all the time.Not to mention most games do not really want this behaviour.
            /// 
            /// Most gamers I saw also minimized the game and sometimes forgot to close them off.This cause not only battery drain when there is a wake lock active, but also when the lock turns into `LONG` state it will show up as a warning in Google Play Store, as it could detect that an app has a [Stuck partial wake lock](https://developer.android.com/topic/performance/vitals/wakelock) or not.
            /// 
            /// With this as `false` (default), on <see cref="Initialize"> the native side will remember your request's spec. On minimize it will dispose all the sources (and in turn stopping them). On coming back it will reinitialize with the same spec.
            /// 
            /// [iOS] No effect, iOS's audio sources is already minimize-compatible but it is prevented by the app's build option.
            /// 
            /// If you want the audio to continue to be heard in minimize, use "Behaviour in background" set as Custom - Audio in Unity Player Settings + <a href="https://forum.unity.com/threads/how-do-i-get-the-audio-running-in-background-ios.319602/" target="_blank">follow this thread</a> to setup the <code>AVAudioSession</code> to correct settings.
            /// </summary>
    public bool preserveOnMinimize = false;
        }

    }
}