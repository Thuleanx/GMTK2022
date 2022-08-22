using System;
using System.Collections.Generic;
using UnityEngine;
using WizOsu.Patterns;
using Thuleanx.Utils;

namespace WizOsu {
	public static class TextureDiffUtils {
		/// <summary>
		/// ! Be careful. This operation is expensive
		/// </summary>
		public static float SumSquareDifference(Texture2D t1, Texture2D t2, int numRow = 512, int numCol = 512) {
			if (numRow < 2 || numCol < 2) {
				Debug.LogWarning("Can't compare images with at most 1 columns or rows using this sampling method");
				return 10000f;
			}
			float S_sq = 0;
			float normalizer = 0;
			for (int i = 0; i < numRow; i++) for (int j = 0; j < numCol; j++) {
				float pu = (float)i / (numRow - 1);
				float pv = (float)i / (numCol- 1);
				S_sq += CorrectPixel(t1.GetPixelBilinear(pu, pv), t2.GetPixelBilinear(pu, pv), ref normalizer);
			}
			return normalizer > 0 ? S_sq / normalizer : 0f;
		}

		/// <summary>
		/// ! Be careful. This operation is expensive. The two given textures must have the same dimensions
		/// </summary>
		public static float SumSquareDifferenceExact(Texture2D t1, Texture2D t2) {
			Color[] colT1 = t1.GetPixels();
			Color[] colT2 = t2.GetPixels();
			if (colT1.Length != colT2.Length) Debug.LogWarning("Trying to compare two textures with different dimensions");
			float S_sq = 0;
			float normalizer = 0;
			for (int i = 0; i < Mathf.Min(colT1.Length, colT2.Length); i++)
				S_sq += CorrectPixel(colT1[i], colT2[i], ref normalizer);
			return normalizer > 0 ? S_sq / normalizer : 0f;
		}

		public static float CrossCorrelationDifferenceExact(Texture2D t1, Texture2D t2) {
			Color[] colT1 = t1.GetPixels();
			Color[] colT2 = t2.GetPixels();
			if (colT1.Length != colT2.Length) Debug.LogWarning("Trying to compare two textures with different dimensions");
			float S = 0;
			float normalizer = 0;
			for (int i = 0; i < Mathf.Min(colT1.Length, colT2.Length); i++)
				S += CrossCorrelate(colT1[i], colT2[i], ref normalizer);
			return normalizer > 0 ? S / normalizer : 0f;
		}

		public static float CrossCorrelationDifference(Texture2D t1, Texture2D t2, int numRow = 512, int numCol = 512) {
			if (numRow < 2 || numCol < 2) {
				Debug.LogWarning("Can't compare images with at most 1 columns or rows using this sampling method");
				return 10000f;
			}
			float S = 0;
			float normalizer = 0;
			for (int i = 0; i < numRow; i++) for (int j = 0; j < numCol; j++) {
				float pu = (float)i / (numRow - 1);
				float pv = (float)i / (numCol- 1);
				S += CrossCorrelate(t1.GetPixelBilinear(pu, pv), t2.GetPixelBilinear(pu, pv), ref normalizer);
			}
			return normalizer > 0 ? S / normalizer : 0f;
		}

		public enum ImageComparisonModes {
			SUM_SQUARE_DIFFERENCE,
			CROSS_CORRELATION
		}

		public enum ImageSamplingModes {
			BILINEAR,
			EXACT
		}

		static float SquareDistance(Color c1, Color c2, ref float normalizer) {
			float res = 0;
			// normalizer += Mathx.Square(c1.grayscale) + Mathx.Square(c2.grayscale);
			// res += Mathx.Square((c1 - c2).grayscale);
			for (int j = 0; j < 3; j++) {
				normalizer += Mathx.Square(c1[j]) + Mathx.Square(c2[j]);
				res += Mathx.Square(c1[j] - c2[j]);
			}
			return res;
		}
		static float CorrectPixel(Color c1, Color c2, ref float normalizer) {
			normalizer += 1;
			for (int j = 0; j < 3; j++) if (Mathf.Abs(c1[j] - c2[j]) > .1f) return 1f;
			return 0f;
		}
		static float CrossCorrelate(Color c1, Color c2, ref float normalizer) {
			float res = 0;
			for (int j = 0; j < 3; j++) {
				normalizer += Mathx.Square(c1[j]) + Mathx.Square(c2[j]);
				res += c1[j] * c2[j];
			}
			return res;
		}
		static float MaxComponent(Color c) => Mathf.Max(c.maxColorComponent, c.a);
	}
}