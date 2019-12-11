using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    public class CountThreeAnimationHandler : MonoBehaviour
    {
        public Text countText;

        public void Hana()
        {
            countText.text = "1";
            UISound.Play(CountThree.Instance.countOneSfx);
        }

        public void Dul()
        {
            countText.text = "2";
            UISound.Play(CountThree.Instance.countTwoSfx);
        }

        public void Set()
        {
            countText.text = "3";
            UISound.Play(CountThree.Instance.countThreeSfx);
        }

        public void Yap()
        {
            CountThree.Finish();
        }
    }
}