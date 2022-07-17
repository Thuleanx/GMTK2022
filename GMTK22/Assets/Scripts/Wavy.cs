using UnityEngine;
using Thuleanx.Utils;

/// <summary>
/// Bops up and down
/// </summary>
namespace Thuleanx.UI {
	public class Wavy : MonoBehaviour {
		public float Amount = 2f;
		public float Period = 1.333f;
		public bool Randomized = false;

		float timeStart;
		float offset;
		Vector3 original;
		RectTransform rectTransform;

		private void Awake() {
			rectTransform = GetComponent<RectTransform>();
		}

		private void OnEnable() {
			timeStart = Time.time;
			original = rectTransform == null ? transform.position : rectTransform.anchoredPosition;
			if (Randomized) offset = Mathx.RandomRange(0, 1f);
		}

		void Update() {
			if (rectTransform != null)
				rectTransform.anchoredPosition = original + (Vector3) Wave(Time.time - timeStart);
			else
				transform.position = original + (Vector3) Wave(Time.time - timeStart);
		}

		public Vector2 Wave(float time) {
			return new Vector2(0f, Amount * Mathf.Sin(2 * Mathf.PI * (offset + time / Period)));
		}
	}
}