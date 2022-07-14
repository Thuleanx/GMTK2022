using UnityEngine;
using Thuleanx.Utils;
using NaughtyAttributes;

namespace Thuleanx.FX {
	/// <summary>
	/// Wiggly tentacle. Theoretically usable in 2D
	/// </summary>
	[RequireComponent(typeof(LineRenderer))]
	public class Tentacle : MonoBehaviour {
		public LineRenderer LineRend {get; private set;}
		Vector3[] segmentPoses;
		Vector3[] segmentV;

		[Range(1, 30)] public int segmentCount = 4;
		public Optional<Transform> targetDir;
		public Optional<Transform> wiggleDir;
		public float length = 10f;
		public float smoothTime = .05f;
		public float trailSpeed = 350;
		public float smoothLambda = 9f;

		[HorizontalLine(color: EColor.Green)]
		[Header("Wiggle")]
		public float wiggleRotationPerSecond;
		public float wiggleMagnitude;
		float originalWiggleRot;
		float randomOffset;

		void Start() {
			if (!targetDir.Enabled) targetDir = new Optional<Transform>(transform);
			if (!wiggleDir.Enabled) wiggleDir = new Optional<Transform>(transform);
			LineRend = GetComponent<LineRenderer>();
			LineRend.positionCount = segmentCount;
			segmentPoses = new Vector3[segmentCount];
			segmentV = new Vector3[segmentCount];
			originalWiggleRot = wiggleDir.Value.localRotation.eulerAngles.z;
			randomOffset = Calc.RandomRange(0f, 2*Mathf.PI);
		}

		void Update() {
			segmentPoses[0] = targetDir.Value.position;
			wiggleDir.Value.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(Time.time*wiggleRotationPerSecond*2*Mathf.PI + randomOffset) * wiggleMagnitude + originalWiggleRot);

			for (int i = 1; i < segmentPoses.Length; i++) {
				segmentPoses[i] = 
				Vector3.SmoothDamp(
					segmentPoses[i], 
					segmentPoses[i-1] + targetDir.Value.right * length / segmentCount,
					ref segmentV[i], 
					smoothTime + i/trailSpeed
				);
			}

			LineRend.SetPositions(segmentPoses);
		}
	}
}