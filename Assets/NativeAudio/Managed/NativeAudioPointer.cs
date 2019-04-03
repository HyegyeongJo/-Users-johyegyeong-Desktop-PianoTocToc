// Native Audio
// 5argon - Exceed7 Experiments
// Problems/suggestions : 5argon@exceed7.com

using System;
using UnityEngine;

namespace E7.Native
{
    /// <summary>
	/// When you load an audio natively your audio is being kept at native side's memory. This "pointer" is actually just an `int`
	/// that the native side can use to point to the correct audio memory. <see cref="Play(NativeAudio.PlayOptions)"> is just sending this `int`
	/// to native side so it put that audio to a selected source (automatically selected, or via <see cref="NativeAudio.PlayOptions">.)
	/// 
    /// Please do not create an instance of this class by yourself! Call <see cref="NativeAudio.Load"> to get it.
    /// </summary>
    public class NativeAudioPointer 
    {
		private string soundPath;
		private int startingIndex;

		/// <summary>
		/// Some implementation in the future may need you to specify concurrent amount for each audio upfront so I prepared this field.
		/// But it is always 1 for now. It has no effect because both iOS and Android implementation automatically rotate players on play.
		/// and so you get the concurrent amount equal to amount of players shared for all sounds, not just this one sound.
		/// </summary>
		private int amount;
		private bool isUnloaded;

		/// <summary>
		/// Cached length in SECONDS of a loaded audio calculated from PCM byte size and specifications.
		/// </summary>
		public float Length { get; private set; }

		//[iOS] When preparing to play, OpenAL need you to remember some information so that the next Play will use the prepared things correctly.
#if UNITY_IOS
		private int prepareIndex;
		private bool prepared;

        /// <summary>
        /// Indicate that the source had went through <see cref="Prepare"> before. The next play of this source could be a bit faster.
        /// It will becomes `false` automatically on next <see cref="Play"> call. Currently there is no way to manually clear out <see cref="Prepared"> status.
        /// </summary>
        public bool Prepared { get { return prepared; } }
#endif
        private int currentIndex;

        /// <summary>
        /// This will automatically cycles for you if the amount is not 1.
        /// </summary>
        public int NextIndex
		{
            get
            {
				int toReturn = currentIndex;
				currentIndex = currentIndex + 1;
                if (currentIndex > startingIndex + amount - 1)
				{
					currentIndex = startingIndex;
				}
				return toReturn;
			}
		}

        /// <param name="amount">Right now amount is not used anywhere yet.</param>
        public NativeAudioPointer(string soundPath, int index, float length, int amount = 1)
		{
            this.soundPath = soundPath;
			this.startingIndex = index;
			this.amount = amount;
			this.Length = length;

			this.currentIndex = index;
		}

        private void AssertLoadedAndInitialized()
        {
            if (isUnloaded)
            {
                throw new InvalidOperationException("You cannot use an unloaded NativeAudioPointer.");
            }

            if (NativeAudio.Initialized == false)
            {
                throw new InvalidOperationException("You cannot use NativeAudioPointer while Native Audio itself is not in initialized state.");
            }
        }


        /// <summary>
        /// Plays an audio using the underlying OS's native method. A number stored in this object will be used to determine which loaded audio data at native side that we would like to play. If you previously call <see cref="Prepare"> it will take effect here.
		/// You can adjust volume and others immediately via <see cref="NativeAudio.PlayOptions"> or adjust later via the returned <see cref="NativeAudioController">.
		/// 
		/// [iOS] (Unprepared) Native Audio remembered which of the total of 16 OpenAL source that we just played. It will get the next one (wraps to the first instance when already at 16th instance), find the correct sound buffer and assign it to that source, then play it.
		/// 
		/// [iOS] (Prepared) Instead of using buffer index (the sound, not the sound player) it will play the source at the time you call <see cref="Prepare"> immediately without caring if the sound in that source is currently the same as when you call <see cref="Prepare"> or not. After calling this, the prepare status will be reset to unprepared, and the next <see cref="Play"> will use buffer index as usual.
		/// 
		/// [Android] Use the index stored in this <see cref="NativeAudioPointer"> as an index to unmanaged audio data array loaded at OpenSL ES part. (C part) The code remembered which OpenSL ES AudioPlayer was used the last time and this will get you the next one. Loop back to the first player when already at the final player.
        /// </summary>
		/// <returns>With the controller you can further set the volume, panning, stop, etc. to the audio player that was just chosen to play your audio.</returns>
        public NativeAudioController Play()
        {
            return Play(NativeAudio.PlayOptions.defaultOptions);
        }

