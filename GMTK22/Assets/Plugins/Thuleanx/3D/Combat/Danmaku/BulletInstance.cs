using UnityEngine;
using Thuleanx.Optimization;
using Thuleanx.Utils;

namespace Thuleanx3D.Combat.Danmaku {
	[System.Serializable]
	public class BulletInstance {
		public BubblePool Bullet;
		public Vector2 Pos;
		public float Dir;
		public bool AimDir;
		public float Speed;
		public float Delay = 0;
		public float Period = 0;
		public float Duration = -1;

		public BulletInstance(BubblePool Bullet, Vector3 Pos, float Dir = 0, bool AimDir = false,
			float Speed = 0, float Delay = 0, float Period = 0, float Duration = -1) {
			
			this.Pos = Pos;
			this.Bullet = Bullet;
			this.AimDir = AimDir;
			this.Dir = Dir;
			this.Speed = Speed;
			this.Delay = Delay;
			this.Period = 0;
			this.Duration = Duration;
		}

		public BulletInstance(BulletInstance clone) {
			this.Pos = clone.Pos;
			this.Bullet = clone.Bullet;
			this.AimDir = clone.AimDir;
			this.Dir = clone.Dir;
			this.Speed = clone.Speed;
			this.Delay = clone.Delay;
			this.Period = clone.Period;
			this.Duration = clone.Duration;
		}
	}
}