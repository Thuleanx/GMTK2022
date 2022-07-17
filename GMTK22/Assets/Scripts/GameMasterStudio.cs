using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using WizOsu.Animation;
using WizOsu.Patterns;
using WizOsu.Painting;
using NaughtyAttributes;
using TNTC.Painting;
using Thuleanx.FX.Particles;
using Thuleanx.Utils;
using Yarn.Unity;

namespace WizOsu {
	public class GameMasterStudio : Singleton<GameMasterStudio> {
		[Header("References")]
		[SerializeField, Required] GameObject wizard;
		[SerializeField, Required] GameObject npc;
		[SerializeField, Required] MagicCanvas drawingCanvas;
		[SerializeField, Required] Renderer referenceCanvas;
		[SerializeField, Required] DialogueRunner dialogueRunner;
		[SerializeField, Required] InMemoryVariableStorage storage;
		[SerializeField, Required] ParticlePaintOnHit brush;
		[SerializeField] ParticleCombo wandParticles;
		[SerializeField] string paintinOrderSource = "/Paintings/";

		[SerializeField, ReadOnly] List<PaintingOrder> PossibleOrders;

		#region Positional Anchors
		[Header("Anchor Positions")]
		[HorizontalLine(color: EColor.Red)]
		[SerializeField] Transform npcEntrancePos;
		[SerializeField] Transform npcDestinationPos;
		#endregion


		[Header("Game Vars")]
		[ProgressBar("Reputation", 4f)]
		public float Reputation = 0;
		[Range(0, 7f)]
		public float ReputationRequired = 7f;
		[MinMaxSlider(0, 20f)] public Vector2 colorChangeRange = Vector2.one;
		[MinMaxSlider(0, 1f)] public Vector2 reputationReward = Vector2.one;

		bool dialogueCompleted;

		public override void Awake() {
			base.Awake();
			PossibleOrders = new List<PaintingOrder>(Resources.LoadAll<PaintingOrder>(paintinOrderSource));
		}

		void Start() {
			StartCoroutine(Sequence_MainGameLoop());
		}

		public IEnumerator IntroSequence() {
			npc.transform.position = npcEntrancePos.position;
			yield return AnimationManager.instance?.DoBunnyHop(npc.transform, npcDestinationPos.position);
		}

		public IEnumerator Sequence_MainGameLoop() {
			while (Reputation < ReputationRequired)
				yield return Sequence_PaintingOrder(PossibleOrders[Mathx.RandomRange(0, PossibleOrders.Count)]);
		}

		public IEnumerator Sequence_PaintingOrder(PaintingOrder order) {
			npc.GetComponentInChildren<SpriteRenderer>().flipX = false;
			npc.GetComponentInChildren<SpriteRenderer>().sprite = order.npcSprite;
			yield return AnimationManager.instance?.DoBunnyHop(npc.transform, npcDestinationPos.position);
			drawingCanvas.Paintable.ResetTextures();
			drawingCanvas.Paintable.getRenderer().material.SetTexture("_MainTex", order.lineart);
			referenceCanvas.material.SetTexture("_MainTex", order.referenceImage);

			yield return WaitForDialogue(order.requestNode);

			npc.GetComponentInChildren<SpriteRenderer>().flipX = true;
			yield return AnimationManager.instance?.DoBunnyHop(npc.transform, npcEntrancePos.position);

			wandParticles?.Activate();

			Color[] paletteColors = order.palette.GetPixels();

			Timer workingOnOrder = order.durationSeconds;
			Timer colorChangeCD = 0;
			while (workingOnOrder) {
				if (!colorChangeCD) {
					Color nxtCol = paletteColors[Mathx.RandomRange(0, paletteColors.Length)];
					brush.paintColor = nxtCol;
					brush.GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", nxtCol);
					colorChangeCD = Mathx.RandomRange(colorChangeRange);
				}
				yield return null;
			}

			wandParticles?.StopProducing();

			float score = TextureDiffUtils.SumSquareDifference(drawingCanvas.GetTexture(), 
				order.referenceImage);

			storage.SetValue("$threshold", order.successThreshold);
			storage.SetValue("$score", score);

			npc.GetComponentInChildren<SpriteRenderer>().flipX = false;
			yield return AnimationManager.instance?.DoBunnyHop(npc.transform, npcDestinationPos.position);

			yield return WaitForDialogue(order.evalNode);

			float impressiveness = Mathf.InverseLerp(1 - order.successThreshold, 1, 
				Mathf.Max(1 - score, 1 - order.successThreshold));

			Reputation += Mathf.Lerp(reputationReward.x, reputationReward.y, EasingFunction.Linear(0, 1, impressiveness));

			npc.GetComponentInChildren<SpriteRenderer>().flipX = true;
			yield return AnimationManager.instance?.DoBunnyHop(npc.transform, npcEntrancePos.position);
		}

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