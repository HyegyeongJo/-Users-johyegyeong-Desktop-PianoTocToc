using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToryUX;

public class CameraObversion : MonoBehaviour
{
    public GameObject mainCam;
   // bool gloabalIsFlip;

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
           // Debug.Log("is flipped");
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else
        {
          //  Debug.Log("is NOT flipped");
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
    }


    //public void Start(bool isFlip)
    //{
    //    if (isFlip)
    //        mainCam.transform.rotation = Quaternion.Euler(0, 0, 90);
    //    else
    //        mainCam.transform.rotation = Quaternion.Euler(0, 0, -90);
    //}


    //public void Flip(bool isFlip)
    //{
    //    if (isFlip)
    //    {
    //        gloabalIsFlip = true;
    //        mainCam.transform.rotation = Quaternion.Euler(0, 0, 90);
    //    }
    //    else
    //    {
    //        gloabalIsFlip = false;
    //        mainCam.transform.rotation = Quaternion.Euler(0, 0, -90);
    //    }
    //}




}
