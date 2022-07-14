using UnityEngine;
using XNode;
using System.Collections;
using System.Collections.Generic;
using Thuleanx.Utils;
using NaughtyAttributes;
using Thuleanx.Enums;

namespace Thuleanx3D.Combat.Danmaku {
	public class PatternPeriod : Node {
		public CompositionMode mode = CompositionMode.OVERRIDE;
		[Input(connectionType =ConnectionType.Override)] public float Period = 0;
		[Input(connectionType =ConnectionType.Override)] public PatternComposite In;
		[Output] public PatternComposite Out;

		public override object GetValue(NodePort port) {
			if (port.fieldName == "Out") return new PatternComposite(_GetAllBullets);
			return null;
		}

		IEnumerator<BulletInstance> _GetAllBullets() {
			PatternComposite Compose = GetInputValue<PatternComposite>("In");
			IEnumerator<BulletInstance> Subinstances = Compose.GetAllBullets();
			for (int k = 0; Subinstances.MoveNext(); k++) {
				BulletInstance Instance = Subinstances.Current;
				if (mode == CompositionMode.OVERRIDE)			Instance.Period = Period;
				else if (mode == CompositionMode.ADDITIVE)		Instance.Period += Period;
				yield return Instance;
			}
		}
	}
}