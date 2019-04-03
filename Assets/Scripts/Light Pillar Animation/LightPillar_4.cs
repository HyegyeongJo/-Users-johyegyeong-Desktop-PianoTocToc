using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar_4 : MonoBehaviour
{
    public Animator lightAnim;

    private void OnEnable()
    {
        LightPillarEvent.FaPillar_4 += LightPillarAnimation;
    }

    private void OnDisable()
    {
        LightPillarEvent.FaPillar_4 -= LightPillarAnimation;

    }

    void LightPillarAnimation()
    {
        lightAnim.SetTrigger("LightPillar");
    }
}
