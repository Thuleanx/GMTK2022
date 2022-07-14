using UnityEngine;
using XNode;
using NaughtyAttributes;
using System;	
using System.Collections;	
using System.Collections.Generic;	
using Thuleanx.Utils;
using Thuleanx.Enums;

namespace Thuleanx3D.Combat.Danmaku {
	public class Aim : Node {
		[System.Serializable]
		public enum Mode {
			FIXED = 0,
			AIM = 1
		}

		public Mode mode = Aim.Mode.FIXED;

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
				Instance.AimDir = mode == Mode.AIM;
				yield return Instance;
			}
		}
	}
}