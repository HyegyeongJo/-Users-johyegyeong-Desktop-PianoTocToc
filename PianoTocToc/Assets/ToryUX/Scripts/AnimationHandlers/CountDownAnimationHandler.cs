using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    public class CountDownAnimationHandler : MonoBehaviour
    {
        public Text countText;

        private bool firstCount = true;

        void Awake()
        {
            firstCount = true;
        }

        public void CountDownStarted()
        {
            ResultUI.TriggerCountDownStartEvent();
        }

        public void ChangeText(string count)
        {
            countText.text = count;
            if (firstCount)
            {
                firstCount = false;
                return;
            }
            if (ResultUI.Instance.countDownSfx != null)
            {
                UISound.Play(ResultUI.Instance.countDownSfx);
            }
        }

        public void CountDownFinished()
        {
            ResultUI.TriggerCountDownFinishEvent();
            ResultUI.FadeOut();
        }
    }
}