using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToryUX
{
    public class BestScoreCelebrationAnimationHandler : MonoBehaviour
    {
        public void ShowingUp()
        {
            if (ResultUI.Instance.bestScoreSfx != null)
            {
                UISound.Play(ResultUI.Instance.bestScoreSfx);
            }
            if (ResultUI.Instance.bestScoreVfx != null)
            {
                ResultUI.Instance.bestScoreVfx.Play();
            }
        }
    }
}