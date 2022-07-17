using System;
using UnityEngine;
using UnityEngine.UI;
using WizOsu.Patterns;
using Thuleanx;
using UnityEngine.SceneManagement;
using DG.Tweening;
using NaughtyAttributes;

namespace WizOsu {
	public class TransitionManager : Singleton<TransitionManager> {
		[SerializeField, Required] Image fader;
		[SerializeField, Range(0, 10f)] float fadeInDuration = 3f;
		[SerializeField, Range(0, 10f)] float fadeOutDuration = 3f;
		[SerializeField] Ease fadeInEase = Ease.InCirc;
		[SerializeField] Ease fadeOutEase = Ease.InCirc;

		void Start() {
			FadeIn();
			App.AfterSceneLoad.AddListener(AfterSceneLoad);
		}

		void AfterSceneLoad(Scene scene, LoadSceneMode mode) => FadeIn();


		public void FadeIn(Action onComplete = null) {
			Color colS = fader.color; colS.a = 1f;
			Color colT = fader.color; colT.a = 0f;
			fader.raycastTarget = true;
			fader.DOColor(colT, fadeInDuration).From(colS).SetEase(fadeInEase).OnComplete(
				() => { fader.raycastTarget = false; onComplete?.Invoke(); }
			).Play();
		}

		public void Fadeout(Action onComplete = null) {
			Color colS = fader.color; colS.a = 1f;
			Color colT = fader.color; colT.a = 0f;
			fader.raycastTarget = true;
			fader.DOColor(colS, fadeOutDuration).From(colT).SetEase(fadeOutEase).Play().OnComplete(
				()=>onComplete?.Invoke()
			);
		}
	}
}