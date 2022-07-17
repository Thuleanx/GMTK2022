using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using Thuleanx.Utils;

namespace Thuleanx.Animation {
	public class PopAppear : MonoBehaviour {
		[SerializeField, MinMaxSlider(0, 2f)] Vector2 duration;
		[SerializeField, MinMaxSlider(0, 2f)] Vector2 delay;
		[SerializeField] Ease easing = Ease.OutElastic;

		Vector3 scale;
		void OnEnable() {
			scale = transform.localScale;
			transform.DOScale(transform.localScale, Mathx.RandomRange(duration))
				.SetEase(easing).From(Vector3.zero).SetDelay(
					Mathx.RandomRange(delay)
			);
		}

		void OnDisable() {
			transform.localScale = scale;
		}
	}
}