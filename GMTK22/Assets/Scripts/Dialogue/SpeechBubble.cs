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

		void Awake() {
			rectTransform = GetComponent<RectTransform>();
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
				// Vector3 posWithCorrectZ = attachedGameObject.transform.position;
				// Vector2 pos = (Vector2) Camera.main.WorldToScreenPoint(posWithCorrectZ) + offset;
				transform.position = attachedGameObject.transform.position + (Vector3) offset;
			}
		}

		private void OnDisable() => attachedGameObject = null;
	}
}