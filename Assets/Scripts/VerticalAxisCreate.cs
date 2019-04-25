using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalAxisCreate : MonoBehaviour
{
    public Camera verticalAxisSpaceCam;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(verticalAxisSpaceCam.pixelWidth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
