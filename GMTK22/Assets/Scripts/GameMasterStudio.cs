using UnityEngine;
using System.Collections;
using WizOsu.Animation;
using WizOsu.Patterns;
using NaughtyAttributes;

namespace WizOsu {
	public class GameMasterStudio : Singleton<GameMasterStudio> {
		[SerializeField, Required] GameObject wizard;

		void Start() {
		}
	}
}