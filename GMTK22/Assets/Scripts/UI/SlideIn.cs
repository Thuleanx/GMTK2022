using UnityEngine;
using UnityEngine.Events;
using Thuleanx.Utils;

namespace Thuleanx.UI {
	public class SlideIn : MonoBehaviour {
		[SerializeField, Range(0, 10f)] float delay;
		[SerializeField, Range(0.01f, 10f)] float effectDuration;
		[SerializeField] Vector2 offsetPos;
		[SerializeField, Range(0,1f)] float durationNoise;
		[SerializeField] UnityEvent OnSlide;
		[SerializeField] UnityEvent OnSlideComplete;
		RectTransform rectTransform;
		Vector2 originalPos;
		Timer delaying;
		bool slideout;
		bool slidingIn = false;
		float effectiveDuration;
		float t;

		void Awake() {
			rectTransform = GetComponent<RectTransform>();
			originalPos = rectTransform.anchoredPosition;
		}

		void OnEnable() {
			delaying = delay;
			t = 0;
			slidingIn = false;
			slideout = false;
			effectiveDuration = Mathx.RandomRange(0, durationNoise) + effectDuration;
			Reposition();
		}

		void LateUpdate() {
			if (!delaying) {
				if (!slidingIn) OnSlide?.Invoke();
				slidingIn = true;
				bool animating = t < effectiveDuration;
				if (slideout) t -= Time.unscaledDeltaTime;
				else t += Time.unscaledDeltaTime;
				if (animating && t >= effectiveDuration) OnSlideComplete?.Invoke();
			}
			Reposition();
		}


		void Reposition() {
			float tr = Mathf.Clamp01(t/effectiveDuration);
			rectTransform.anchoredPosition = Vector2.Lerp(
				originalPos + offsetPos,
				originalPos,
				EasingFunction.EaseOutCirc(0, 1f, tr)
			);
		}

		public void SlideOut() {
			slideout = true;
			t = effectiveDuration;
		} 
	}
}