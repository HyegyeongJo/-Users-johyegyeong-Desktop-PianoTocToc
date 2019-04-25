using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCreator : MonoBehaviour
{
    public delegate void ClearMusicEventHandler();
    public static event ClearMusicEventHandler ClearMusic = () => { };

    public TextAsset score;
    public AudioClip[] tones;
    public char[] keys;

    public Transform start;
    public Note note;

    public bool auto;
    public int BPM;

    float timer = 0;

    string[] play;

    int node = 0;
    int prev = -1;


    private void Start()
    {
        play = score.text.Split("\n"[0]);
    }


    private void Update()
    {
        timer += Time.deltaTime;
        if (auto)
        {
            node = Mathf.FloorToInt(timer * BPM / 60) % play.Length;

            if (node == play.Length - 1)
            {
                ClearMusic();
                auto = false;
            }


            if (prev != node)
            {
                prev = node;
                Play(play[node]);
            }

        }
        else
        {
            if (Input.inputString.Length > 0)
                Play(Input.inputString.ToUpper());
        }
    }

    void Play(string s)
    {
        foreach (char c in s.ToCharArray())
            for (int i = 0; i < keys.Length; i++)
                if (c == keys[i])
                {
                 GameObject n = Instantiate(note.gameObject, start.position - Vector3.left * (note.transform.localScale.x * i * .295f), start.rotation);
                    n.name = tones[i].name;
                    n.GetComponent<Note>().tone = tones[i];
                }
    }
}
