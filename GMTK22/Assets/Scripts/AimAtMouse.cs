using UnityEngine;
using WizOsu.InputSystem;
using TNTC.Painting;
using Thuleanx.Utils;

namespace WizOsu.Behaviour {
	public class AimAtMouse : MonoBehaviour {
		InputManager input => InputManager.instance;
		Optional<Paintable> paintingCanvas;

		private void Update() {
			Vector3 pos = input.MouseWorldPosition(paintingCanvas.Enabled ? paintingCanvas.Value.transform.position.z : 0);
			transform.LookAt(pos, Vector3.up);
		}
	}
}