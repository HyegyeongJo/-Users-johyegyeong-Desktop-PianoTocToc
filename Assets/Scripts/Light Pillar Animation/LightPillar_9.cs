using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar_9 : MonoBehaviour
{
    public Animator lightAnim;

    private void OnEnable()
    {
        LightPillarEvent.RePillar_9 += LightPillarAnimation;
    }

    private void OnDisable()
    {
        LightPillarEvent.RePillar_9 -= LightPillarAnimation;

    }

    void LightPillarAnimation()
    {
        lightAnim.SetTrigger("LightPillar");
    }
}
