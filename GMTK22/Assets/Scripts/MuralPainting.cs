using UnityEngine;

namespace WizOsu.Painting {
	[RequireComponent(typeof(Renderer))]
	public class MuralPainting : MonoBehaviour {
		public static Texture2D painting;
		public Texture2D emptyMask;

		void Start() {
			if (painting) {
				GetComponent<Renderer>().material.SetTexture("_MainTex", painting);
			}
			GetComponent<Renderer>().material.SetTexture("_MaskTexture", emptyMask);
		}
	}
}