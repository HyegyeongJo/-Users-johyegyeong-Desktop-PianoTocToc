using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKeyCollisionEvent : MonoBehaviour
{
    //public AudioClip[] pianoNote;
    //private AudioSource pianoNoteSource;
   // public Transform collisionParticle;

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

    public static event PianoKeyCheckEventHandler IDo_1 = () => { };
    public static event PianoKeyCheckEventHandler IRe_2 = () => { };
    public static event PianoKeyCheckEventHandler IMi_3 = () => { };
    public static event PianoKeyCheckEventHandler IFa_4 = () => { };
    public static event PianoKeyCheckEventHandler ISol_5 = () => { };
    public static event PianoKeyCheckEventHandler IRa_6 = () => { };
    public static event PianoKeyCheckEventHandler ISi_7 = () => { };
    public static event PianoKeyCheckEventHandler IDo_8 = () => { };
    public static event PianoKeyCheckEventHandler IRe_9 = () => { };
    public static event PianoKeyCheckEventHandler IMi_10 = () => { };
    public static event PianoKeyCheckEventHandler IFa_11 = () => { };
    public static event PianoKeyCheckEventHandler ISol_12 = () => { };
    public static event PianoKeyCheckEventHandler IRa_13 = () => { };
    public static event PianoKeyCheckEventHandler ISi_14 = () => { };
    public static event PianoKeyCheckEventHandler IDo_15 = () => { };
    public static event PianoKeyCheckEventHandler IRe_16 = () => { };
    public static event PianoKeyCheckEventHandler IMi_17 = () => { };

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "1_Do")
        {
            DoOn_1();
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "2_Re")
        {
            ReOn_2();
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "3_Mi")
        {
            MiOn_3();
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "4_Fa")
        {
            FaOn_4();
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "5_Sol")
        {
            SolOn_5();
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "6_Ra")
        {
            RaOn_6();
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "7_Si")
        {
            SiOn_7();
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "8_Do")
        {
            DoOn_8();
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "9_Re")
        {
          ReOn_9();
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "10_Mi")
        {
          MiOn_10();
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "11_Fa")
        {
         FaOn_11();
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "12_Sol")
        {
         SolOn_12();
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "13_Ra")
        {
           RaOn_13();
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "14_Si")
        {
            SiOn_14();
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "15_Do")
        {
           DoOn_15();
            Destroy(gameObject);
        }






        if (col.gameObject.tag == "1_IDo")
        {
            IDo_1();
        }
        if (col.gameObject.tag == "2_IRe")
        {
            IRe_2();
        }
        if (col.gameObject.tag == "3_IMi")
        {
            IMi_3();
        }
        if (col.gameObject.tag == "4_IFa")
        {
            IFa_4();
        }
        if (col.gameObject.tag == "5_ISol")
        {
            ISol_5();
        }
        if (col.gameObject.tag == "6_IRa")
        {
            IRa_6();
        }
        if (col.gameObject.tag == "7_ISi")
        {
            ISi_7();
        }
        if (col.gameObject.tag == "8_IDo")
        {
            IDo_8();
        }
        if (col.gameObject.tag == "9_IRe")
        {
            IRe_9();
        }
        if (col.gameObject.tag == "10_IMi")
        {
            IMi_10();
        }
        if (col.gameObject.tag == "11_IFa")
        {
            IFa_11();
        }
        if (col.gameObject.tag == "12_ISol")
        {
            ISol_12();
        }
        if (col.gameObject.tag == "13_IRa")
        {
            IRa_13();
        }
        if (col.gameObject.tag == "14_ISi")
        {
            ISi_14();
        }
        if (col.gameObject.tag == "15_IDo")
        {
            IDo_15();
        }
    }

}