        /// <summary>
        /// Plays an audio using the underlying OS's native method. A number stored in this object will be used to determine which loaded audio data at native side that we would like to play. If you previously call <see cref="Prepare"> it will take effect here.
		/// You can adjust volume and others immediately via <see cref="NativeAudio.PlayOptions"> or adjust later via the returned <see cref="NativeAudioController">.
		/// 
		/// [iOS] (Unprepared) Native Audio remembered which of the total of 16 OpenAL source that we just played. It will get the next one (wraps to the first instance when already at 16th instance), find the correct sound buffer and assign it to that source, then play it.
		/// 
		/// [iOS] (Prepared) Instead of using buffer index (the sound, not the sound player) it will play the source at the time you call <see cref="Prepare"> immediately without caring if the sound in that source is currently the same as when you call <see cref="Prepare"> or not. After calling this, the prepare status will be reset to unprepared, and the next <see cref="Play"> will use buffer index as usual.
		/// 
		/// [Android] Use the index stored in this <see cref="NativeAudioPointer"> as an index to unmanaged audio data array loaded at OpenSL ES part. (C part) The code remembered which OpenSL ES AudioPlayer was used the last time and this will get you the next one. Loop back to the first player when already at the final player.
        /// </summary>
		/// <returns>With the controller you can further set the volume, panning, stop, etc. to the audio player that was just chosen to play your audio.</returns>
		/// <exception cref="InvalidOperationException">Thrown when you attempt to play an unloaded audio.</exception>
        public NativeAudioController Play(NativeAudio.PlayOptions playOptions)
        {
			AssertLoadedAndInitialized();
            int playedSourceIndex = -1;
#if UNITY_IOS
            if (prepared)
            {
				//This is using source index. It means we have already loaded our sound to that source with Prepare.
                NativeAudio._PlayAudioWithSourceCycle(this.prepareIndex, playOptions);
				playedSourceIndex = this.prepareIndex;
            }
            else
            {
				//-1 audioPlayerIndex results in round-robin, 0~15 results in hard-specifying the track.
				playedSourceIndex = NativeAudio._PlayAudio(this.NextIndex, playOptions.audioPlayerIndex, playOptions);
            }
            prepared = false;
#elif UNITY_ANDROID
            playedSourceIndex = NativeAudio.playAudio(this.NextIndex, playOptions);
#endif
            return new NativeAudioController(playedSourceIndex);
        }


        /// <summary>
        /// Try to shave off as much start up time as possible to play a sound, depending on platforms.
		/// It affects **only the next play** from this <see cref="NativeAudioPointer"> and then automatically clears the <see cref="Prepared"> status from the pointer.
		/// Currently there is no way to cancel the <see cref="Prepared"> status other than playing.
		/// 
		/// The majority of load time is already in `Load` but `Prepare` might help a bit more, or not at all. 
		/// The effectiveness depends on platform's audio library's approach :
		/// 
        /// [iOS] Normally on <see cref="Play"> OpenAL will 
		/// 
		/// 1. Choose a source at native side, depending on your <see cref="NativeAudio.PlayOptions"> when using <see cref="Play(NativeAudio.PlayOptions)"> if manually. Or automatically if not specifying any source.
		/// 2. Stop that source, and then assign a new audio buffer to it.
		/// 3. Play that source.
		/// 
		/// <see cref="Prepare"> make it do 1. and 2. preemptively then remember a selected source from 1. in C# side. (So it also stop whatever is playing at that source, but not playing a new one yet.) The next <see cref="Play"> you call will immediately play this remembered source without caring what sound is in it.
		/// 
		/// If you use an option version <see cref="Play(NativeAudio.PlayOptions)"> after <see cref="Prepare"> and you still specifying <see cref="NativeAudio.PlayOptions.audioPlayerIndex"> it will be ignored. The remembered source from prepare takes priority.
		/// 
		/// If in between <see cref="Prepare"> and <see cref="Play"> you have played 16 sounds, the resulting sound will be something else as other sound has already put their buffer into the source you have remembered. Producing unexpected behaviour. So I recommend you prepare only if you will be playing very soon.
		/// 
		/// Likely, this gain is very negligible. (depending on how efficient OpenAL "assign the buffer to source" under the hood) But I have to do what I could.
		/// 
        /// [Android] No effect as OpenSL ES play audio by pushing data into `SLAndroidSimpleBufferQueueItf`. All the prepare is already at the <see cref="NativeAudio.Load(AudioClip)">.
        /// </summary>
        public void Prepare()
        {
			AssertLoadedAndInitialized();
#if UNITY_IOS
            prepareIndex = NativeAudio._PrepareAudio(NextIndex);
			prepared = true;
#elif UNITY_ANDROID
			//There is no possible preparation for OpenSL ES at the moment..
#endif
        }

		public override string ToString()
		{
			return soundPath;
		}

        /// <summary>
        /// You cannot call <see cref="Play"> anymore after unloading. It will throw an exception if you do so.
		/// 
		/// [iOS] Unload OpenAL buffer. The total number of 16 OpenAL source does not change. Immediately stop the sound if it is playing.
		/// 
		/// [Android] `free` the unmanaged audio data array at C code part of OpenSL ES code.
		/// 
		/// It is HIGHLY recommended to stop those audio player via <see cref="NativeAudioController.Stop"> before unloading because the play head will continue 
		/// running into oblivion if you unload data while it is still reading. I have seen 2 cases : 
		/// 
		/// - The game immediately crash with signal 11 (SIGSEGV), code 1 (SEGV_MAPERR) on my 36$ low end phone. Probably it does not permit freed memory reading.
		/// - In some device it produce scary noisehead sound if you load something new, `malloc` decided to use the same memory area you just freed,
		/// and the still running playhead pick that up.
        /// </summary>
        public void Unload()
        {
			if(!isUnloaded)
			{
#if UNITY_IOS
				NativeAudio._UnloadAudio(startingIndex);
				isUnloaded = true;
#elif UNITY_ANDROID
				for(int i = startingIndex; i < startingIndex + amount; i++)
				{
					NativeAudio.unloadAudio(i);
				}
#endif
				isUnloaded = true;
			}
        }

    }
}