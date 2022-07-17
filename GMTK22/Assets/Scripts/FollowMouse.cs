using UnityEngine;
using WizOsu.InputSystem;

namespace WizOsu.Behaviour {
	public class FollowMouse : MonoBehaviour {
		InputManager input => InputManager.instance;

		private void Update() {
			Vector3 pos = input.MouseWorldPosition();

			transform.position = new Vector3(pos.x, pos.y, -2.43f);
		}
	}
}