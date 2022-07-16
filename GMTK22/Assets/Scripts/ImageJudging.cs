using UnityEngine;
using WizOsu.Patterns;

namespace WizOsu {
	[CreateAssetMenu(fileName = "ImageJudging", menuName = "GMTK22/ImageJudging", order = 0)]
	public class ImageJudging : ScriptableObject {
		public static ImageJudging instance;
		

		private void Awake() {
			instance = this;
		}

		public void CompareTextures(Texture2D t1, Texture2D t2, ImageComparisonModes comparisonMode) {
		}

		void Sum

		public enum ImageComparisonModes {
			SUM_SQUARE_DIFFERENCE
		}
	}
}