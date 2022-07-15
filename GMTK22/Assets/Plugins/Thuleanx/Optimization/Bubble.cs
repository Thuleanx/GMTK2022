using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Thuleanx.Utils;
using UnityEngine.SceneManagement;

namespace Thuleanx.Optimization {
	public class Bubble : MonoBehaviour {
		[HideInInspector] public BubblePool Pool;
		[HideInInspector] public bool InPool = false;
		[HideInInspector] public Scene scene;

		public virtual void Dispose() {
			ReturnToPool();
		}
		protected void ReturnToPool() {
			if (!InPool) Pool.Collects(this);
		}
		void OnDisable() { 
			if (!InPool) Pool.BubbleLoss(this);
		}
		public IEnumerator CollectsAfterOneFrame() {
			yield return null;
			if (!InPool) Pool.Collects(this);
		}
	}
}