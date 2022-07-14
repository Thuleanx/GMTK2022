using UnityEngine;
using Thuleanx3D.Combat;
using Thuleanx.Utils;
using Thuleanx.Enums;

namespace Thuleanx3D.Combat.Danmaku {
	[RequireComponent(typeof(Rigidbody))]
	public class Bullet3D : MonoBehaviour {
		public Rigidbody Body {get; private set; }

		public Vector3 Velocity { get => Body.velocity; set => Body.velocity = value; }
		public Optional<Hitbox3D> Hitbox;

		public void Initialize(Vector3 Velocity, bool isEnemy, bool faceDir = true) {
			this.Velocity = Velocity;
			if (Hitbox.Enabled) 
				Hitbox.Value.gameObject.layer = 
				(int) (isEnemy ? Layer.HostileHitbox : Layer.FriendlyHitbox);
			if (faceDir) transform.LookAt(transform.position + Velocity, Vector3.up);
		}

		void Awake() {
			Body = GetComponent<Rigidbody>();
		}
	}
}