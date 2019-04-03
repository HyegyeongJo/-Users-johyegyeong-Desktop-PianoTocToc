using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToryUX;

public class RotateCamera : MonoBehaviour {

    IEnumerator Start()
    {
        yield return null;
        FlipCamera(UIOrientationSetter.IsFlipped.Value);
    }

    void OnEnable()
    {
        UIOrientationSetter.IsFlipped.ValueChanged += FlipCamera;
    }

    void OnDisable()
    {
        UIOrientationSetter.IsFlipped.ValueChanged -= FlipCamera;
    }

    public void FlipCamera(bool flipped)
    {
        if (flipped)
        {
            Debug.Log("is flipped");
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            Debug.Log("is NOT flipped");
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);  
        }
    }
}