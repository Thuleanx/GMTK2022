using System;
using UnityEngine;
using System.Collections.Generic;

using Thuleanx.Patterns.ResChain;

namespace Thuleanx.NeoInput {
	public interface IInputProvider {
		IInputState GetBlank();
		IInputState GetState();
	}

	public interface IInputState {
	}

	public abstract class ChainInputProvider<T> : ScriptableChain<T>, IInputProvider 
		where T : IInputState, new() {

		public IInputState GetState() => Assemble((T) GetBlank());
		public IInputState GetBlank() => new T();
	}

	public interface InputMiddleware<T> : IProgram<T> where T : IInputState {
	}
}
