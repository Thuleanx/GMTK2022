using UnityEngine;

namespace Thuleanx.Utils {
	public static class ExtensionMethod {
		public static Texture2D toTexture2D(this RenderTexture rTex) {
			Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
			var old_rt = RenderTexture.active;
			RenderTexture.active = rTex;

			tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
			tex.Apply();

			RenderTexture.active = old_rt;
			return tex;
		}
	}
}