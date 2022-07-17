using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using Thuleanx.Utils;
using Thuleanx.Audio;

namespace Thuleanx.Animation {
	public class PopAppear : MonoBehaviour {
		[SerializeField, MinMaxSlider(0, 2f)] Vector2 duration;
		[SerializeField, MinMaxSlider(0, 2f)] Vector2 delay;
		[SerializeField] Ease easing = Ease.OutElastic;
		[SerializeField] float popPerMinute = 30f;
		[SerializeField, Range(1f,2f)] float popScale = 1.2f;
		[SerializeField] FMODUnity.EventReference PopSound;

		Vector3 scale;
		void OnEnable() {
			scale = transform.localScale;
			transform.DOScale(transform.localScale, Mathx.RandomRange(duration))
				.SetEase(easing).From(Vector3.zero).SetDelay(
					Mathx.RandomRange(delay)
			).OnComplete(PopOverTime).OnStart(()=> {
				AudioManager.Instance?.PlayOneShot(PopSound);
			});
		}

		void PopOverTime() {
			Sequence seq = DOTween.Sequence();
			seq.Append(transform.DOScale(scale * popScale, 60f / popPerMinute / 2).SetEase(Ease.InOutSine).From(scale));
			seq.Append(transform.DOScale(scale, 60f / popPerMinute / 2).SetEase(Ease.InOutSine).From(scale*popScale));
			seq.SetLoops(-1);
		}

		void OnDisable() {
			transform.localScale = scale;
		}
	}
}