using UnityEngine;
using Thuleanx.Utils;
using DG.Tweening;

namespace WizOsu.Animation {
	public class Jumpy : MonoBehaviour {
		[SerializeField, Min(0.1f)] float jumpPerMinute = 80f;
		[SerializeField, Range(0, 1f)] float jumpProbability = .1f;
		[SerializeField, Range(0, 2f)] float jumpHeight = 2f;
		[SerializeField, Range(0, 1f)] float jumpDuration = .5f;
		Timer jumpCooldown;
		bool jumping;

		void OnEnable() {
			jumpCooldown = Mathx.RandomRange(0, 60f / jumpPerMinute);
		}

		void Update() {
			if (!jumpCooldown && !jumping) {
				bool toJump = Mathx.RandomRange(0f, 1f) < jumpProbability;
				if (toJump) Jump();
				jumpCooldown = Mathx.RandomRange(0, 120f /jumpPerMinute);
			}
		}

		void Jump() {
			jumping = true;
			Sequence seq = DOTween.Sequence();
			float curHeight = transform.position.y;
			seq.Append(transform.DOMoveY(jumpHeight + curHeight, jumpDuration/2f).SetEase(Ease.OutCirc).From(curHeight));
			seq.Append(transform.DOMoveY(curHeight, jumpDuration/2f).SetEase(Ease.InCirc).From(jumpHeight + curHeight));
			seq.OnComplete(() => {
				jumping= false;
			});
		}
	}
}