using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Thuleanx3D.Combat {
	[RequireComponent(typeof(Collider))]
	public class Hitbox3D : MonoBehaviour {
		protected enum ColliderState {
			Open,
			Closed
		}

		public Collider Collider {get; private set; }
		public bool DefaultActive;
		public float Knockback;

		Dictionary<long,float> HurtboxCD = new Dictionary<long,float>();
		ColliderState _state;
		public UnityEvent<Hit3D> OnHit;
		protected ColliderState State {
			get => _state;
			set {
				_state = value;
				Collider.enabled = value == ColliderState.Open;
			}
		}

		[SerializeField, Min(0f)] float Frequency;

		public virtual Hit3D GenerateHit(Collider other) {
			return new Hit3D(1, this, other.GetComponent<Hurtbox3D>(), Knockback);
		}

		void OnTriggerStay(Collider other) {
			Hurtbox3D hurtbox = other.GetComponent<Hurtbox3D>();
			if (hurtbox && hurtbox.CanTakeHit() && TimedOut(hurtbox.ID)) {
				Hit3D Hit = GenerateHit(other);
				hurtbox.ApplyHit(Hit);
				OnHit?.Invoke(Hit);
				Refresh(hurtbox.ID);
			}
		}

		public void startCheckingCollision() => State = ColliderState.Open;
		public void stopCheckingCollision() {
			State = ColliderState.Closed;
			HurtboxCD.Clear();
		} 

		bool TimedOut(long hurtboxID) => !HurtboxCD.ContainsKey(hurtboxID) 
			|| (Frequency > 0 && HurtboxCD[hurtboxID] + Frequency < Time.time);
		void Refresh(long hurtboxID) => HurtboxCD[hurtboxID] = Time.time;

		void Awake() {
			Collider = GetComponent<Collider>();
		}

		void OnEnable() {
			Reset();
			if (DefaultActive) 	startCheckingCollision();
			else 				stopCheckingCollision();
		}

		void Reset() => HurtboxCD.Clear();
	}
}