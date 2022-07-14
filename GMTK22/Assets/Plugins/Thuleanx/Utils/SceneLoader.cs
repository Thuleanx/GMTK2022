using UnityEngine;

namespace Thuleanx.Utils {
	public class SceneLoader : MonoBehaviour {
		[SerializeField] SceneReference Reference;

		public void Transition() {
			Reference.LoadScene();
		}
	}
}