using UnityEngine;
using XNode;
using System.Collections.Generic;
using System.Collections;

namespace Thuleanx3D.Combat.Danmaku {
	public class Combine : Node {
		[Input] public PatternComposite In;
		[Output] public PatternComposite Out;

		public override object GetValue(NodePort port) {
			if (port.fieldName == "Out") return new PatternComposite(_GetAllBullets);
			return null;
		}

		IEnumerator<BulletInstance> _GetAllBullets() {
			PatternComposite[] composites = 
				GetInputValues<PatternComposite>("In");
			foreach (PatternComposite Compose in composites) {
				IEnumerator<BulletInstance> Subinstances = Compose.GetAllBullets();
				while (Subinstances.MoveNext())
					yield return Subinstances.Current;
			}
		}
	}
}