using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E7.Native;

public class PianoKeyPressedAudio : MonoBehaviour
{
    public AudioClip pianoNote;

    NativeAudioPointer nativeAudioPointer;
    NativeAudio.PlayOptions playOptions;

    Coroutine playCrt;

    private void Start()
    {
        NativeAudio.Initialize();
        nativeAudioPointer = NativeAudio.Load(pianoNote);
        playOptions = NativeAudio.PlayOptions.defaultOptions;
        playOptions.volume = 0f;
        playOptions.trackLoop = false;
    }

    private void OnDisable()
    {
        nativeAudioPointer.Unload();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayFrom(0f);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            PlayFrom(2f);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            PlayFrom(4f);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            PlayFrom(6f);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            PlayFrom(8f);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            PlayFrom(10f);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            PlayFrom(12f);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            PlayFrom(14f);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            PlayFrom(16f);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            PlayFrom(18f);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            PlayFrom(20f);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            PlayFrom(22f);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            PlayFrom(24f);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            PlayFrom(26f);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            PlayFrom(28f);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            PlayFrom(30f);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayFrom(32f);
        }
    }

    int trackIndex = 0;
    private void PlayFrom(float offset)
    {
        playOptions.volume = 1f;
        playOptions.offsetSeconds = offset;
        playOptions.audioPlayerIndex = trackIndex++ % NativeAudio.InitializationOptions.defaultOptions.androidAudioTrackCount;
        NativeAudioController nativeAudioController = nativeAudioPointer.Play(playOptions);
        //if (playCrt != null)
        //{
        //    StopCoroutine(playCrt);
        //}
        playCrt = StartCoroutine(DecreaseVolume(nativeAudioController));
    }

    IEnumerator DecreaseVolume(NativeAudioController ctrl)
    {
        float volume = 1f;
        while (volume > 0f)
        {
            volume -= Time.deltaTime;
            volume = Mathf.Clamp01(volume);
            ctrl.SetVolume(volume);
            yield return null;
        }
        ctrl.Stop();
    }
}
