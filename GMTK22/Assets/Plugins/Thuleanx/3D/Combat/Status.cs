using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using Thuleanx.Optimization;
using UnityEngine.SceneManagement;

namespace Thuleanx3D.Combat {
	public class Status : MonoBehaviour {

		[HorizontalLine(color: EColor.Blue)]
		[Header("Stats")]
		public int MaxHealth;
		
		int hp;

		public int Health {
			get => hp;
			private set => hp = Mathf.Clamp(value, 0, MaxHealth);
		}

		[HorizontalLine(color: EColor.Red)]
		[Header("Events")]
		public UnityEvent OnDeath;
		public UnityEvent<Hit3D> OnHit;
		public BubblePool Corpse;

		public bool IsDead() => Health == 0;

		public void _NoCall_SetHealth(int health) => Health=health;
		public void _NoCall_TakeDamage(int damage) => Health-=damage;

		void Awake() {
			Health = MaxHealth;
		}

		public void _OnHit(Hit3D hit) {
			OnHit?.Invoke(hit);
			if (Health != 0) {
				Health -= hit.damage;
				if (Health==0) {
					OnDeath?.Invoke();
					if (Corpse) {
						GameObject obj = Corpse.Borrow(SceneManager.GetActiveScene(), transform.position);
					}
				}
			}
		}
	}
}