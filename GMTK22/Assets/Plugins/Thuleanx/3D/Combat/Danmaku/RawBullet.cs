using UnityEngine;
using XNode;
using Thuleanx.Optimization;
using System.Collections;
using System.Collections.Generic;

namespace Thuleanx3D.Combat.Danmaku {
	public class RawBullet : Node {
		public BubblePool Bullet;
		public Vector2 Pos = Vector3.zero;

		[Output(connectionType = ConnectionType.Multiple)] 
		public PatternComposite Out;

		public override object GetValue(NodePort port) {
			if (port.fieldName == "Out") return new PatternComposite(_GetAllBullets);
			return null;
		}

		IEnumerator<BulletInstance> _GetAllBullets() {
			yield return new BulletInstance(Bullet, Pos);
		}
	}
}