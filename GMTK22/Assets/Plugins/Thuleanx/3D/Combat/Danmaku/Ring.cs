using UnityEngine;
using XNode;
using System.Collections;
using System.Collections.Generic;
using Thuleanx.Utils;
using NaughtyAttributes;

namespace Thuleanx3D.Combat.Danmaku {
	public class Ring : Node {
		[Input(connectionType = ConnectionType.Override)] public int n = 3;
		[Input(connectionType = ConnectionType.Override)] public Vector2 angles;
		[HorizontalLine(color: EColor.Red)]
		[Input(connectionType = ConnectionType.Override)] public float distFromCenter;

		[Input(connectionType = ConnectionType.Override)] public PatternComposite In;
		[Output] public PatternComposite Out;

		public override object GetValue(NodePort port) {
			if (port.fieldName == "Out") return new PatternComposite(_GetAllBullets);
			return null;
		}

		IEnumerator<BulletInstance> _GetAllBullets() {
			PatternComposite Compose = GetInputValue<PatternComposite>("In");
			if (Compose != null) {
				IEnumerator<BulletInstance> Subinstances = Compose.GetAllBullets();
				while (Subinstances.MoveNext()) {
					BulletInstance instance = Subinstances.Current;
					if (instance != null) {
						for (int k = 0; k < n; k++) {
							BulletInstance clone = new BulletInstance(instance);
							float nxtDir = instance.Dir + Mathf.Lerp(angles.x, angles.y, (float) k/(n-1)) * Mathf.Deg2Rad; 
							Vector2 displacement = new Vector2(
								Mathf.Cos(nxtDir), 
								Mathf.Sin(nxtDir)
							);
							clone.Pos += displacement * distFromCenter;
							clone.Dir = nxtDir;
							yield return clone;
						}
					}
				}
			}
		}
	}
}