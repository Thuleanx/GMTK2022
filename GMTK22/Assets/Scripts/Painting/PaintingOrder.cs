using UnityEngine;
using NaughtyAttributes;

namespace WizOsu {
	[CreateAssetMenu(fileName = "PaintingOrder", menuName = "GMTK22/PaintingOrder", order = 0)]
	public class PaintingOrder : ScriptableObject {
		[Required] public Texture2D palette;
		[Required] public Texture2D referenceImage;
		[Required] public Texture2D lineart;
		[Required] public Sprite npcSprite;
		public string requestNode;
		public string evalNode;
		[Range(0, 1f)] public float successThreshold = .3f;
		[Range(0, 300f)] public float durationSeconds = 60f;
	}
}