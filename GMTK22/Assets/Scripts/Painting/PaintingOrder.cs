using UnityEngine;
using NaughtyAttributes;

namespace WizOsu {
	[CreateAssetMenu(fileName = "PaintingOrder", menuName = "GMTK22/PaintingOrder", order = 0)]
	public class PaintingOrder : ScriptableObject {
		[SerializeField, Required] Texture2D palette;
		[SerializeField, Required] Texture2D referenceImage;
		[SerializeField, Required] Texture2D lineart;
		[SerializeField] string interactionNode;
		[SerializeField, Range(0f, 1f)] float successThreshold = .3f;
		[SerializeField, Range(0, 10f)] float durationMins;
		[SerializeField, Range(0, 59f)] float durationSeconds;
	}
}