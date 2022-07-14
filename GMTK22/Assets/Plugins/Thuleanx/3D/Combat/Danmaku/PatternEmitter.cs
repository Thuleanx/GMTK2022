using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine.SceneManagement;
using System;
using Thuleanx.Utils;
using Thuleanx.DS;

// Requirement: an object with "Player" tag
namespace Thuleanx3D.Combat.Danmaku {
	public class PatternEmitter : MonoBehaviour {
		GameObject Player;
		public PatternOutput Source;
		[SerializeField] Vector3 Up = Vector3.up;

		void OnEnable() {
			Player = GameObject.FindGameObjectWithTag("Player");
		}

		Vector3 ProjOrthoPlane(Vector3 vec) => vec - Vector3.Project(vec, Up);
		CoroutineHandle currentEmission;

		[Button]
		public CoroutineHandle Emit() {
			Stop();
			return currentEmission = this.RunCoroutine(_Emit());
		}

		public void Stop() {
			if (currentEmission != null) {
				StopAllCoroutines();
				currentEmission = null;
			}
		}

		public IEnumerator _Emit() {
			// Please implement Priority Queue 
			PriorityQueue<BulletInstance> Instances = new PriorityQueue<BulletInstance>(
				Comparer<BulletInstance>.Create((a, b) => (int) Mathf.Sign(a.Delay - b.Delay)));
			IEnumerator<BulletInstance> Subpatterns = 
				Source.GetPattern().GetAllBullets();
			while (Subpatterns.MoveNext())
				Instances.Enqueue(Subpatterns.Current);
			float startTime = Time.time;
			while (Instances.Count > 0) {
				BulletInstance BulletData = Instances.Dequeue();

				while (Time.time - startTime < BulletData.Delay) yield return null;
				GameObject Bullet = BulletData.Bullet.Borrow(
					SceneManager.GetActiveScene(),
					transform.position + transform.forward * BulletData.Pos.x + transform.right * BulletData.Pos.y
				);
				Vector3 Dir = (BulletData.AimDir ? 
					ProjOrthoPlane(Player.transform.position - Bullet.transform.position).normalized : transform.forward);
				
				Dir = Quaternion.AngleAxis(BulletData.Dir * Mathf.Rad2Deg, Up) * Dir;
				Bullet3D bullet3D = Bullet.GetComponent<Bullet3D>();
				bullet3D.Initialize(Dir * BulletData.Speed, true, true);
				if (BulletData.Period > 0 && (BulletData.Duration == -1 || BulletData.Duration >= BulletData.Period)) {
					BulletData.Delay += BulletData.Period;
					BulletData.Duration -= BulletData.Period;
					Instances.Enqueue(BulletData);
				}
			}
		}
	}
}