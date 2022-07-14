using UnityEngine;

namespace Thuleanx3D.Combat {
	public struct Hit3D {
		public int damage;
		public float knockback;

		public Hitbox3D Hitbox;
		public Hurtbox3D Hurtbox;

		public Hit3D(int damage, Hitbox3D hitbox, Hurtbox3D hurtbox, float knockback = 0) {
			this.damage = damage;
			this.Hitbox = hitbox;
			this.Hurtbox = hurtbox;
			this.knockback = knockback;
		}
	}
}