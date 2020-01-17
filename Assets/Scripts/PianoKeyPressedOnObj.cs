using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKeyPressedOnObj : MonoBehaviour
{
    public GameObject[] musicalScaleCube;
    public GameObject[] flowerCollider;

    void Update()
    {
            if (Input.GetKeyDown(KeyCode.A))
            {
            musicalScaleCube[0].SetActive(true);
            flowerCollider[0].SetActive(true);

            StartCoroutine(WaitFalseObj_0());
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                musicalScaleCube[1].SetActive(true);
            flowerCollider[1].SetActive(true);

                StartCoroutine(WaitFalseObj_1());
}
        if (Input.GetKeyDown(KeyCode.C))
            {
                musicalScaleCube[2].SetActive(true);
            flowerCollider[2].SetActive(true);

            StartCoroutine(WaitFalseObj_2());
}
        if (Input.GetKeyDown(KeyCode.D))
            {
            musicalScaleCube[3].SetActive(true);
            flowerCollider[3].SetActive(true);

            StartCoroutine(WaitFalseObj_3());
}
        if (Input.GetKeyDown(KeyCode.E))
            {
                musicalScaleCube[4].SetActive(true);
            flowerCollider[4].SetActive(true);

            StartCoroutine(WaitFalseObj_4());
}
        if (Input.GetKeyDown(KeyCode.F))
            {
                musicalScaleCube[5].SetActive(true);
            flowerCollider[5].SetActive(true);

            StartCoroutine(WaitFalseObj_5());
}
        if (Input.GetKeyDown(KeyCode.G))
            {
                musicalScaleCube[6].SetActive(true);
            flowerCollider[6].SetActive(true);

            StartCoroutine(WaitFalseObj_6());
}
        if (Input.GetKeyDown(KeyCode.H))
            {
                musicalScaleCube[7].SetActive(true);
            flowerCollider[7].SetActive(true);

            StartCoroutine(WaitFalseObj_7());

        }
        if (Input.GetKeyDown(KeyCode.I))
            {
                musicalScaleCube[8].SetActive(true);
            flowerCollider[8].SetActive(true);

            StartCoroutine(WaitFalseObj_8());
}
        if (Input.GetKeyDown(KeyCode.J))
            {
                musicalScaleCube[9].SetActive(true);
            flowerCollider[9].SetActive(true);

            StartCoroutine(WaitFalseObj_9());
}
        if (Input.GetKeyDown(KeyCode.K))
            {
                musicalScaleCube[10].SetActive(true);
            flowerCollider[10].SetActive(true);

            StartCoroutine(WaitFalseObj_10());
}
        if (Input.GetKeyDown(KeyCode.L))
            {
                musicalScaleCube[11].SetActive(true);
            flowerCollider[11].SetActive(true);

            StartCoroutine(WaitFalseObj_11());
}
        if (Input.GetKeyDown(KeyCode.M))
            {
                musicalScaleCube[12].SetActive(true);
            flowerCollider[12].SetActive(true);

            StartCoroutine(WaitFalseObj_12());

        }
        if (Input.GetKeyDown(KeyCode.N))
            {
                musicalScaleCube[13].SetActive(true);
            flowerCollider[13].SetActive(true);

            StartCoroutine(WaitFalseObj_13());
}
        if (Input.GetKeyDown(KeyCode.O))
            {
                musicalScaleCube[14].SetActive(true);
            flowerCollider[14].SetActive(true);

            StartCoroutine(WaitFalseObj_14());
}
       






        if (Input.GetKeyUp(KeyCode.A))
        {
            musicalScaleCube[0].SetActive(false);
            flowerCollider[0].SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            musicalScaleCube[1].SetActive(false);
            flowerCollider[1].SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            musicalScaleCube[2].SetActive(false);
            flowerCollider[2].SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            musicalScaleCube[3].SetActive(false);
            flowerCollider[3].SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            musicalScaleCube[4].SetActive(false);
            flowerCollider[4].SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            musicalScaleCube[5].SetActive(false);
            flowerCollider[5].SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            musicalScaleCube[6].SetActive(false);
            flowerCollider[6].SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            musicalScaleCube[7].SetActive(false);
            musicalScaleCube[7].SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            musicalScaleCube[8].SetActive(false);
            flowerCollider[8].SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            musicalScaleCube[9].SetActive(false);
            flowerCollider[9].SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            musicalScaleCube[10].SetActive(false);
            flowerCollider[10].SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            musicalScaleCube[11].SetActive(false);
            flowerCollider[11].SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            musicalScaleCube[12].SetActive(false);
            flowerCollider[12].SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.N))
        {
            musicalScaleCube[13].SetActive(false);
            flowerCollider[13].SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            musicalScaleCube[14].SetActive(false);
            flowerCollider[14].SetActive(false);
        }
    }



    IEnumerator WaitFalseObj_0()
    {
        yield return new WaitForSeconds(0.1f);
        if(musicalScaleCube[0].activeSelf == true)
        musicalScaleCube[0].SetActive(false);
        flowerCollider[0].SetActive(false);
    }
    IEnumerator WaitFalseObj_1()
    {
        yield return new WaitForSeconds(0.1f);
        if (musicalScaleCube[1].activeSelf == true)
            musicalScaleCube[1].SetActive(false);
        flowerCollider[1].SetActive(false);
    }
    IEnumerator WaitFalseObj_2()
    {
        yield return new WaitForSeconds(0.1f);
        if (musicalScaleCube[2].activeSelf == true)
            musicalScaleCube[2].SetActive(false);
        flowerCollider[2].SetActive(false);
    }
    IEnumerator WaitFalseObj_3()
    {
        yield return new WaitForSeconds(0.1f);
        if (musicalScaleCube[3].activeSelf == true)
            musicalScaleCube[3].SetActive(false);
        flowerCollider[3].SetActive(false);
    }
    IEnumerator WaitFalseObj_4()
    {
        yield return new WaitForSeconds(0.1f);
        if (musicalScaleCube[4].activeSelf == true)
            musicalScaleCube[4].SetActive(false);
        flowerCollider[4].SetActive(false);
    }
    IEnumerator WaitFalseObj_5()
    {
        yield return new WaitForSeconds(0.1f);
        if (musicalScaleCube[5].activeSelf == true)
            musicalScaleCube[5].SetActive(false);
        flowerCollider[5].SetActive(false);
    }
    IEnumerator WaitFalseObj_6()
    {
        yield return new WaitForSeconds(0.1f);
        if (musicalScaleCube[6].activeSelf == true)
            musicalScaleCube[6].SetActive(false);
        flowerCollider[6].SetActive(false);
    }
    IEnumerator WaitFalseObj_7()
    {
        yield return new WaitForSeconds(0.1f);
        if (musicalScaleCube[7].activeSelf == true)
            musicalScaleCube[7].SetActive(false);
        flowerCollider[7].SetActive(false);
    }
    IEnumerator WaitFalseObj_8()
    {
        yield return new WaitForSeconds(0.1f);
        if (musicalScaleCube[8].activeSelf == true)
            musicalScaleCube[8].SetActive(false);
        flowerCollider[8].SetActive(false);
    }
    IEnumerator WaitFalseObj_9()
    {
        yield return new WaitForSeconds(0.1f);
        if (musicalScaleCube[9].activeSelf == true)
            musicalScaleCube[9].SetActive(false);
        flowerCollider[9].SetActive(false);
    }
    IEnumerator WaitFalseObj_10()
    {
        yield return new WaitForSeconds(0.1f);
        if (musicalScaleCube[10].activeSelf == true)
            musicalScaleCube[10].SetActive(false);
        flowerCollider[10].SetActive(false);
    }
    IEnumerator WaitFalseObj_11()
    {
        yield return new WaitForSeconds(0.1f);
        if (musicalScaleCube[11].activeSelf == true)
            musicalScaleCube[11].SetActive(false);
        flowerCollider[11].SetActive(false);
    }
    IEnumerator WaitFalseObj_12()
    {
        yield return new WaitForSeconds(0.1f);
        if (musicalScaleCube[12].activeSelf == true)
            musicalScaleCube[12].SetActive(false);
        flowerCollider[12].SetActive(false);
    }
    IEnumerator WaitFalseObj_13()
    {
        yield return new WaitForSeconds(0.1f);
        if (musicalScaleCube[13].activeSelf == true)
            musicalScaleCube[13].SetActive(false);
        flowerCollider[13].SetActive(false);
    }
    IEnumerator WaitFalseObj_14()
    {
        yield return new WaitForSeconds(0.1f);
        if (musicalScaleCube[14].activeSelf == true)
            musicalScaleCube[14].SetActive(false);
        flowerCollider[14].SetActive(false);

    }


}
