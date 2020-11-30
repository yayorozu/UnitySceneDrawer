using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool
{
	public class EditorNormalDrawer
	{
		private const string MENU_PATH = "Tools/Scene Drawer/Show Mesh Normal";

		[MenuItem(MENU_PATH)]
		private static void MenuAction()
		{
			EditorPrefs.SetBool(MENU_PATH, !EditorPrefs.GetBool(MENU_PATH, false));
		}

		[MenuItem(MENU_PATH, true)]
		private static bool MenuValidate()
		{
			Menu.SetChecked(MENU_PATH, EditorPrefs.GetBool(MENU_PATH, false));
			return true;
		}

		private static Vector3 _cache;

		[DrawGizmo(GizmoType.Selected)]
		private static void DrawGizmo(MeshFilter meshFilter, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			if (meshFilter.sharedMesh == null)
				return;

			Draw(meshFilter.sharedMesh, meshFilter.transform);
		}

		[DrawGizmo(GizmoType.Selected)]
		private static void DrawGizmo(SkinnedMeshRenderer meshRenderer, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			if (meshRenderer.sharedMesh == null)
				return;

			Draw(meshRenderer.sharedMesh, meshRenderer.transform);
		}

		private static void Draw(Mesh mesh, Transform transform)
		{
			if (mesh.normals.Length != mesh.vertices.Length)
				return;

			var rotation = transform.rotation;
			var position = transform.position;
			var scale = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3;

			Gizmos.color = Color.red;
			for (var i = 0; i < mesh.vertices.Length; i++)
			{
				_cache = rotation * mesh.vertices[i];
				for (var j = 0; j < 3; j++)
					_cache[j] *= transform.localScale[j];

				Gizmos.DrawLine(position + _cache, position + _cache + mesh.normals[i] * scale / 10f);
			}
		}

	}
}
