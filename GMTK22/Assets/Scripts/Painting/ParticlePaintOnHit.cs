using UnityEngine;
using System.Collections.Generic;
using TNTC.Painting;
using Thuleanx.Utils;
using NaughtyAttributes;

namespace WizOsu.Painting {
	[RequireComponent(typeof(ParticleSystem))]
	public class ParticlePaintOnHit : MonoBehaviour {
		public ParticleSystem ParticleSystem { get; private set;  }

		[SerializeField, MinMaxSlider(0, 1f)] Vector2 particleHitRadiusRange;
		[SerializeField, Range(0, 1f)] float strength;
		[SerializeField, Range(0, 1f)] float hardness;
		[SerializeField] Color paintColor;

		void Awake() {
			ParticleSystem = GetComponent<ParticleSystem>();
		}

		void OnParticleCollision(GameObject other) {
			List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
			int numCollisions = ParticleSystem.GetCollisionEvents(other, collisionEvents);

			Paintable paintable = other.GetComponent<Paintable>();
			if (paintable) {
				foreach (ParticleCollisionEvent collisionEvent in collisionEvents) {
					PaintManager.instance.paint(paintable, collisionEvent.intersection,
						Mathx.RandomRange(particleHitRadiusRange), hardness, strength, paintColor
					);
				}
			}
		}
	}
}