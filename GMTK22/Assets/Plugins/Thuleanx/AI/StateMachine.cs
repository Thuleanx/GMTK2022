using UnityEngine;
using System;
using UnityEngine.Assertions;
using System.Collections;

namespace Thuleanx.AI {
	public class StateMachine : MonoBehaviour {
		bool constructed = false;
		int currentState;
		int defaultState;
		bool updateAuto;
		Coroutine activeCoroutine;
		bool[] StopCoroutineOnEnd;
		public Action[] Begins, Ends;
		public Func<int>[] Updates, Transitions, FixUpdates;
		public Func<IEnumerator>[] Coroutines;
		
		public int State {
			get { return currentState; }
			set {
				if (value != currentState) {
					Ends[currentState]?.Invoke();
					currentState = value;
					Begins[currentState]?.Invoke();
					if (Coroutines[currentState] != null)
						StartCoroutine(Coroutines[currentState]());
				}
			}
		}

		public void SetCallbacks(int state, Func<int> update, Func<int> fixUpdate, Func<int> transition, 
				Func<IEnumerator> coroutine, Action begin, Action end) {

			SetCallbackTransition(state, transition);
			SetCallbackUpdate(state, update);
			SetCallbackFixUpdate(state, fixUpdate);
			SetCallbackCoroutine(state, coroutine);
			SetCallbackBegin(state, begin);
			SetCallbackEnd(state, end);
		}

		public void SetCallbackTransition(int state, Func<int> transition)  => Transitions[state] = transition;
		public void SetCallbackUpdate(int state, Func<int> update)  		=> Updates[state] = update;
		public void SetCallbackFixUpdate(int state, Func<int> update) 		=> FixUpdates[state] = update;
		public void SetCallbackCoroutine(int state, Func<IEnumerator> coroutine, bool stopOnEnd = true) {
			Coroutines[state] = coroutine;
			StopCoroutineOnEnd[state] = stopOnEnd;
		}
		public void SetCallbackBegin(int state, Action begin)  				=> Begins[state] = begin;
		public void SetCallbackEnd(int state, Action end)  					=> Ends[state] = end;

		public void Init() {
			currentState = defaultState;
			Begins[currentState]?.Invoke();
		}

		public void Construct(int numbers, int defaultState, bool UpdateAuto = true) {
			Begins = new Action[numbers];
			Ends = new Action[numbers];
			Updates = new Func<int>[numbers];
			FixUpdates = new Func<int>[numbers];
			Transitions = new Func<int>[numbers];
			Coroutines = new Func<IEnumerator>[numbers];
			StopCoroutineOnEnd = new bool[numbers];
			this.defaultState = defaultState;
			constructed = true;
		}

		void OnEnable() {
			if (constructed) Init();
		} 
		void Update() {
			if (updateAuto) _Update();
		}
		public void FixedUpdate() {
			if (updateAuto) _FixUpdate();
		}
		public void _Update() {
			while (Transitions[State] != null) {
				int nxt = Transitions[State].Invoke();
				if (nxt == State || nxt == -1) break;
				State = nxt;
			}
			if (Updates[State] != null) {
				int nxt = Updates[State].Invoke();
				if (nxt != -1) State = nxt;
			}
		}
		public void _FixUpdate() {
			if (FixUpdates[State] != null) {
				int nxt = FixUpdates[State].Invoke();
				if (nxt != -1) State = nxt;
			}
		}
	}
}