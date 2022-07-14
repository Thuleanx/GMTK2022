using System;
using System.Collections;
using UnityEngine;

namespace Thuleanx.Utils {
	public class CoroutineHandle : IEnumerator {
		public bool IsDone { get; private set; }

		public bool MoveNext() => !IsDone;
		public object Current { get; }
		public void Reset() { }

		private event Action<CoroutineHandle> CompletedInternal;
		public event Action<CoroutineHandle> Completed {
			add {
				if (IsDone)
					value(this);
				else
					CompletedInternal += value;
			}
			remove => CompletedInternal -= value;
		}

		public CoroutineHandle(MonoBehaviour owner, IEnumerator coroutine) {
			Current = owner.StartCoroutine(Wrap(coroutine));
		}

		private IEnumerator Wrap(IEnumerator coroutine) {
			yield return coroutine;
			IsDone = true;
			CompletedInternal?.Invoke(this);
		}
	}
	public static class MonoBehaviourExtensions {
		public static CoroutineHandle RunCoroutine(this MonoBehaviour owner, IEnumerator coroutine) {
			return new CoroutineHandle(owner, coroutine);
		}
	}
}
