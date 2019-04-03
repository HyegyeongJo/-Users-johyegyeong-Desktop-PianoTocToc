using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillarEvent : MonoBehaviour
{
    public delegate void LightPillarCollisionEventHandler();
    public static event LightPillarCollisionEventHandler DoPillar_1 = () => { };
    public static event LightPillarCollisionEventHandler RePillar_2 = () => { };
    public static event LightPillarCollisionEventHandler MiPillar_3 = () => { };
    public static event LightPillarCollisionEventHandler FaPillar_4 = () => { };
    public static event LightPillarCollisionEventHandler SolPillar_5 = () => { };
    public static event LightPillarCollisionEventHandler RaPillar_6 = () => { };
    public static event LightPillarCollisionEventHandler SiPillar_7 = () => { };
    public static event LightPillarCollisionEventHandler DoPillar_8 = () => { };
    public static event LightPillarCollisionEventHandler RePillar_9 = () => { };
    public static event LightPillarCollisionEventHandler MiPillar_10 = () => { };
    public static event LightPillarCollisionEventHandler FaPillar_11 = () => { };
    public static event LightPillarCollisionEventHandler SolPillar_12 = () => { };
    public static event LightPillarCollisionEventHandler RaPillar_13 = () => { };
    public static event LightPillarCollisionEventHandler SiPillar_14 = () => { };
    public static event LightPillarCollisionEventHandler DoPillar_15 = () => { };
    public static event LightPillarCollisionEventHandler RePillar_16 = () => { };
    public static event LightPillarCollisionEventHandler MiPillar_17 = () => { };

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "P_1")
        {
            DoPillar_1();
            Debug.Log("Pillar Animation Ready");
        }
        if (col.gameObject.tag == "P_2")
        {
            RePillar_2();
            Debug.Log("Pillar Animation Ready");

        }
        if (col.gameObject.tag == "P_3")
        {
            MiPillar_3();
            Debug.Log("Pillar Animation Ready");

        }
        if (col.gameObject.tag == "P_4")
        {
            FaPillar_4();
            Debug.Log("Pillar Animation Ready");

        }
        if (col.gameObject.tag == "P_5")
        {
            SolPillar_5();
            Debug.Log("Pillar Animation Ready");

        }
        if (col.gameObject.tag == "P_6")
        {
            RaPillar_6();
        }
        if (col.gameObject.tag == "P_7")
        {
            SiPillar_7();
        }
        if (col.gameObject.tag == "P_8")
        {
            DoPillar_8();
        }
        if (col.gameObject.tag == "P_9")
        {
            RePillar_9();
        }
        if (col.gameObject.tag == "P_10")
        {
            MiPillar_10();
        }
        if (col.gameObject.tag == "P_11")
        {
            FaPillar_11();
        }
        if (col.gameObject.tag == "P_12")
        {
            SolPillar_12();
        }
        if (col.gameObject.tag == "P_13")
        {
            RaPillar_13();
        }
        if (col.gameObject.tag == "P_14")
        {
            SiPillar_14();
        }
        if (col.gameObject.tag == "P_15")
        {
            DoPillar_15();
        }
        if (col.gameObject.tag == "P_16")
        {
            RePillar_16();
        }
        if (col.gameObject.tag == "P_17")
        {
            MiPillar_17();
        }

    }
}
