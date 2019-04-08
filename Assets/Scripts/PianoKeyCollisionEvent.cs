using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKeyCollisionEvent : MonoBehaviour
{
    //public AudioClip[] pianoNote;
    //private AudioSource pianoNoteSource;
    public Transform collisionParticle;

    public delegate void PianoKeyCheckEventHandler();
    public static event PianoKeyCheckEventHandler DoOn_1 = () => { };
    public static event PianoKeyCheckEventHandler ReOn_2 = () => { };
    public static event PianoKeyCheckEventHandler MiOn_3 = () => { };
    public static event PianoKeyCheckEventHandler FaOn_4 = () => { };
    public static event PianoKeyCheckEventHandler SolOn_5 = () => { };
    public static event PianoKeyCheckEventHandler RaOn_6 = () => { };
    public static event PianoKeyCheckEventHandler SiOn_7 = () => { };
    public static event PianoKeyCheckEventHandler DoOn_8 = () => { };
    public static event PianoKeyCheckEventHandler ReOn_9 = () => { };
    public static event PianoKeyCheckEventHandler MiOn_10 = () => { };
    public static event PianoKeyCheckEventHandler FaOn_11 = () => { };
    public static event PianoKeyCheckEventHandler SolOn_12 = () => { };
    public static event PianoKeyCheckEventHandler RaOn_13 = () => { };
    public static event PianoKeyCheckEventHandler SiOn_14 = () => { };
    public static event PianoKeyCheckEventHandler DoOn_15 = () => { };
    public static event PianoKeyCheckEventHandler ReOn_16 = () => { };
    public static event PianoKeyCheckEventHandler MiOn_17 = () => { };



    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "1_Do")
        {
            DoOn_1();
            Destroy(gameObject);
            Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        }
        if (col.gameObject.tag == "2_Re")
        {
            ReOn_2();
            Destroy(gameObject);
            Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        }
        if (col.gameObject.tag == "3_Mi")
        {
            MiOn_3();
            Destroy(gameObject);
            Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        }
        if (col.gameObject.tag == "4_Fa")
        {
            FaOn_4();
            Destroy(gameObject);
            Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        }
        if (col.gameObject.tag == "5_Sol")
        {
            SolOn_5();
            Destroy(gameObject);
            Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        }
        if (col.gameObject.tag == "6_Ra")
        {
            RaOn_6();
            Destroy(gameObject);
            Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        }
        if (col.gameObject.tag == "7_Si")
        {
            SiOn_7();
            Destroy(gameObject);
            Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        }
        if (col.gameObject.tag == "8_Do")
        {
            DoOn_8();
            Destroy(gameObject);
            Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        }
        if (col.gameObject.tag == "9_Re")
        {
          ReOn_9();
            Destroy(gameObject);
            Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        }
        if (col.gameObject.tag == "10_Mi")
        {
          MiOn_10();
            Destroy(gameObject);
            Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        }
        if (col.gameObject.tag == "11_Fa")
        {
         FaOn_11();
            Destroy(gameObject);
            Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        }
        if (col.gameObject.tag == "12_Sol")
        {
         SolOn_12();
            Destroy(gameObject);
            Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        }
        if (col.gameObject.tag == "13_Ra")
        {
           RaOn_13();
            Destroy(gameObject);
            Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        }
        if (col.gameObject.tag == "14_Si")
        {
            SiOn_14();
            Destroy(gameObject);
            Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        }
        if (col.gameObject.tag == "15_Do")
        {
           DoOn_15();
            Destroy(gameObject);
            Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        }
        if (col.gameObject.tag == "16_Re")
        {
            ReOn_16();
            Destroy(gameObject);
            Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        }
        if (col.gameObject.tag == "16_Re")
        {
            MiOn_17();
            Destroy(gameObject);
            Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        }

    }

}
