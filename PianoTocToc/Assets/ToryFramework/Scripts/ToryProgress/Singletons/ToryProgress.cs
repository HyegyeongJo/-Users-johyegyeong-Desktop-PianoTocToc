using UnityEngine;
using ToryFramework.Behaviour;
using ManUtils;

namespace ToryFramework
{
	/// <summary>
	/// Tory class that handles progress points of the game.
	/// </summary>
	public class ToryProgress
	{
		#region SINGLETON

		static volatile ToryProgress instance;
		static readonly object syncRoot = new object();

		ToryProgress() { }

		/// <summary>
		/// Gets the instance of the class that handles progress points of the game.
		/// </summary>
		/// <value>The instance.</value>
		public static ToryProgress Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
						{
							instance = new ToryProgress();
						}
					}
				}
				return instance;
			}
		}

		#endregion


		#region FIELDS

		float minPoint = 0f, maxPoint = 100f, currentPoint = 0f;

		#endregion



		#region PROPERTIES

		// Behaviours

		ToryFrameworkBehaviour FrameworkBehaviour 		{ get { return ToryFrameworkBehaviour.Instance; }}

		/// <summary>
		/// Gets or sets the minimum point whcih <see cref="P:CurrentPoint"/> could possibly go down. 
		/// The default is 0.
		/// </summary>
		/// <value>The minimum point.</value>
		public float MinPoint 			{ get { return minPoint; } set { minPoint = value; }}

		/// <summary>
		/// Gets or sets the maximum point whcih <see cref="P:CurrentPoint"/> could possibly go up.
		/// the default is 100.
		/// </summary>
		/// <value>The maximum point.</value>
		public float MaxPoint 			{ get { return maxPoint; } set { maxPoint = value; }}

		/// <summary>
		/// Gets the current point of the progress (Read Only).
		/// Cannot be smaller than <see cref="P:MinPoint"/> or greater than <see cref="P:MaxPoint"/>.
		/// The default is 0.
		/// </summary>
		/// <value>The current point.</value>
		public float CurrentPoint 		{ 	get { return currentPoint; }
			private set
			{
				float v = Mathf.Clamp(value, MinPoint, MaxPoint);

				// Check event point.
				if (EventPoints != null && v > CurrentPoint)
				{
					for (int i = 0; i < EventPoints.Length; i++)
					{
						if (EventPoints[i] > CurrentPoint && EventPoints[i] <= v)
						{
							EventPointHit(currentPoint = v);
							break;
						}
					}
				}

				currentPoint = v;

				// Check min/max point.
				if (currentPoint <= MinPoint)
				{
					MinPointHit();
				}
				else if (currentPoint >= MaxPoint)
				{
					MaxPointHit();
				}
			}
		}

		/// <summary>
		/// Gets the normalized point of the progress between 0 and 1 (Read Only).
		/// </summary>
		/// <value>The normalized point.</value>
		public float NormPoint 	{ get { return ManMath.Map(CurrentPoint, MinPoint, MaxPoint, 0f, 1f); }}

		/// <summary>
		/// Gets or sets the event points of the progress.
		/// If the <see cref="P:CurrentPoint"/> forwards and hits each value in this array, the <see cref="E:EventPointHit"/> event will be fired.
		/// Each element of this array cannot be smaller than <see cref="P:MinPoint"/> or greater than <see cref="P:MaxPoint"/>.
		/// </summary>
		/// <value>The event points.</value>
		public float[] EventPoints 		{ get; set; }

		#endregion



		#region EVENTS

		public delegate void HitEventHandler();

		public delegate void PointHitEventHandler(float point);

		/// <summary>
		/// Occurs when the <see cref="P:CurrentPoint"/> hits the <see cref="P:MinPoint"/>.
		/// </summary>
		public event HitEventHandler MinPointHit = () => {};

		/// <summary>
		/// Occurs when the <see cref="P:CurrentPoint"/> hits the <see cref="P:MaxPoint"/>.
		/// </summary>
		public event HitEventHandler MaxPointHit = () => {};

		/// <summary>
		/// Occurs when the <see cref="P:CurrentPoint"/> forwards and hits each value in the <see cref="P:EventPoints"/> array.
		/// </summary>
		public event PointHitEventHandler EventPointHit = (float point) => {};

		#endregion



		#region METHODS

		void ResetEvents()
		{
			MinPointHit = () => {};
			MaxPointHit = () => {};
			EventPointHit = (float point) => {};
		}

		void Init()
		{
		}

		/// <summary>
		/// Set the <see cref="P:CurrentPoint"/>.
		/// Cannot be smaller than <see cref="P:MinPoint"/> or greater than <see cref="P:MaxPoint"/>.
		/// </summary>
		/// <returns>The set.</returns>
		/// <param name="point">Point.</param>
		public void Set(float point)
		{
			CurrentPoint = point;
		}

		/// <summary>
		/// Increase the <see cref="P:CurrentPoint"/> by certain amount. 
		/// </summary>
		/// <returns>The forward.</returns>
		/// <param name="point">Point.</param>
		public void Forward(float point)
		{
			CurrentPoint += point;
		}

		/// <summary>
		/// Decrease the <see cref="P:CurrentPoint"/> by certain amount.
		/// </summary>
		/// <returns>The backward.</returns>
		/// <param name="point">Point.</param>
		public void Backward(float point)
		{
			CurrentPoint -= point;
		}

		/// <summary>
		/// Reset <see cref="P:CurrentPoint"/> to be <see cref="P:MinPoint"/>.  
		/// </summary>
		public void Reset()
		{
			CurrentPoint = MinPoint;
		}

		#endregion
	}
}