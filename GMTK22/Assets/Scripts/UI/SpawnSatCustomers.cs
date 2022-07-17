using UnityEngine;
using WizOsu;

namespace WizOsu.UI {
	public class SpawnSatCustomers : MonoBehaviour {
		public GameObject icon;
		void Start() {
			for (int i = 0; i < GameMasterStudio.satisfiedCustomersCnt; i++) {
				Instantiate(icon, transform);
			}
		}
	}
}