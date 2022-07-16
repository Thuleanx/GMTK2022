using UnityEngine;
using NaughtyAttributes;
using WizOsu;
using WizOsu.Painting;

namespace WizOsu.Test {
	[RequireComponent(typeof(MagicCanvas))]
	public class MagicCanvasTest : MonoBehaviour {
		public MagicCanvas MagicCanvas { get; private set; }

		[SerializeField] Texture2D sourceTexture;

		void Awake() {
			MagicCanvas = GetComponent<MagicCanvas>();
		}

		[Button] 
		public void CompareTextures() {
			Texture2D targetTexture = MagicCanvas.GetTexture();

			Debug.Log(
				"SumSquareDiff: " + TextureDiffUtils.SumSquareDifference(sourceTexture, targetTexture) 
			);
		}
	}
}