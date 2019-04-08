using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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
    private void Start()
    {
        play = score.text.Split("\n"[0]);
    }

    int node = 0;
    int prev = -1;
    void Update()
    {
        timer += Time.deltaTime;

        if (auto)
        {
            node = Mathf.FloorToInt(timer * BPM / 60) % play.Length;

            Debug.Log("Node.Length" + node);
            Debug.Log("Play.Length" + play.Length);

            if (node == play.Length-1)
            {
            //    Debug.Log("CLEAR MUSIC!!!!!@@@#####");
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
        foreach(char c in s.ToCharArray())
            for (int i = 0; i < keys.Length; i++)
                if (c == keys[i])
                {
                    GameObject n = GameObject.Instantiate(note.gameObject, start.position - Vector3.left * (note.transform.localScale.x /5.2f* i), start.rotation);
                    n.name = tones[i].name;
                    n.GetComponent<Note>().tone = tones[i];
                }
    }
}
