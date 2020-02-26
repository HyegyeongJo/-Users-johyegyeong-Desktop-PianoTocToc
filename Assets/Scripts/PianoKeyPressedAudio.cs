using UnityEngine;

public class PianoKeyPressedAudio : MonoBehaviour
{
    [SerializeField] AudioClip[] notes;
    AudioSource player;

    //NativeAudioPointer[] pointers;
    //NativeAudioController[] controllers;
  // int audioTrackIndex, audioTrackCount = 5;

    void Awake()
    {
        //SetDspBufferSize(64);

        player = gameObject.AddComponent<AudioSource>();

        //NativeAudio.Initialize(new NativeAudio.InitializationOptions
        //{
        //    androidAudioTrackCount = audioTrackCount,
        //});
        //pointers = new NativeAudioPointer[notes.Length];
        //controllers = new NativeAudioController[notes.Length];
    }

    void SetDspBufferSize(int bufferLenfth)
    {
        var config = AudioSettings.GetConfiguration();
        config.dspBufferSize = bufferLenfth;
        AudioSettings.Reset(config);
        AudioSettings.GetDSPBufferSize(out int bufferLength, out int numBuffers);
        Debug.Log(string.Format("dspBufferLength: {0}, numBuffers: {1}", bufferLength, numBuffers));
    }

    void Start()
    {
        //for (int i = 0; i < notes.Length; i++)
        //{
        //    // pointers[i] = NativeAudio.Load(notes[i]);
        //    pointers[i] = notes[i];
        //}
    }

    void Update()
    {

    }

    void PlayWithUniyAudio()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            player.PlayOneShot(notes[0]);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            player.PlayOneShot(notes[1]);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            player.PlayOneShot(notes[2]);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            player.PlayOneShot(notes[3]);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            player.PlayOneShot(notes[4]);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            player.PlayOneShot(notes[5]);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            player.PlayOneShot(notes[6]);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            player.PlayOneShot(notes[7]);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            player.PlayOneShot(notes[8]);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            player.PlayOneShot(notes[9]);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            player.PlayOneShot(notes[10]);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            player.PlayOneShot(notes[11]);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            player.PlayOneShot(notes[12]);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            player.PlayOneShot(notes[13]);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            player.PlayOneShot(notes[14]);
        }
    }

    //void PlayWithNativeAudio()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        controllers[0] = PlayWithNativeAudio(pointers[0]);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.B))
    //    {
    //        controllers[0] = PlayWithNativeAudio(pointers[1]);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        controllers[0] = PlayWithNativeAudio(pointers[2]);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        controllers[0] = PlayWithNativeAudio(pointers[3]);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        controllers[0] = PlayWithNativeAudio(pointers[4]);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        controllers[0] = PlayWithNativeAudio(pointers[5]);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.G))
    //    {
    //        controllers[0] = PlayWithNativeAudio(pointers[6]);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.H))
    //    {
    //        controllers[0] = PlayWithNativeAudio(pointers[7]);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.I))
    //    {
    //        controllers[0] = PlayWithNativeAudio(pointers[8]);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.J))
    //    {
    //        controllers[0] = PlayWithNativeAudio(pointers[9]);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        controllers[0] = PlayWithNativeAudio(pointers[10]);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        controllers[0] = PlayWithNativeAudio(pointers[11]);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.M))
    //    {
    //        controllers[0] = PlayWithNativeAudio(pointers[12]);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.N))
    //    {
    //        controllers[0] = PlayWithNativeAudio(pointers[13]);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.O))
    //    {
    //        controllers[0] = PlayWithNativeAudio(pointers[14]);
    //    }
    }

    //NativeAudioController PlayWithNativeAudio(NativeAudioPointer ptr)
    //{
    //    var options = NativeAudio.PlayOptions.defaultOptions;
    //    options.audioPlayerIndex = audioTrackIndex;
    //    options.offsetSeconds = 0f;
    //    audioTrackIndex = (audioTrackIndex + 1) % audioTrackCount;
    //    return ptr.Play();
    //}
//}
