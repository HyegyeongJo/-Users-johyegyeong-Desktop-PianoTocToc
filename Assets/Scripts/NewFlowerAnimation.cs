using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFlowerAnimation : MonoBehaviour
{
    public Animator newFlower;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        newFlower.SetTrigger("FlowerTrigger");
    }

}
