using UnityEngine;

namespace Thuleanx.Input {
	public interface InputMiddleware {
		int GetPriority();

		InputState Process(InputState state);
		void DetachFrom(InputProvider provider);
		void AttachTo(InputProvider provider);
	}
}