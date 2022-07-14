using UnityEngine;
using UnityEngine.Events;
using Thuleanx.Utils;

namespace Thuleanx3D.Combat {
	public class Hurtbox3D : MonoBehaviour {
		public static long NextID = 0;

		[HideInInspector]
		public long ID;
		public Collider Collider { get; private set; }
		public Timer Iframe;

		public UnityEvent<Hit3D> OnHit;

		void Awake() {
			Collider = GetComponent<Collider>();
			ID = NextID++;
		}

		public void ApplyHit(Hit3D hit) => OnHit.Invoke(hit);
		public bool CanTakeHit() => !Iframe;
		public void GiveIframe(float duration) => Iframe = duration;

	}
}