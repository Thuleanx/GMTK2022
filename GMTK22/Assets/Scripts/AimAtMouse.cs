using UnityEngine;
using WizOsu.InputSystem;

namespace WizOsu.Behaviour {
	public class AimAtMouse : MonoBehaviour {
		InputManager input => InputManager.instance;

		private void Update() {
			Vector3 pos = input.MouseWorldPosition();
			transform.LookAt(pos, Vector3.up);
		}
	}
}