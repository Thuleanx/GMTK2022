using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Thuleanx.UI {
	public class FloatingText : TMP_Mesh_Manipulation {
		[SerializeField] float WoobleScale = 1f / 64;

		// Update is called once per frame
		public override void Update() {
			base.Update();
			int p = 0;
			Vector3 offset = Wobble(Time.unscaledTime + p++);

			for (int i = 0; i < textMesh.textInfo.characterCount; i++) {
				TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];
				if (c.character == ' ') {
					offset = Wobble(Time.time + p++);
				} else {
					int index = c.vertexIndex;
					for (int j = 0; j < 4; j++)
						vertices[index + j] = vertices[index + j] + offset * WoobleScale;
				}
			}
		}

		Vector2 Wobble(float time) {
			return new Vector2(Mathf.Sin(time * 3.3f), Mathf.Cos(time * 2f));
		}
	}
}