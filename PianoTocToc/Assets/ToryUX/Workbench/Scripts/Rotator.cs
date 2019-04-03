using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    public bool randomStartRotation = true;
    public float rotationSpeed = 25f;

    void Start()
    {
        if (randomStartRotation)
        {
            transform.rotation = Random.rotation;
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.one * rotationSpeed * Time.deltaTime);
    }
}