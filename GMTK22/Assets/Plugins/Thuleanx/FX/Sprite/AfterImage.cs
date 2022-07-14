using UnityEngine;
using Thuleanx.Utils;

namespace Thuleanx.FX {
	public class AfterImage : MonoBehaviour {
		public Optional<SpriteRenderer> Sprite;
		public float lifetime = 3;
		float original;
		Timer Alive;

		void Awake() {
			if (!Sprite.Enabled) Sprite = new Optional<SpriteRenderer>(GetComponentInChildren<SpriteRenderer>());
			if (Sprite.Enabled) original = Sprite.Value.color.a;
		}

		void OnEnable() {
			Alive = lifetime;
			Alive.Start();
		}

		private void Update() {
			if (Sprite.Enabled) {
				Color col = Sprite.Value.color;
				col.a = Mathf.Lerp(0f, 1f, Alive.TimeLeft / lifetime) * original;
				Sprite.Value.color = col;
			}
			if (!Alive) gameObject.SetActive(false);
		}
	}
}