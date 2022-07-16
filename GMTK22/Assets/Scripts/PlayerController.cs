using UnityEngine;
using NaughtyAttributes;
using WizOsu.InputSystem;

namespace WizOsu {
	[RequireComponent(typeof(CharacterController))]
	public class PlayerController : MonoBehaviour {
		public CharacterController Controller { get; private set; }

		[SerializeField, Range(1f, 10f)] private float moveSpeed;
		public Vector3 Velocity { get; private set; }

		InputManager input => InputManager.instance;

		void Awake() {
			Controller = GetComponent<CharacterController>();
		}

		void Update() {
			Vector3 inputDir = new Vector3(
				input.Movement.x,
				0,
				input.Movement.y
			).normalized;

			if (inputDir.magnitude >= .1f) {

				// TODO: add camera reference to localInstance / localApp that can be retrieved. Then retrieve it here
				Vector3 moveDir = Quaternion.Euler(
					0, Camera.main.transform.eulerAngles.y, 0f) * inputDir;

				Velocity = moveDir * moveSpeed;

			} else Velocity = Vector3.zero;


			Controller.Move(Velocity * Time.deltaTime);
		}
	}
}