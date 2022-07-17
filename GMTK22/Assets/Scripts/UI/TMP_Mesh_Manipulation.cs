using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMP_Mesh_Manipulation : MonoBehaviour
{
	protected TMP_Text textMesh;
	protected Mesh mesh;
	protected Vector3[] vertices;
	protected RectTransform rectTransform;

	public virtual void Awake()
	{
		textMesh = GetComponentInChildren<TMP_Text>();
		rectTransform = GetComponentInChildren<RectTransform>();
	}

	public virtual void OnEnable() {
		textMesh.ForceMeshUpdate();
		mesh = textMesh.mesh;
		vertices = mesh.vertices;
	}

	public virtual void Start() {
		textMesh.ForceMeshUpdate();
		mesh = textMesh.mesh;
		vertices = mesh.vertices;
	}

	public virtual void Update()
	{
		textMesh.ForceMeshUpdate();
		mesh = textMesh.mesh;
		vertices = mesh.vertices;
	}

	public void LateUpdate() {
		mesh.vertices = vertices;
		textMesh.canvasRenderer.SetMesh(mesh);
	}

	public virtual void SetText(string text) { 
		textMesh.text = text; 
	}
}