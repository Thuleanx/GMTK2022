using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Thuleanx.FX {
	/// <summary>
	/// To be used alongside the snear shader
	/// </summary>
	[RequireComponent(typeof(Renderer))]
	public class SmearDataUpdate : MonoBehaviour {
		public string PrevPositionPropertyName = "_PrevPosition";

		public Renderer Rend {get; private set;}
		public Material Mat => Rend.material; // ehhhh not always true but ok

		[SerializeField, Tooltip("How many frames does the smear lag behind")] 
		int frameLag = 5;
		[SerializeField]
		bool DefaultEnabled;

		void Awake() {
			Rend = GetComponent<Renderer>();
			Enabled = DefaultEnabled;
		}

		public bool Enabled {
			get {
				if (Mat == null) return false;
				return Mat.GetFloat("_Enabled") == 1;
			}
			set => Mat?.SetFloat("_Enabled", value ? 1 : 0);
		}

		Queue<Vector3> positions = new Queue<Vector3>();

		void LateUpdate() {
			Vector3 pos = transform.position;
			if (positions.Count > frameLag)
				pos = positions.Dequeue();
			else if (positions.Count > 0) 
				pos = positions.Peek();

			if (Mat.HasProperty(PrevPositionPropertyName))
					Mat?.SetVector(PrevPositionPropertyName, pos);
			positions.Enqueue(transform.position);
		}
	}
}