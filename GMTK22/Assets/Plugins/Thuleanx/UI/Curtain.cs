using UnityEngine;
using UnityEngine.UI;
using Thuleanx.Utils;

namespace Thuleanx.UI {
	[RequireComponent(typeof(RawImage))]
	public class Curtain : MonoBehaviour {
		public enum State {
			Closed,
			Open
		}

		RawImage Image;
		public State CurState = Curtain.State.Open;
		[SerializeField] float lambda;

		float Alpha {
			get => Image.color.a;
			set {
				Color col = Image.color;
				col.a = Mathf.Clamp01(value);
				Image.color = col;
			}
		}

		void Awake() {
			Image = GetComponent<RawImage>();
		}

		void OnEnable() {
			Alpha = 1;
		}

		void Update() {
			Alpha = Calc.Damp(Alpha, (CurState == State.Closed ? 1f : 0f), lambda, Time.deltaTime);
		}
	}
}