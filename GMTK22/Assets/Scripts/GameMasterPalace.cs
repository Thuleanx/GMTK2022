using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Collections;
using WizOsu.Animation;
using WizOsu.Patterns;
using WizOsu.Painting;
using TNTC.Painting;
using Thuleanx.FX.Particles;
using Thuleanx.Utils;
using Yarn.Unity;
using Thuleanx;

namespace WizOsu {
	public class GameMasterPalace : MonoBehaviour {
		[Header("References")]
		[SerializeField] GameObject wizard;
		[SerializeField, Required] GameObject npc;
		[SerializeField, Required] MagicCanvas drawingCanvas;
		[SerializeField, Required] DialogueRunner dialogueRunner;
		[SerializeField, Required] ParticlePaintOnHit brush;
		[SerializeField] SceneReference nextScene;
		[SerializeField] ParticleCombo wandParticles;

		#region Positional Anchors
		[Header("Anchor Positions")]
		[HorizontalLine(color: EColor.Red)]
		[SerializeField] Transform npcEntrancePos;
		[SerializeField] Transform npcDestinationPos;
		#endregion

		[Header("Painting")]
		[HorizontalLine(color: EColor.Red)]
		[SerializeField] string requestNode;
		[SerializeField] Sprite squireSprite;
		[SerializeField] string completeNode;
		[SerializeField] Sprite kingSprite;
		[SerializeField, Range(0, 300f)] float durationSeconds;
		[SerializeField, MinMaxSlider(0, 10f)] Vector2 colorChangeIntervalRange;
		[SerializeField] Texture2D palette;

		void Start() {
			StartCoroutine(Sequence_MainGameLoop());
		}

		public IEnumerator Sequence_MainGameLoop() {
			bool waiting = true;
			TransitionManager.instance.FadeIn(() => waiting = false);
			while (waiting) yield return null;

			yield return Sequence_Squire();
			yield return Sequence_Freedraw();
			yield return Sequence_King();

			waiting = true;
			TransitionManager.instance.FadeOut(() => waiting = false);
			while (waiting) yield return null;
			App.Instance.RequestLoad(nextScene.SceneName);
		}

		public IEnumerator Sequence_Freedraw() {
			wandParticles?.Activate();

			Color[] paletteColors = palette.GetPixels();

			Timer workingOnOrder = durationSeconds;
			Timer colorChangeCD = 0;
			while (workingOnOrder) {
				if (!colorChangeCD) {
					Color nxtCol = paletteColors[Mathx.RandomRange(0, paletteColors.Length)];
					brush.paintColor = nxtCol;
					brush.GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", nxtCol);
					colorChangeCD = Mathx.RandomRange(colorChangeIntervalRange);
				}
				yield return null;
			}

			wandParticles?.StopProducing();
		}

		public IEnumerator Sequence_Squire() {
			npc.GetComponentInChildren<SpriteRenderer>().flipX = false;
			npc.GetComponentInChildren<SpriteRenderer>().sprite = squireSprite;
			yield return AnimationManager.instance?.DoBunnyHop(npc.transform, npcDestinationPos.position);

			yield return WaitForDialogue(requestNode);

			npc.GetComponentInChildren<SpriteRenderer>().flipX = true;
			yield return AnimationManager.instance?.DoBunnyHop(npc.transform, npcEntrancePos.position);
		}

		public IEnumerator Sequence_King() {
			npc.GetComponentInChildren<SpriteRenderer>().flipX = false;
			npc.GetComponentInChildren<SpriteRenderer>().sprite = kingSprite;
			yield return AnimationManager.instance?.DoBunnyHop(npc.transform, npcDestinationPos.position);

			yield return WaitForDialogue(completeNode);

			npc.GetComponentInChildren<SpriteRenderer>().flipX = true;
			yield return AnimationManager.instance?.DoBunnyHop(npc.transform, npcEntrancePos.position);
		}

		bool dialogueCompleted = false;

		public IEnumerator WaitForDialogue(string node) {
			if (node != null && node.Length > 0) {
				dialogueCompleted = false;
				dialogueRunner.StartDialogue(node);
				while (!dialogueCompleted) yield return null;
			}
		}

		public void DialogueComplete() => dialogueCompleted = true;
	}
}