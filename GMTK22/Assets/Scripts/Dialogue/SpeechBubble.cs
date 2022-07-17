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

		private void OnDisable() => attachedGameObject = null;
	}
}