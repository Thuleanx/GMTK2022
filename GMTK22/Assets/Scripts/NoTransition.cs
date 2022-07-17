using UnityEngine;

namespace WizOsu.UI {
	public class NoTransition : MonoBehaviour {
		void Start() {
			TransitionManager.instance.FadeInImmediate();
		}
	}
}