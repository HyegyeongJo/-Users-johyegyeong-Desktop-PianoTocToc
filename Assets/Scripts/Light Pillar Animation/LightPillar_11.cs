using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar_11 : MonoBehaviour
{
    public Animator lightAnim;

    private void OnEnable()
    {
        LightPillarEvent.FaPillar_11 += LightPillarAnimation;
    }

    private void OnDisable()
    {
        LightPillarEvent.FaPillar_11 -= LightPillarAnimation;

    }

    void LightPillarAnimation()
    {
        lightAnim.SetTrigger("LightPillar");
    }
}
