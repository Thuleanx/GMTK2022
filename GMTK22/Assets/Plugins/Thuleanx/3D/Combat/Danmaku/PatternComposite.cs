using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Thuleanx3D.Combat.Danmaku {
	[System.Serializable]
	public class PatternComposite {
		public Func<IEnumerator<BulletInstance>> GetAllBullets;

		public PatternComposite(Func<IEnumerator<BulletInstance>> Instance) 
			=> GetAllBullets = Instance;
	}
}