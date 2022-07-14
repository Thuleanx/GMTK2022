using UnityEngine;

namespace Thuleanx.Input {
	public interface InputProvider {
		InputState GetState();
		InputState BlankState();
		void BroadcastEvents(InputState resultState);

		void AddMiddleware(InputMiddleware middleware, int priority);
		void RemoveMiddleware(InputMiddleware middleware, int priority);
	}
}