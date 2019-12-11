using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectNote : MonoBehaviour
{
    public Transform noteCorrectParticle;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "NoteCollider")
        {
            Debug.Log("Note Collider!!!!!");

            Instantiate(noteCorrectParticle, col.transform.position, Quaternion.identity);
        }
    }
}
