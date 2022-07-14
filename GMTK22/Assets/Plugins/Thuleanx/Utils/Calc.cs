using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Thuleanx.Utils {
	/// <summary>
	/// Class <c>Calc</c> contains general utility functions. Replacement for any <c>Math</c> functions
	/// </summary>
	public class Calc {
		static System.Random rand;
		public static System.Random Random {
			get {
				if (rand == null) rand = new System.Random();
				return rand;
			}
		}

		/// <summary>
		/// Not inclusive
		/// </summary>
		public static float RandomRange(float a, float b) 
			=> (float) Random.NextDouble() * (b-a) + a;

		/// <summary>
		/// Not inclusive
		/// </summary>
		public static int RandomRange(int a, int b)
			=> (int) (Random.Next(a,b));


		/// <summary>
		/// cur -> target by distance, but clamped
		/// </summary>
		public static float Approach(float cur, float target, float distance) {
			float amt = Mathf.Clamp(target - cur, -distance, distance);
			cur += amt;
			if (Mathf.Approximately(0, cur - target))
				cur = target;
			return cur;
		}

		/// <summary>
		/// Frame rate independent asymptotic smoothing with lerp
		/// </summary>
		public static float Damp(float a, float b, float lamda, float dt) {
			return Mathf.Lerp(a, b, 1 - Mathf.Exp(-lamda * dt));
		}

		/// <summary>
		/// Frame rate independent asymptotic smoothing with lerp
		/// </summary>
		public static Vector3 Damp(Vector3 a, Vector3 b, float lamda, float dt) {
			return Vector3.Lerp(a, b, 1 - Mathf.Exp(-lamda * dt));
		}

		public static bool Approximately(float a, float b, float threshold = 1e-9f) {
			Assert.IsTrue(threshold >= 0);
			return (a > b ? a - b : b - a) <= threshold;
		}

		public static float Closest(float origin, float a, float b) {
			return Mathf.Abs(origin - a) > Mathf.Abs(origin - b) ? a : b;
		}

		public static Vector2 Rotate(Vector2 root, float rad) {
			return new Vector2(
				root.x * Mathf.Cos(rad) - root.y * Mathf.Sin(rad),
				root.x * Mathf.Sin(rad) + root.y * Mathf.Cos(rad)
			);
		}

		/// <summary>
		/// Keep in range [0, 2pi]
		/// </summary>
		public static float NormalizeAngle(float radians) => Normalize(radians, 0, 2*Mathf.PI);

		public static float Normalize(float value, float start, float end)  {
			float width = end - start;
			float offset = value - start;
			return (offset - (Mathf.Floor(offset/width)*width))+start;
		}

		public static float Remap(float t, float a, float b, float c, float d) => (t - a) / (b - a) * (d - c) + c;

		public static float Remap_Clamp(float t, float a, float b, float c, float d) => Mathf.Clamp((t - a) / (b - a) * (d - c) + c, c, d);

		public static Quaternion ToQuat(Vector2 vec) => Quaternion.Euler(0, 0, -Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg);

		public static float HeightToVelocity(float height) => height > 0 ? Mathf.Sqrt(Mathf.Abs(2 * height * Physics2D.gravity.y)) : 0;

		public static LayerMask GetPhysicsLayerMask(int currentLayer) {
			int finalMask = 0;
			for (int i = 0; i < 32; i++)
				if (!Physics.GetIgnoreLayerCollision(currentLayer, i)) finalMask = finalMask | (1 << i);
			return finalMask;
		}

		public static Vector3 CameraBoundMove(Vector2 move) {
			Vector3 right = Camera.main.transform.right;
			Vector3 up = Camera.main.transform.forward;

			right.y = up.y = 0;
			if (right != Vector3.zero) right.Normalize();
			if (up != Vector3.zero) up.Normalize();
			return move.x * right + move.y * up;
		}
	}
}