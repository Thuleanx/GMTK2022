using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Thuleanx.FX {
	[RequireComponent(typeof(Camera))]
	public class PixelScreen : MonoBehaviour {
		public enum PixelScreenMode { Resize, Scale }

		[System.Serializable]
		public struct ScreenSize {
			public int width;
			public int height;
		}

		[Header("Screen scale settings")]
		public PixelScreenMode mode;
		public ScreenSize targetScreenSize = new ScreenSize { width = 256, height = 144 };
		public uint screenScaleFactor = 1;

		#region Components
		Camera renderCamera;
		#endregion

		RenderTexture renderTexture;
		int screenWidth, screenHeight;

		[Header("Display")]
		public RawImage display;

		void Start() {
			Init();
		}

		void Init() {
			if (!renderCamera) renderCamera = GetComponent<Camera>();

			screenWidth = Screen.width;
			screenHeight = Screen.height;

			if (screenScaleFactor < 1) screenScaleFactor = 1;
			if (targetScreenSize.width < 1) targetScreenSize.width = 1;
			if (targetScreenSize.height < 1) targetScreenSize.height = 1;

			int width = mode == PixelScreenMode.Resize ? (int)targetScreenSize.width : screenWidth / (int)screenScaleFactor;
			int height = mode == PixelScreenMode.Resize ? (int)targetScreenSize.height : screenHeight / (int)screenScaleFactor;

			renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.DefaultHDR) {
				filterMode = FilterMode.Point,
				antiAliasing = 1
			};

			renderCamera.targetTexture = renderTexture;
			display.texture = renderTexture;
		}

		void Update() {
			if (CheckResize() || CheckModeChange()) Init();
		}

		public bool CheckResize() => screenHeight != Screen.height || screenWidth != Screen.width;

		PixelScreenMode _lastMode;
		public bool CheckModeChange() {
			bool delta = _lastMode == mode;
			_lastMode = mode;
			return delta;
		}
	}

}