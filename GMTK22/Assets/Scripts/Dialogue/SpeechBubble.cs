using UnityEngine;
using NaughtyAttributes;
using TMPro;

namespace WizOsu.UI {
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class SpeechBubble : MonoBehaviour {
		[SerializeField, ReadOnly] GameObject attachedGameObject = null;
		[SerializeField, Required] public TextMeshProUGUI textObj;
		[SerializeField] Vector2 padding;
		[SerializeField] Vector2 offset;
		RectTransform rectTransform;
		Canvas canvas;

		void Awake() {
			rectTransform = GetComponent<RectTransform>();
			canvas = GetComponentInParent<Canvas>();
		}

		public void Setup(GameObject speaker, string text) {
			attachedGameObject = speaker;
			textObj.SetText(text);
			textObj.ForceMeshUpdate();
			ResizeToTextContent();
		}

		public void ResizeToTextContent() {
			Vector2 textSize = textObj.GetRenderedValues();
			rectTransform.sizeDelta = textSize + padding * 2;
		}

		void LateUpdate() {
			Reposition();
		}

		public void Reposition() {
			if (attachedGameObject) {
				Plane plane = new Plane(Vector3.forward, canvas.transform.position);
				Vector3 screenPoint = Camera.main.WorldToScreenPoint(attachedGameObject.transform.position);
				Ray ray = Camera.main.ScreenPointToRay(screenPoint);
				float dist = 0;
				plane.Raycast(ray, out dist);
				Vector3 corPos = ray.GetPoint(dist);
				transform.position = corPos + (Vector3) offset;
			}
		}

		private void OnDisable() => attachedGameObject = null;
	}
}