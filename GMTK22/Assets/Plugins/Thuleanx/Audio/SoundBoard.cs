using UnityEngine;
using System.Collections.Generic;

namespace Thuleanx.Audio {
	public class SoundBoard : MonoBehaviour {
		public List<FMODUnity.EventReference> Event = new List<FMODUnity.EventReference>();

		public void PlaySound(int id) {
			AudioManager.Instance.PlayOneShot(Event[id]);
		}

		public void PlaySound3D(int id) {
			AudioManager.Instance.PlayOneShot3D(Event[id], transform.position);
		}
	}
}