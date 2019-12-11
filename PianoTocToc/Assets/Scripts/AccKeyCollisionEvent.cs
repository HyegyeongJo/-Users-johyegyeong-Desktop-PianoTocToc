using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccKeyCollisionEvent : MonoBehaviour
{
    public AudioClip[] accKeyNote;
    private AudioSource accKeySource;

    private void Start()
    {
        accKeySource = GetComponent<AudioSource>();

        for(int i =0; i<accKeyNote.Length; i++)
        {
            accKeySource.clip = accKeyNote[i];
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Acc_01_Do")
        {
            accKeySource.PlayOneShot(accKeyNote[0]);
        }
        if (col.gameObject.tag == "Acc_02_Re")
        {
            accKeySource.PlayOneShot(accKeyNote[1]);
        }
        if (col.gameObject.tag == "Acc_03_Mi")
        {
            accKeySource.PlayOneShot(accKeyNote[2]);
        }
        if (col.gameObject.tag == "Acc_04_Fa")
        {
            accKeySource.PlayOneShot(accKeyNote[3]);
        }
        if (col.gameObject.tag == "Acc_05_Sol")
        {
            accKeySource.PlayOneShot(accKeyNote[4]);
        }
        if (col.gameObject.tag == "Acc_06_Ra")
        {
            accKeySource.PlayOneShot(accKeyNote[5]);
        }
        if (col.gameObject.tag == "Acc_07_Si")
        {
            accKeySource.PlayOneShot(accKeyNote[6]);
        }
        if (col.gameObject.tag == "Acc_08_Do")
        {
            accKeySource.PlayOneShot(accKeyNote[7]);
        }
        if (col.gameObject.tag == "Acc_09_Re")
        {
            accKeySource.PlayOneShot(accKeyNote[8]);
        }
        if (col.gameObject.tag == "Acc_10_Mi")
        {
            accKeySource.PlayOneShot(accKeyNote[9]);
        }
        if (col.gameObject.tag == "Acc_11_Fa")
        {
            accKeySource.PlayOneShot(accKeyNote[10]);
        }
        if (col.gameObject.tag == "Acc_12_Sol")
        {
            accKeySource.PlayOneShot(accKeyNote[11]);
        }
        if (col.gameObject.tag == "Acc_13_Ra")
        {
            accKeySource.PlayOneShot(accKeyNote[12]);
        }
        if (col.gameObject.tag == "Acc_14_Si")
        {
            accKeySource.PlayOneShot(accKeyNote[13]);
        }
        if (col.gameObject.tag == "Acc_15_Do")
        {
            accKeySource.PlayOneShot(accKeyNote[14]);
        }
        if (col.gameObject.tag == "Acc_16_Re")
        {
            accKeySource.PlayOneShot(accKeyNote[15]);
        }
        if (col.gameObject.tag == "Acc_17_Mi")
        {
            accKeySource.PlayOneShot(accKeyNote[16]);
        }
        if (col.gameObject.tag == "Acc_18_Fa")
        {
            accKeySource.PlayOneShot(accKeyNote[17]);
        }
        if (col.gameObject.tag == "Acc_19_Sol")
        {
            accKeySource.PlayOneShot(accKeyNote[18]);
        }
        if (col.gameObject.tag == "Acc_20_Ra")
        {
            accKeySource.PlayOneShot(accKeyNote[19]);
        }
        if (col.gameObject.tag == "Acc_21_Si")
        {
            accKeySource.PlayOneShot(accKeyNote[20]);
        }
        if (col.gameObject.tag == "Acc_22_Do")
        {
            accKeySource.PlayOneShot(accKeyNote[21]);
        }
        if (col.gameObject.tag == "Acc_23_Re")
        {
            accKeySource.PlayOneShot(accKeyNote[22]);
        }
        if (col.gameObject.tag == "Acc_24_Mi")
        {
            accKeySource.PlayOneShot(accKeyNote[23]);
        }
        if (col.gameObject.tag == "Acc_25_Fa")
        {
            accKeySource.PlayOneShot(accKeyNote[24]);
        }
        if (col.gameObject.tag == "Acc_26_Sol")
        {
            accKeySource.PlayOneShot(accKeyNote[25]);
        }
        if (col.gameObject.tag == "Acc_27_Ra")
        {
            accKeySource.PlayOneShot(accKeyNote[26]);
        }
        if (col.gameObject.tag == "Acc_28_Si")
        {
            accKeySource.PlayOneShot(accKeyNote[27]);
        }
    }

}
