using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accompaniment_Player : MonoBehaviour
{
    public TextAsset score;
    public AudioClip[] tones;
    public char[] keys;

    public Transform start;
    public Accompaniment_Note note;

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
                    GameObject n = GameObject.Instantiate(note.gameObject, start.position - Vector3.left * (note.transform.localScale.x / 2 * i), start.rotation);
                    n.name = tones[i].name;
                    n.GetComponent<Accompaniment_Note>().tone = tones[i];
                }
    }
}
