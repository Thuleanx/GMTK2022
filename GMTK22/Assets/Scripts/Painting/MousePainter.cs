using UnityEngine;
using WizOsu.InputSystem;
using TNTC.Painting;

namespace WizOsu.Painting {
	public class MousePainter : MonoBehaviour {
		public Color paintColor;
		public float radius = 1f;
		public float strength = 1f;
		public float hardness = 1f;

		InputManager input => InputManager.instance;

		private void Update() {
			if (input.MouseDown) {
				Ray ray = Camera.main.ScreenPointToRay(input.MousePos);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100f)) {
					Paintable p = hit.collider.GetComponent<Paintable>();
					if (p) PaintManager.instance.paint(p, hit.point, radius, hardness, strength, paintColor);
				} 
			}
		}
	}
}