using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToryUX
{
    public interface IComboUIWrapperAnimationPlayer
    {
        bool ShouldShowWithScoreUI
        {
            get;
        }

        void PlayActivateAnimation();
        void PlayDeactivateAnimation();
        void PlayFailAnimationAlone();
        void PlayFailAnimationAndHide(bool shouldDeactivateAfterward = true);
    }
}