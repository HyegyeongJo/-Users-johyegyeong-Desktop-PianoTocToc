using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToryUX
{
    [RequireComponent(typeof(Animator))]
    public class EchoComboUIWrapperAnimationPlayer : MonoBehaviour, IComboUIWrapperAnimationPlayer
    {
        public bool ShouldShowWithScoreUI
        {
            get
            {
                return true;
            }
        }

        public void PlayActivateAnimation()
        {
            GetComponent<Animator>().ResetTrigger("BreakCombo");
            GetComponent<Animator>().ResetTrigger("HideCombo");
            GetComponent<Animator>().SetTrigger("ShowCombo");
        }

        public void PlayDeactivateAnimation()
        {
            GetComponent<Animator>().ResetTrigger("BreakCombo");
            GetComponent<Animator>().ResetTrigger("ShowCombo");
            GetComponent<Animator>().SetTrigger("HideCombo");
        }

        public void PlayFailAnimationAlone()
        {
            GetComponent<Animator>().Play("Fail", 0, 0);
        }

        public void PlayFailAnimationAndHide(bool shouldDeactivateAfterward = true)
        {
            GetComponent<Animator>().ResetTrigger("ShowCombo");
            GetComponent<Animator>().ResetTrigger("HideCombo");
            GetComponent<Animator>().SetTrigger("BreakCombo");
        }
    }
}