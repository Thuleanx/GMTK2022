using UnityEngine;

namespace Thuleanx2D.Combat {
	[RequireComponent(typeof(Collider2D))]
	public abstract class Hurtbox : MonoBehaviour, IHurtable {
		public static long NextID = 0;

		public long ID;
		public Collider2D Box { get; private set; }

		void Awake() {
			Box = GetComponent<BoxCollider2D>();
			ID = NextID++;
		}
		public abstract void ApplyHit(IHit hit);
		public abstract bool CanTakeHit();
	}
}