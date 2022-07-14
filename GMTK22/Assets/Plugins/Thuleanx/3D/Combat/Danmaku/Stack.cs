using UnityEngine;
using XNode;
using System.Collections;
using System.Collections.Generic;
using Thuleanx.Utils;
using NaughtyAttributes;

namespace Thuleanx3D.Combat.Danmaku {
	public class Stack : Node {
		[Input(connectionType = ConnectionType.Override), Min(1)] public int n = 2;
		[Input(connectionType = ConnectionType.Override)] public Vector2 SpeedRange;

		[Input(connectionType = ConnectionType.Override)] public PatternComposite In;
		[Output] public PatternComposite Out;

		public override object GetValue(NodePort port) {
			if (port.fieldName == "Out") return new PatternComposite(_GetAllBullets);
			return null;
		}

		IEnumerator<BulletInstance> _GetAllBullets() {
			PatternComposite Compose = GetInputValue<PatternComposite>("In");
			if (Compose != null) {
				for (int k = 0; k < n; k++) {
					IEnumerator<BulletInstance> Subinstances = Compose.GetAllBullets();
					while (Subinstances.MoveNext()) {
						BulletInstance instance = Subinstances.Current;
						instance.Speed = Mathf.Lerp(SpeedRange.x, SpeedRange.y, (float) k / (n-1));
						yield return instance;
					}
				}
			}
		}
	}
}