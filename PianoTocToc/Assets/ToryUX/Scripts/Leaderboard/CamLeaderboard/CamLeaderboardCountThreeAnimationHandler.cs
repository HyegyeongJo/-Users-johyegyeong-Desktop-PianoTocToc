using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    public class CamLeaderboardCountThreeAnimationHandler : MonoBehaviour
    {
        public AudioClip countOneSfx;
        public AudioClip countTwoSfx;
        public AudioClip countThreeSfx;

        public Text countText;

        public event CountFinishAction OnCountFinish;
        public delegate void CountFinishAction();

        public void Hana()
        {
            countText.text = "1";
            UISound.Play(countOneSfx);
        }

        public void Dul()
        {
            countText.text = "2";
            UISound.Play(countTwoSfx);
        }

        public void Set()
        {
            countText.text = "3";
            UISound.Play(countThreeSfx);
        }

        public void Yap()
        {
            if (OnCountFinish != null)
            {
                OnCountFinish.Invoke();
            }
        }

        void OnDestroy()
        {
            OnCountFinish = null;
        }
    }
}