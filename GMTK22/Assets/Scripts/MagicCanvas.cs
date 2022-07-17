using UnityEngine;
using UnityEngine.Rendering;
using TNTC.Painting;
using Thuleanx.Utils;

namespace WizOsu.Painting {
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(Paintable))]
	public class MagicCanvas : MonoBehaviour {
		const int TEXTURE_SIZE = 1024;

		public Renderer Renderer { get; private set;  }
		public Paintable Paintable { get; private set; }

		Material rendMat => Renderer.material;

		RenderTexture canvasTexture;
		CommandBuffer command;

		void Awake() {
			Renderer = GetComponent<Renderer>();
			Paintable = GetComponent<Paintable>();
			command = new CommandBuffer();
		}

		void OnEnable() {
			canvasTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
			canvasTexture.filterMode = FilterMode.Bilinear;
		}

		public Texture2D GetTexture() {
			command.SetRenderTarget(canvasTexture);
			command.Blit(rendMat.GetTexture("_MainTex"), canvasTexture, rendMat);
			Graphics.ExecuteCommandBuffer(command);
			command.Clear();
			return canvasTexture.toTexture2D();
		}

		void OnDisable() {
			canvasTexture.Release();
		}
	}
}