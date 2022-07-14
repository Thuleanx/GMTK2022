using UnityEngine;
using Thuleanx.Utils;

namespace Thuleanx.Input {
	public abstract class MonoMiddleware : MonoBehaviour, InputMiddleware {
		public Optional<InputProvider> Provider;

		public void AttachTo(InputProvider provider) => provider.AddMiddleware(this, GetPriority());
		public void DetachFrom(InputProvider provider) => provider.RemoveMiddleware(this, GetPriority());
		public abstract int GetPriority();
		public abstract InputState Process(InputState state);

		protected void OnEnable() {
			if (Provider.Enabled) AttachTo(Provider.Value);
		}
		protected void OnDisable() {
			if (Provider.Enabled) DetachFrom(Provider.Value);
		}
	}
}