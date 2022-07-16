using UnityEngine;
using UnityEngine.InputSystem;
using WizOsu.Patterns;

namespace WizOsu.InputSystem {
	public class InputManager : Singleton<InputManager> {
		[SerializeField] float InputBufferTime = .2f; // This is reasonable number

		Camera cam => Camera.main;

		public bool MouseDown { get; private set;  }
	 	public Vector2 MousePos { get; private set;  }
		public Vector2 Movement { get; private set; }

		/// <summary>
		/// Detect the world position that the mouse is pointing at
		/// </summary>
		public Vector3 MouseWorldPosition(float zPos = 0f) {
			Vector3 pos = (Vector3) MousePos + Vector3.forward 
				* (zPos - cam.transform.position.z);
			return cam.ScreenToWorldPoint(pos);
		}

		public override void Awake() {
		}

		public void OnMouseDown(InputAction.CallbackContext ctx) {
			if (ctx.started) MouseDown = true;
			else if (ctx.canceled) MouseDown = false;
		}

		public void OnMousePos(InputAction.CallbackContext ctx) => MousePos = ctx.ReadValue<Vector2>();
		public void OnMovement(InputAction.CallbackContext ctx) => Movement = ctx.ReadValue<Vector2>();
	}
}