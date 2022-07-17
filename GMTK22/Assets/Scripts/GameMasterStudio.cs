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
using Thuleanx;

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
		[SerializeField] SceneReference nextScene;

		[SerializeField, ReadOnly] List<PaintingOrder> PossibleOrders;

		#region Positional Anchors
		[Header("Anchor Positions")]
		[HorizontalLine(color: EColor.Red)]
		[SerializeField] Transform npcEntrancePos;
		[SerializeField] Transform npcDestinationPos;
		#endregion


		[Header("Game Vars")]
		[HorizontalLine(color: EColor.Blue)]
		[ProgressBar("Reputation", 4f)]
		public float Reputation = 0;
		[Range(0, 7f)]
		public float ReputationRequired = 7f;
		[MinMaxSlider(0, 20f)] public Vector2 colorChangeRange = Vector2.one;
		[MinMaxSlider(0, 1f)] public Vector2 reputationReward = Vector2.one;

		[Header("Intro")]
		[HorizontalLine(color: EColor.Green)]
		public bool PlayIntro = false;
		[SerializeField] string introNode;

		[Header("Squire")]
		[HorizontalLine(color: EColor.Green)]
		[SerializeField] string squireNode;
		[SerializeField] Sprite squireSprite;
		[SerializeField] string clarisNode;

		bool dialogueCompleted;
		int satisfiedCustomersCnt = 0;

		public override void Awake() {
			base.Awake();
			PossibleOrders = new List<PaintingOrder>(Resources.LoadAll<PaintingOrder>(paintinOrderSource));
		}

		void Start() {
			StartCoroutine(Sequence_MainGameLoop());
		}

		public IEnumerator IntroSequence() {
			yield return WaitForDialogue(introNode);
		}

		public IEnumerator Sequence_MainGameLoop() {
			bool waiting = true;
			TransitionManager.instance.FadeIn(() => waiting = false);
			while (waiting) yield return null;
			if (PlayIntro) yield return IntroSequence();
			while (Reputation < ReputationRequired)
				yield return Sequence_PaintingOrder(PossibleOrders[Mathx.RandomRange(0, PossibleOrders.Count)]);
			yield return Sequence_Squire();
			waiting = true;
			TransitionManager.instance.Fadeout(() => waiting = false);
			while (waiting) yield return null;
			storage.SetValue("$satCustomerCnt", satisfiedCustomersCnt);
			App.Instance.RequestLoad(nextScene.SceneName);
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

			storage.SetValue("$threshold0", order.thresholds.x);
			storage.SetValue("$threshold1", order.thresholds.y);
			storage.SetValue("$score", score);

			npc.GetComponentInChildren<SpriteRenderer>().flipX = false;
			yield return AnimationManager.instance?.DoBunnyHop(npc.transform, npcDestinationPos.position);

			yield return WaitForDialogue(order.evalNode);

			float impressiveness = Mathf.InverseLerp(1 - order.thresholds.y, 1, 
				Mathf.Max(1 - score, 1 - order.thresholds.y));

			Reputation += Mathf.Lerp(reputationReward.x, reputationReward.y, EasingFunction.Linear(0, 1, impressiveness));

			satisfiedCustomersCnt += score < order.thresholds.y ? 1 : 0;

			npc.GetComponentInChildren<SpriteRenderer>().flipX = true;
			yield return AnimationManager.instance?.DoBunnyHop(npc.transform, npcEntrancePos.position);
		}

		public IEnumerator Sequence_Squire() {
			npc.GetComponentInChildren<SpriteRenderer>().flipX = false;
			npc.GetComponentInChildren<SpriteRenderer>().sprite = squireSprite;
			yield return AnimationManager.instance?.DoBunnyHop(npc.transform, npcDestinationPos.position);

			yield return WaitForDialogue(squireNode);

			npc.GetComponentInChildren<SpriteRenderer>().flipX = true;
			yield return AnimationManager.instance?.DoBunnyHop(npc.transform, npcEntrancePos.position);

			yield return WaitForDialogue(clarisNode);
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