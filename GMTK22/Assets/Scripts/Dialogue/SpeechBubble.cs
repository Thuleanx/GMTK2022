using UnityEngine;
using NaughtyAttributes;
using TMPro;
using WizOsu.Dialogue;

namespace WizOsu.UI {
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class SpeechBubble : MonoBehaviour {
		[SerializeField, ReadOnly] Speaker attachedSpeaker = null;
		[SerializeField, Required] public TextMeshProUGUI textObj;
		[SerializeField] Vector2 padding;
		RectTransform rectTransform;
		Canvas canvas;

		void Awake() {
			rectTransform = GetComponent<RectTransform>();
			canvas = GetComponentInParent<Canvas>();
		}

		public void Setup(Speaker speaker, string text) {
			attachedSpeaker = speaker;
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
			if (attachedSpeaker) {
				Plane plane = new Plane(Vector3.forward, canvas.transform.position);
				Vector3 screenPoint = Camera.main.WorldToScreenPoint(attachedSpeaker.transform.position + (Vector3) attachedSpeaker.offset);
				Ray ray = Camera.main.ScreenPointToRay(screenPoint);
				float dist = 0;
				plane.Raycast(ray, out dist);
				Vector3 corPos = ray.GetPoint(dist);
				transform.position = corPos;
			}
		}

		private void OnDisable() => attachedSpeaker = null;
	}
}