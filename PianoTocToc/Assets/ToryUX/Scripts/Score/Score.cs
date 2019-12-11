using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    /// <summary>
    /// A static class to handle game's score points and combo counts.
    /// It will work together with one <c>ScoreUI</c> component in the scene.
    /// </summary>
    public static class Score
    {
        public static ScoreUI scoreObject;

        /// <summary>
        /// Occurs when the score point hits maximum value.
        /// </summary>
        public static event HitMaxScoreAction OnHitMaxScore;
        public delegate void HitMaxScoreAction();

        /// <summary>
        /// Occurs when score point hits values provided via <c>ScoreEventPoints</c>.
        /// The event will only be fired on score increment.
        /// </summary>
        public static event HitScorePointAction OnHitScorePoint;
        public delegate void HitScorePointAction(int point);

        /// <summary>
        /// <c>OnHitScorePoint</c> event will be fired when the score point hits each value in this array.
        /// </summary>
        public static int[] ScoreEventPoints
        {
            get;
            set;
        }

        /// <summary>
        /// Occurs when combo count hits values provided via <c>ComboEventPoints</c>.
        /// </summary>
        public static event HitComboPointAction OnHitComboPoint;
        public delegate void HitComboPointAction(int point);

        /// <summary>
        /// <c>OnHitComboPoint</c> event will be fired when the combo count hits each values in this array.
        /// </summary>
        public static int[] ComboEventPoints
        {
            get;
            set;
        }

        /// <summary>
        /// Make <c>OnHitMaxScore</c>, <c>OnHitScorePoint</c> and <c>OnHitComboPoint</c> to null.
        /// </summary>
        public static void ResetEvents()
        {
            OnHitMaxScore = null;
            OnHitScorePoint = null;
            OnHitComboPoint = null;
        }

        /// <summary>
        /// How many points should increase/decrease on each animation interval?
        /// Default value is 1.
        /// </summary>
        public static int AnimationStep
        {
            get
            {
                return animationStep;
            }
            set
            {
                animationStep = value;
            }
        }
        private static int animationStep = 1;

        /// <summary>
        /// How many frames should the animation take for each point increase/decrease animation?
        /// Default value is 3.
        /// </summary>
        /// <value>The animation interval.</value>
        public static int AnimationInterval
        {
            get
            {
                return animationInterval;
            }
            set
            {
                animationInterval = value;
            }
        }
        private static int animationInterval = 3;

        /// <summary>
        /// Current point. Can only be read.
        /// Use <c>Gain()</c>, <c>Lose()</c> or <c>Set()</c> to modify the value.
        /// </summary>
        public static int CurrentScorePoint
        {
            get
            {
                return currentScorePoint;
            }
            private set
            {
                // Check for events to trigger before updating the value.
                if (OnHitScorePoint != null && ScoreEventPoints != null && value > currentScorePoint)
                {
                    for (int i = 0; i < ScoreEventPoints.Length; i++)
                    {
                        if (ScoreEventPoints[i] > currentScorePoint && ScoreEventPoints[i] <= value)
                        {
                            OnHitScorePoint(ScoreEventPoints[i]);
                            break;
                        }
                    }
                }

                currentScorePoint = value;
                if (scoreObject != null && IsUIShown)
                {
                    scoreObject.AnimateScorePoints();
                }

                // Check for event to trigger.
                if (currentScorePoint == MaximumScorePoint && OnHitMaxScore != null)
                {
                    OnHitMaxScore();
                }
            }
        }
        private static int currentScorePoint;

        /// <summary>
        /// Maximum number of points which current point could possibly go up.
        /// Default value is the maximum number of integer. (2147483647)
        /// </summary>
        public static int MaximumScorePoint
        {
            get
            {
                return maximumScorePoint;
            }
            set
            {
                maximumScorePoint = value;
            }
        }
        private static int maximumScorePoint = int.MaxValue;

        /// <summary>
        /// Minimum number of points which current point could possibly go down.
        /// Current point will always be equal or greater than this value.
        /// </summary>
        public static int MinimumScorePoint
        {
            get;
            set;
        }

        /// <summary>
        /// Combo UI will show up when combo point hits this value.
        /// Default value is 5.
        /// Assign <c>int.MaxValue</c> to prevent combo UI from ever showing up.
        /// </summary>
        public static int MinimumComboCount
        {
            get
            {
                return minimumComboCount;
            }
            set
            {
                minimumComboCount = value;
            }
        }
        private static int minimumComboCount = 5;

        /// <summary>
        /// Current combo point. Can only be read.
        /// Combo point goes up in tandem with score <c>Gain()</c> call.
        /// Combo breaks when score <c>Lose()</c> is called or <c>BreakCombo()</c> is explicitly called.
        /// Be aware that unlike score point, combo count will only get updated during <c>IsUIShown</c> is <value>>true</value>.
        /// </summary>
        public static int CurrentComboPoint
        {
            get
            {
                return currentComboPoint;
            }
            private set
            {
                if (value != 0 || currentComboPoint != 0)
                {
//                    currentComboPoint = value;
                    if (scoreObject != null && IsUIShown)
                    {
						if (currentComboPoint >= MinimumComboCount && value <= 0)
						{
							scoreObject.BreakCombo();
						}
						currentComboPoint = value;
						if (currentComboPoint >= MinimumComboCount)
						{
							if (CurrentComboPoint == MinimumComboCount)
							{
								scoreObject.ShowCombo();
							}
							scoreObject.AnimateComboPoints();
						}
//                        if (currentComboPoint <= 0)
//                        {
//                            scoreObject.BreakCombo();
//                        }
//                        else if (currentComboPoint >= MinimumComboCount)
//                        {
//                            if (currentComboPoint == MinimumComboCount)
//                            {
//                                scoreObject.ShowCombo();
//                            }
//                            scoreObject.AnimateComboPoints();
//                        }
                    }
					else
					{
						currentComboPoint = value;
					}

                    // Check for events to trigger.
                    if (ComboEventPoints != null)
                    {
                        for (int i = 0; i < ComboEventPoints.Length; i++)
                        {
                            if (currentComboPoint == ComboEventPoints[i] && OnHitComboPoint != null)
                            {
                                OnHitComboPoint(currentComboPoint);
                                break;
                            }
                        }
                    }
                }
            }
        }
        private static int currentComboPoint;

        /// <summary>
        /// Record score. Can only be read.
        /// <c>ResultUI</c> component will automatically update this value.
        /// Call <c>ResetHighScore(int)</c> to manually set the value.
        /// </summary>
        public static int HighScore
        {
            get
            {
                return PlayerPrefs.GetInt("HighScore", 0);
            }
        }

        private static bool Evaluate(int point)
        {
            // Minimum point is first checked; current point will never go below minimum point even when min/max points are not right (i.e., min=10, max=5)
            if (point < MinimumScorePoint)
            {
                CurrentScorePoint = MinimumScorePoint;
                return false;
            }
            else if (point > MaximumScorePoint)
            {
                CurrentScorePoint = MaximumScorePoint;
                return false;
            }
            else
            {
                CurrentScorePoint = point;
                return true;
            }
        }

        /// <summary>
        /// Sets current score manually.
        /// Returns <value>false</value> when set value does not fit between <c>MinimumPoint</c> and <c>MaximumPoint</c>.
        /// </summary>
        /// <param name="point">Score point to set.</param>
        public static bool Set(int point)
        {
            return Evaluate(point);
        }

        /// <summary>
        /// Increases current score by certain amount.
        /// Returns <value>false</value> when set value does not fit between <c>MinimumPoint</c> and <c>MaximumPoint</c>.
        /// Combo count goes up automatically.
        /// </summary>
        /// <param name="point">Number of points to add up.</param>
        public static bool Gain(int point = 1)
        {
            if (IsUIShown)
            {
                CurrentComboPoint += 1;
            }
            return Evaluate(CurrentScorePoint + point);
        }

        /// <summary>
        /// Decreases current score by certain amount.
        /// Returns <value>false</value> when set value does not fit between <c>MinimumPoint</c> and <c>MaximumPoint</c>.
        /// Combo count resets automatically unless explicitly set not to.
        /// </summary>
        /// <param name="point">Number of points to take away.</param>
		/// <param name="breakCombo">Whether to break combo on point lose.</param>
		public static bool Lose(int point = 1, bool breakCombo = true)
        {
			if (breakCombo)
			{
	            BreakCombo();
			}
            return Evaluate(CurrentScorePoint - point);
        }

        /// <summary>
        /// Resets current score to be <c>MinimumPoint</c>.
        /// Animations and effects will not be triggered when using this method.
        /// </summary>
        public static void Reset()
        {
            currentScorePoint = MinimumScorePoint;
			currentComboPoint = 0;
            if (scoreObject != null)
            {
                scoreObject.UpdateScorePoints();
            }
        }

        /// <summary>
        /// Resets combo point and play combo failure animations and effects.
        /// Note that combo resets automatically on <c>Lose()</c> call; no need to call this function if the game uses score losing system.
        /// </summary>
        public static void BreakCombo()
        {
            CurrentComboPoint = 0;
        }

        /// <summary>
        /// Resets high score.
        /// Pass integer value to manually modify the record.
        /// This method will automatically called by <c>ResultUI</c> component.
        /// </summary>
        public static void ResetHighScore(int highScore = 0)
        {
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        /// <summary>
        /// Makes score UI appear.
        /// </summary>
        public static void ShowUI()
        {
            scoreObject.gameObject.SetActive(true);
            scoreObject.Show();
        }

        /// <summary>
        /// Makes score UI disappear.
        /// </summary>
        public static void HideUI()
        {
            scoreObject.Hide();
        }

        /// <summary>
        /// Returns true if score UI game object is active.
        /// </summary>
        public static bool IsUIShown
        {
            get
            {
                if (scoreObject != null)
                {
                    return scoreObject.IsShown;
                }
                else
                {
                    #if UNITY_EDITOR || DEVELOPMENT_BUILD
                    Debug.LogWarning("No SocreUI is registered to Score static class!");
                    #endif
                    return false;
                }
            }
        }
    }
}