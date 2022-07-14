using UnityEngine;

using Thuleanx.Utils;

namespace Thuleanx.Engine {
	public class ExpireAfterDuration : MonoBehaviour {
		public float Duration;
		Timer _Timer;

		void OnEnable() {
			_Timer = Duration;
		}

		void Update() {
			if (!_Timer) General.TryDispose(gameObject);
		}
	}
}