using UnityEngine;
using UnityEngine.Events;
using FMODUnity;

namespace Thuleanx.Ballison {
	public class AudioTrack {
		FMOD.Studio.EventInstance track;

		public AudioTrack(EventReference reference) {
			track = FMODUnity.RuntimeManager.CreateInstance(reference);
		}

		public int GetTrackTimeMS() {
			int timelinePos;
			if (track.getTimelinePosition(out timelinePos) == FMOD.RESULT.OK) 
				return timelinePos;
			else return -1;
		}

		public void SetTime(int time) {
			if (track.setTimelinePosition(time) != FMOD.RESULT.OK) {
			}
		}

		public int GetTrackDuration() {
			FMOD.Studio.EventDescription description;
			if (track.getDescription(out description) != FMOD.RESULT.OK) {
				return -1;
			}
			int length;
			if (description.getLength(out length) != FMOD.RESULT.OK)
				return -1;
			return length;
		}

		public void Play() => track.start();
		public void Pause() => track.setPaused(true);
		public void Resume() => track.setPaused(false);
		public void Stop() => track.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}
}