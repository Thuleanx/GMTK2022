using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections.Generic;
using System.Collections;
using WizOsu.Animation;
using WizOsu.Patterns;
using WizOsu.Painting;
using NaughtyAttributes;
using WizOsu.Behaviour;
using TNTC.Painting;
using Thuleanx.FX.Particles;
using Thuleanx.Utils;
using Yarn.Unity;
using Thuleanx;
using Thuleanx.Audio;

namespace WizOsu {
	public class GameMasterStudio : Singleton<GameMasterStudio> {
		public static int satisfiedCustomersCnt = 7;

		[Header("References")]
		[SerializeField, Required] GameObject wizard;
		[SerializeField, Required] GameObject npc;
		[SerializeField, Required] MagicCanvas drawingCanvas;
		[SerializeField, Required] Renderer referenceCanvas;
		[SerializeField, Required] DialogueRunner dialogueRunner;
		[SerializeField, Required] InMemoryVariableStorage storage;
		[SerializeField, Required] ParticlePaintOnHit brush;
		[SerializeField, Required] GameObject satisfiedCustomerIcon;
		[SerializeField, Required] GameObject satisfiedCustomerArea;
		[SerializeField, Required] FMODUnity.StudioEventEmitter MagicPaintSFX;
		[SerializeField, Required] FMODUnity.StudioEventEmitter SpeakingSFX;
		[SerializeField] ParticleCombo wandParticles;
		[SerializeField] string paintinOrderSource = "/Paintings/";
		[SerializeField] SceneReference nextScene;
		[SerializeField] List<PaintingOrder> PossibleOrders;

		#region Positional Anchors
		[Header("Anchor Positions")]
		[HorizontalLine(color: EColor.Red)]
		[SerializeField] Transform npcEntrancePos;
		[SerializeField] Transform npcDestinationPos;
		#endregion

		[Header("Sound")]
		[SerializeField] FMODUnity.EventReference SFX_MagicPaintSwitch;
		[SerializeField] FMODUnity.EventReference SFX_EraseDrawingCanvas;
		[SerializeField] FMODUnity.EventReference SFX_PutUpReference;

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

		public override void Awake() {
			base.Awake();
			if (PossibleOrders == null || PossibleOrders.Count == 0)
				PossibleOrders = new List<PaintingOrder>(Resources.LoadAll<PaintingOrder>(paintinOrderSource));
		}

		void Start() {
			StartCoroutine(Sequence_MainGameLoop());
		}

		public IEnumerator IntroSequence() {
			yield return WaitForDialogue(introNode);
		}

		public IEnumerator Sequence_MainGameLoop() {
			DisableAiming(wizard.gameObject);
			bool waiting = true;
			TransitionManager.instance.FadeIn(() => waiting = false);
			while (waiting) yield return null;
			if (PlayIntro) yield return IntroSequence();
			while (Reputation < ReputationRequired)
				yield return Sequence_PaintingOrder(PossibleOrders[Mathx.RandomRange(0, PossibleOrders.Count)]);
			yield return Sequence_Squire();
			waiting = true;
			TransitionManager.instance.FadeOut(() => waiting = false);
			while (waiting) yield return null;
			storage.SetValue("$satCustomerCnt", satisfiedCustomersCnt);
			App.Instance.RequestLoad(nextScene.SceneName);
		}

		public static void EnableAniming(GameObject jobj) {
			foreach (var animator in jobj.GetComponentsInChildren<Animator>())
				animator.SetInteger("State", 1);
			foreach (var obj in jobj.GetComponentsInChildren<AimAtMouse>())
				obj.enabled = true;
			foreach (var constraint in jobj.GetComponentsInChildren<MultiAimConstraint>()) {
				if (constraint.gameObject.name.Contains("Arm")) {
					var sources = constraint.data.sourceObjects;
					sources.SetWeight(0, 1);
					constraint.data.sourceObjects = sources;
				} else {
					var sources = constraint.data.sourceObjects;
					sources.SetWeight(0, .5f);
					constraint.data.sourceObjects = sources;
				}
			}
		}

		public static void DisableAiming(GameObject gobj) {
			foreach (var animator in gobj.GetComponentsInChildren<Animator>())
				animator.SetInteger("State", 0);
			foreach (var obj in gobj.GetComponentsInChildren<AimAtMouse>())
				obj.enabled =false;
			foreach (var constraint in gobj.GetComponentsInChildren<MultiAimConstraint>()) {
				var sources = constraint.data.sourceObjects;
				sources.SetWeight(0, 0);
				constraint.data.sourceObjects = sources;
			}
		}

		public IEnumerator Sequence_PaintingOrder(PaintingOrder order) {
			npc.GetComponentInChildren<SpriteRenderer>().flipX = false;
			npc.GetComponentInChildren<SpriteRenderer>().sprite = order.npcSprite;
			yield return AnimationManager.instance?.DoBunnyHop(npc.transform, npcDestinationPos.position);
			drawingCanvas.Paintable.ResetTextures();
			drawingCanvas.Paintable.getRenderer().material.SetTexture("_MainTex", order.lineart);
			AudioManager.Instance?.PlayOneShot(SFX_EraseDrawingCanvas);
			yield return new WaitForSeconds(0.5f);
			referenceCanvas.material.SetTexture("_MainTex", order.referenceImage);
			AudioManager.Instance?.PlayOneShot(SFX_PutUpReference);
			yield return new WaitForSeconds(0.5f);

			yield return WaitForDialogue(order.requestNode);

			npc.GetComponentInChildren<SpriteRenderer>().flipX = true;
			yield return AnimationManager.instance?.DoBunnyHop(npc.transform, npcEntrancePos.position);

			wandParticles?.Activate();
			MagicPaintSFX?.Play();

			Color[] paletteColors = order.palette.GetPixels();

			Timer workingOnOrder = order.durationSeconds;
			Timer colorChangeCD = 0;

			EnableAniming(wizard.gameObject);
			while (workingOnOrder) {
				if (!colorChangeCD) {
					Color nxtCol = paletteColors[Mathx.RandomRange(0, paletteColors.Length)];
					brush.paintColor = nxtCol;
					brush.GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", nxtCol);
					colorChangeCD = Mathx.RandomRange(colorChangeRange);
					AudioManager.Instance?.PlayOneShot(SFX_MagicPaintSwitch);
				}
				yield return null;
			}
			DisableAiming(wizard.gameObject);

			wandParticles?.StopProducing();
			MagicPaintSFX?.Stop();

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

			npc.GetComponentInChildren<SpriteRenderer>().flipX = true;
			yield return AnimationManager.instance?.DoBunnyHop(npc.transform, npcEntrancePos.position);

			bool satisfied = score < order.thresholds.y;
			satisfiedCustomersCnt += satisfied?1:0;

			if (satisfied) Instantiate(satisfiedCustomerIcon, satisfiedCustomerArea.transform);

			yield return new WaitForSeconds(3f);
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