#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
[UnityEngine.AddComponentMenu("Wwise/AkSurfaceReflector")]
[UnityEngine.DisallowMultipleComponent]
///@brief This component will convert the triangles of the GameObject's geometry into sound reflective surfaces.
///@details This component requires a Mesh Filter component. The triangles of the mesh will be sent to the Spatial Audio wrapper by calling SpatialAudio::AddGeometrySet(). The triangles will reflect the sound emitted from AkSpatialAudioEmitter components.
[UnityEngine.RequireComponent(typeof(UnityEngine.MeshFilter))]
public class AkSurfaceReflector : UnityEngine.MonoBehaviour
{
	[UnityEngine.Tooltip("All triangles of the component's mesh will be applied with this texture. The texture will change the filter parameters of the sound reflected from this component.")]
	/// All triangles of the component's mesh will be applied with this texture. The texture will change the filter parameters of the sound reflected from this component.
	public AK.Wwise.AcousticTexture AcousticTexture;

	[UnityEngine.Header("Geometric Diffraction (Experimental)")]
	[UnityEngine.Tooltip("Enable or disable geometric diffraction for this mesh.")]
	/// Switch to enable or disable geometric diffraction for this mesh.
	public bool EnableDiffraction = false;

	[UnityEngine.Tooltip("Enable or disable geometric diffraction on boundary edges for this mesh.  Boundary edges are edges that are connected to only one triangle.")]
	/// Switch to enable or disable geometric diffraction on boundary edges for this mesh.  Boundary edges are edges that are connected to only one triangle.
	public bool EnableDiffractionOnBoundaryEdges = false;

	private UnityEngine.MeshFilter MeshFilter;

	public static ulong GetAkGeometrySetID(UnityEngine.MeshFilter meshFilter)
	{
		return (ulong)meshFilter.GetInstanceID();
	}

	/// <summary>
	///     Sends the mesh filter's triangles and their acoustic texture to Spatial Audio
	/// </summary>
	/// <param name="acousticTexture"></param>
	/// <param name="meshFilter"></param>
	public static void AddGeometrySet(AK.Wwise.AcousticTexture acousticTexture, UnityEngine.MeshFilter meshFilter, bool enableDiffraction, bool enableDiffractionOnBoundaryEdges)
	{
		if (!AkSoundEngine.IsInitialized())
			return;

		if (meshFilter == null)
			UnityEngine.Debug.Log("AddGeometrySet(): No mesh found!");
		else
		{
			var mesh = meshFilter.sharedMesh;
			var vertices = mesh.vertices;
			var triangles = mesh.triangles;

			// Remove duplicate vertices
			var vertRemap = new int[vertices.Length];
			var uniqueVerts = new System.Collections.Generic.List<UnityEngine.Vector3>();
			var vertDict = new System.Collections.Generic.Dictionary<UnityEngine.Vector3, int>();

			for (var v = 0; v < vertices.Length; ++v)
			{
				int vertIdx = 0;
				if (!vertDict.TryGetValue(vertices[v], out vertIdx))
				{
					vertIdx = uniqueVerts.Count;
					uniqueVerts.Add(vertices[v]);
					vertDict.Add(vertices[v], vertIdx);
					vertRemap[v] = vertIdx;
				}
				else
				{
					vertRemap[v] = vertIdx;
				}
			}

			int vertexCount = uniqueVerts.Count;

			using (var surfaceArray = new AkAcousticSurfaceArray(1))
			{
				var surface = surfaceArray[0];
				surface.textureID = acousticTexture.Id;
				surface.reflectorChannelMask = unchecked((uint)-1);
				surface.strName = meshFilter.gameObject.name;

				using (var vertexArray = new AkVertexArray(vertexCount))
				{
					for (var v = 0; v < vertexCount; ++v)
					{
						var point = meshFilter.transform.TransformPoint(uniqueVerts[v]);
						using (var akVert = vertexArray[v])
						{
							akVert.X = point.x;
							akVert.Y = point.y;
							akVert.Z = point.z;
						}
					}
					
					var numTriangles = mesh.triangles.Length / 3;
					using (var triangleArray = new AkTriangleArray(numTriangles))
					{
						for (var i = 0; i < numTriangles; ++i)
						{
							using (var triangle = triangleArray[i])
							{
								triangle.point0 = (ushort)vertRemap[triangles[3 * i + 0]];
								triangle.point1 = (ushort)vertRemap[triangles[3 * i + 1]];
								triangle.point2 = (ushort)vertRemap[triangles[3 * i + 2]];
								triangle.surface = (ushort)0;
							}
						}

					   
						AkSoundEngine.SetGeometry(GetAkGeometrySetID(meshFilter), triangleArray, (uint)triangleArray.Count(), vertexArray, (uint)vertexArray.Count(), surfaceArray, (uint)surfaceArray.Count(), enableDiffraction, enableDiffractionOnBoundaryEdges);
					}
				}
			}
		}
	}

	/// <summary>
	///     Remove the corresponding mesh filter's geometry from Spatial Audio.
	/// </summary>
	/// <param name="meshFilter"></param>
	public static void RemoveGeometrySet(UnityEngine.MeshFilter meshFilter)
	{
		if (meshFilter != null)
			AkSoundEngine.RemoveGeometry(GetAkGeometrySetID(meshFilter));
	}

	private void Awake()
	{
		MeshFilter = GetComponent<UnityEngine.MeshFilter>();
	}

	private void OnEnable()
	{
		AddGeometrySet(AcousticTexture, MeshFilter, EnableDiffraction, EnableDiffractionOnBoundaryEdges);
	}

	private void OnDisable()
	{
		RemoveGeometrySet(MeshFilter);
	}


#if UNITY_EDITOR
	[UnityEditor.CustomEditor(typeof(AkSurfaceReflector))]
	[UnityEditor.CanEditMultipleObjects]
	private class Editor : UnityEditor.Editor
	{
		private UnityEditor.SerializedProperty AcousticTexture;
		private UnityEditor.SerializedProperty EnableDiffraction;
		private UnityEditor.SerializedProperty EnableDiffractionOnBoundaryEdges;

		public void OnEnable()
		{
			AcousticTexture = serializedObject.FindProperty("AcousticTexture");
			EnableDiffraction = serializedObject.FindProperty("EnableDiffraction");
			EnableDiffractionOnBoundaryEdges = serializedObject.FindProperty("EnableDiffractionOnBoundaryEdges");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			UnityEditor.EditorGUILayout.PropertyField(AcousticTexture);

			UnityEditor.EditorGUILayout.PropertyField(EnableDiffraction);
			if (EnableDiffraction.boolValue)
				UnityEditor.EditorGUILayout.PropertyField(EnableDiffractionOnBoundaryEdges);

			serializedObject.ApplyModifiedProperties();
		}
	}
#endif

}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.