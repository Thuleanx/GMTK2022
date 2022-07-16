using UnityEngine;
using NaughtyAttributes;

namespace WizOsu {
	public class ImageJudgingTest : MonoBehaviour {
		[SerializeField] Texture2D tex1, tex2;

		[Button]
		public void Judge() {
			Debug.Log(
				"Tests: \n" +
				"\t SumSquareDiff: \t\t" + TextureDiffUtils.SumSquareDifference(tex1, tex2) + " " + TextureDiffUtils.SumSquareDifferenceExact(tex1, tex2) + "\n" +
				"\t CrossCorrelationDiff \t" + TextureDiffUtils.CrossCorrelationDifference(tex1, tex2) + " " + TextureDiffUtils.CrossCorrelationDifferenceExact(tex1, tex2)
			);
		}
	}
}