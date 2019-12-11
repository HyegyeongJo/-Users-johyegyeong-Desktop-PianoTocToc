using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    /// <summary>
    /// A static class to handle game's progress points.
    /// It will work together with one <c>ProgressUI</c> component in the scene.
    /// </summary>
    public static class Progress
    {
        public static ProgressUI progressObject;

        /// <summary>
        /// Occurs when progression hits maximum value.
        /// </summary>
        public static event HitMaxProgressAction OnHitMaxProgress;
        public delegate void HitMaxProgressAction();

        /// <summary>
        /// Occurs when progression hits values provided by <c>ProgressionEventPoints</c>.
        /// The event will only be fired on progression forward.
        /// </summary>
        public static event HitProgressionAction OnHitProgression;
        public delegate void HitProgressionAction(float progression);

        /// <summary>
        /// <c>OnHitProgression</c> event will be fired when the progression hits each value in this array.
        /// Each value should be between 0 and 1.
        /// </summary>
        public static float[] ProgressionEventPoints
        {
            get;
            set;
        }

        /// <summary>
        /// Make <c>OnHitMaxProgress</c> and <c>OnHitProgression</c> to null.
        /// </summary>
        public static void ResetEvents()
        {
            OnHitMaxProgress = null;
            OnHitProgression = null;
        }

        /// <summary>
        /// Current progression. Can only be read.
        /// This always will return a value between 0 and 1;
        /// 0 when <c>CurrentProgressPoint</c> equals to <c>MinimumProgressPoint</c>,
        /// 1 when <c>CurrentProgressPoint</c> equals to <c>MaximumProgressPoint</c>.
        /// </summary>
        public static float CurrentProgression
        {
            get
            {
                return currentProgression;
            }
            private set
            {
                // Check for events to trigger before updating the value.
                if (OnHitProgression != null && ProgressionEventPoints != null && value > currentProgression)
                {
                    for (int i = 0; i < ProgressionEventPoints.Length; i++)
                    {
                        if (ProgressionEventPoints[i] > currentProgression && ProgressionEventPoints[i] <= value)
                        {
                            OnHitProgression(ProgressionEventPoints[i]);
                            break;
                        }
                    }
                }

                currentProgression = value;

                // Check for maximum event.
				if (currentProgression >= 1f && OnHitMaxProgress != null)
                {
                    OnHitMaxProgress();
                }
            }
        }
        private static float currentProgression;

        /// <summary>
        /// Current progress point. Can only be read.
        /// This is raw value of progress point, not a clamped value between 0 and 1.
        /// </summary>
        public static float CurrentProgressPoint
        {
            get
            {
                return currentProgressPoint;
            }
            private set
            {
                currentProgressPoint = value;
                CurrentProgression = (CurrentProgressPoint - MinimumProgressPoint) / (MaximumProgressPoint - MinimumProgressPoint);
            }
        }
        private static float currentProgressPoint;

        /// <summary>
        /// Maximum number of point which <c>CurrentProgressPoint</c> could possibly go up.
        /// Default value is 100.
        /// <c>CurrentProgressPoint</c> will always be equal or smaller than this value.
        /// </summary>
        public static float MaximumProgressPoint
        {
            get
            {
                return maximumProgressPoint;
            }
            set
            {
                maximumProgressPoint = value;
            }
        }
        private static float maximumProgressPoint = 100f;

        /// <summary>
        /// Minimum number of point which <c>CurrentProgressPoint</c> could possibly go down.
        /// Default value is 0.
        /// <c>CurrentProgressPoint</c> will always be equal or greater than this value.
        /// </summary>
        public static float MinimumProgressPoint
        {
            get;
            set;
        }

        private static bool Evaluate(float point)
        {
            if (point < MinimumProgressPoint)
            {
                CurrentProgressPoint = MinimumProgressPoint;
                return false;
            }
            else if (point > MaximumProgressPoint)
            {
                CurrentProgressPoint = MaximumProgressPoint;
                return false;
            }
            else
            {
                CurrentProgressPoint = point;
                return true;
            }
        }

        /// <summary>
        /// Sets <c>CurrentProgressPoint</c> manually.
        /// Returns <value>false</value> when set value does not fit between <c>MinimumProgressPoint</c> and <c>MaximumProgressPoint</c>.
        /// </summary>
        /// <param name="point">Progress point to set.</param>
        public static bool Set(float point)
        {
            return Evaluate(point);
        }

        /// <summary>
        /// Increases <c>CurrentProgressPoint</c> by certain amount.
        /// Returns <value>false</value> when set value does not fit between <c>MinimumProgressPoint</c> and <c>MaximumProgressPoint</c>.
        /// </summary>
        /// <param name="point">Number of progress point to add up.</param>
        public static bool Forward(float point)
        {
            return Evaluate(CurrentProgressPoint + point);
        }

        /// <summary>
        /// Decreases <c>CurrentProgressPoint</c> by certain amount.
        /// Returns <value>false</value> when set value does not fit between <c>MinimumProgressPoint</c> and <c>MaximumProgressPoint</c>.
        /// </summary>
        /// <param name="point">Number of progress point to take away.</param>
        public static bool Backward(float point)
        {
            return Evaluate(CurrentProgressPoint - point);
        }

        /// <summary>
        /// Resets <c>CurrentProgressPoint</c> to be <c>MinimumProgressPoint</c>.
        /// Animations and effects will not be triggered when using this method.
        /// </summary>
        public static void Reset()
        {
            currentProgressPoint = MinimumProgressPoint;
			currentProgression = 0;
        }


		public static void ShowUI()
		{
			progressObject.gameObject.SetActive(true);
			progressObject.Show();
		}

		public static void HideUI()
		{
			progressObject.Hide();
		}

		public static float UIFillSpeed
		{
			get
			{
				return uiFillSpeed;
			}
			set
			{
				uiFillSpeed = Mathf.Clamp(value, 0, 1f);
			}
		}
		private static float uiFillSpeed = 0.5f;

		public static bool IsUIShown
		{
			get
			{
				if (progressObject != null)
				{
					return progressObject.IsShown;
				}
				else
				{
					#if UNITY_EDITOR || DEVELOPMENT_BUILD
					Debug.LogWarning("No ProgressUI is registered to Progress static class!");
					#endif
					return false;
				}
			}
		}
    }
}