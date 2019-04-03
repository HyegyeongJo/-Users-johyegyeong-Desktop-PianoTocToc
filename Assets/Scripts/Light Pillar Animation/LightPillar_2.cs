using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar_2 : MonoBehaviour
{
    public Animator lightAnim;

    private void OnEnable()
    {
        LightPillarEvent.RePillar_2 += LightPillarAnimation;
    }

    private void OnDisable()
    {
        LightPillarEvent.RePillar_2 -= LightPillarAnimation;

    }

    void LightPillarAnimation()
    {
        lightAnim.SetTrigger("LightPillar");
    }
}
