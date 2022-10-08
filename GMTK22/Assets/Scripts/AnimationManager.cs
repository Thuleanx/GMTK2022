using UnityEngine;
using DG.Tweening;
using System.Collections;
using WizOsu.Patterns;
using Thuleanx.Audio;

namespace WizOsu.Animation {
	public class AnimationManager : Singleton<AnimationManager> {
		[SerializeField, Range(0, 2f)] float hopHeight = 2f;
		[SerializeField, Range(0, 1f)] float hopDuration = .5f;
		[SerializeField, Range(0, 10f)] float hopAnimationDuration = 1.5f;
		[SerializeField, Range(0, 10)] int hopsAfterDestination = 3;
		[SerializeField] FMODUnity.EventReference hopSound;

		public IEnumerator DoBunnyHop(Transform obj, Vector3 destination) {
			Tween moveX = obj.DOMoveX(destination.x, hopAnimationDuration).SetEase(Ease.Linear);
			Tween moveZ = obj.DOMoveZ(destination.z, hopAnimationDuration).SetEase(Ease.Linear);
			moveX.Play();
			moveZ.Play();
			Sequence seq = DOTween.Sequence();
			float curHeight = destination.y;
			seq.InsertCallback(0, () => {
				AudioManager.Instance?.PlayOneShot(hopSound);
			});
			seq.Append(obj.DOMoveY(hopHeight + curHeight, hopDuration/2f).SetEase(Ease.OutCirc).From(curHeight));
			seq.Append(obj.DOMoveY(curHeight, hopDuration/2f).SetEase(Ease.InCirc).From(hopHeight + curHeight));
			seq.SetLoops(-1);
			yield return new WaitForSeconds(hopAnimationDuration);
			yield return seq.WaitForElapsedLoops(seq.CompletedLoops() + hopsAfterDestination);
			seq.Kill();
			obj.transform.position = destination;
		}

		// public IEnumerator DoWiggleMove(Transform obj, Vector3 destination, float duration) {
		// 	Tween moveX = obj.DOMoveX(destination.x, duration).SetEase(Ease.Linear);
		// 	moveX.Play();

		// 	Sequence seq = DOTween.Sequence();
		// 	yield return new WaitForSeconds(duration);
		// 	yield return seq.WaitForElapsedLoops(seq.CompletedLoops() + hopsAfterDestination);
		// 	seq.Kill();
		// }
	}
}