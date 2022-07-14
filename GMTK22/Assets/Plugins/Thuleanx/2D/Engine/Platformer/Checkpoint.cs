using UnityEngine;

namespace Thuleanx2D.Engine.Platformer {
	public class Checkpoint : MonoBehaviour {
		public float duration;
		public Vector2 position => transform.position;
	}
}