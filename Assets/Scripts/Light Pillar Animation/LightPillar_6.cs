using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar_6 : MonoBehaviour
{
    public Animator lightAnim;

    private void OnEnable()
    {
        LightPillarEvent.RaPillar_6 += LightPillarAnimation;
    }

    private void OnDisable()
    {
        LightPillarEvent.RaPillar_6 -= LightPillarAnimation;

    }

    void LightPillarAnimation()
    {
        lightAnim.SetTrigger("LightPillar");
    }
}
