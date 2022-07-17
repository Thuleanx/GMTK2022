using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

using NaughtyAttributes;

namespace Thuleanx.Ballison {
	public class AudioManager : MonoBehaviour
	{
		public static AudioManager Instance = null;
		public AudioTrack MainTrack {get; private set; }
		[ReorderableList]
		public List<FMODUnity.EventReference> References;
		public bool PlayOnAwake = false;
		int trackIndex;
		bool prepped = false;

		void Awake() {
			Instance = this;
			trackIndex = 0;
			if (PlayOnAwake) Play();
		}

		public void Play() {
			SetMainTrack(new AudioTrack(References[trackIndex]));
		}

		public AudioTrack GetTrack(FMODUnity.EventReference reference) {
			return new AudioTrack(reference);
		}

		public void SetMainTrack(AudioTrack track) {
			MainTrack = track;
			track.Play();
		}

		public void RemoveMainTrack() => RemoveMainTrack(MainTrack);

		public void RemoveMainTrack(AudioTrack track) {
			if (track == MainTrack)
				MainTrack = null;
		}

		void Update() {
			if (MainTrack != null && MainTrack.GetTrackTimeMS() >= MainTrack.GetTrackDuration()) {
				RemoveMainTrack();
				trackIndex++;
				if (trackIndex == References.Count) trackIndex = 0;
				SetMainTrack(new AudioTrack(References[trackIndex]));
			}
		}

		void OnDestroy() {
			if (MainTrack != null) MainTrack.Stop();
		}
	}
}