using UnityEngine;

namespace Thuleanx2D.FX {
	public class Parallax : MonoBehaviour {
		Vector2 _camLastPos;
		Vector3 _originalPosition;

		void OnEnable() {
			_originalPosition = transform.position;
			Vector2 diff = transform.position - Camera.main.transform.position;
			float mult = 
				10f / (transform.position.z - Camera.main.transform.position.z);
			float move = diff.x * mult + Camera.main.transform.position.x - 
				_originalPosition.x; 
			transform.Translate(
				move * Vector2.right
			);
			_camLastPos = Camera.main.transform.position;
		}

		void OnDisable() {
			transform.position = _originalPosition;
		}

		void LateUpdate() {
			Vector2 diff = 
				(Vector2) Camera.main.transform.position - _camLastPos;
			float mult = 
				10f / (transform.position.z - Camera.main.transform.position.z);
			transform.Translate(Vector2.right * diff.x * (1 - mult));
			_camLastPos = Camera.main.transform.position;
		}
	}
}