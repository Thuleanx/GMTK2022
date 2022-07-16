using UnityEngine;
using UnityEngine.InputSystem;
using WizOsu.Patterns;

namespace WizOsu.InputSystem {
	public class InputManager : Singleton<InputManager> {
		[SerializeField] float InputBufferTime = .2f; // This is reasonable number
		[HideInInspector] public bool MouseDown { get; private set;  }

		public override void Awake() {
		}

		public void OnMouse(InputAction.CallbackContext ctx) {
			if (ctx.started) MouseDown = true;
			else if (ctx.canceled) MouseDown = false;
		}

	}
}