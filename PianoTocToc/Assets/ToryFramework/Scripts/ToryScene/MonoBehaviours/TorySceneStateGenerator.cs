using UnityEngine;

namespace ToryFramework.Behaviour
{
	public class TorySceneStateGenerator : MonoBehaviour
	{
		[SerializeField] string[] states;

		public string[] States  		{ get { return states; } set { states = value; }}
	}
}