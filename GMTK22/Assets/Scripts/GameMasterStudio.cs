using UnityEngine;
using System.Collections;
using WizOsu.Animation;
using WizOsu.Patterns;
using NaughtyAttributes;

namespace WizOsu {
	public class GameMasterStudio : Singleton<GameMasterStudio> {
		[SerializeField, Required] GameObject wizard;
		[SerializeField, Required] GameObject npc;

		#region Positional Anchors
		[Header("Anchor Positions")]
		[HorizontalLine(color: EColor.Red)]
		[SerializeField] Transform npcEntrancePos;
		[SerializeField] Transform npcDestinationPos;
		#endregion

		void Start() {
			StartCoroutine(IntroSequence());
		}

		public IEnumerator IntroSequence() {
			yield return AnimationManager.instance?.DoBunnyHop(npc.transform, Vector2.zero, 4f);
		}
	}
}