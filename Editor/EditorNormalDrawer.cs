using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool.SceneDrawer
{
	/// <summary>
	/// MeshFilter, SkinnedMeshRenderer の法線を表示
	/// </summary>
	internal static class EditorNormalDrawer
	{
		private const string MENU_PATH = SceneDrawerUtility.TOOL_PATH + "Show Mesh Normal";

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

			using (new GizmoMatrixScope(transform))
			{
				using (new GizmoColorScope(Color.red))
				{
					for (var i = 0; i < mesh.vertices.Length; i++)
					{
						var pos = mesh.vertices[i];
						var to = pos + mesh.normals[i].normalized;

						Gizmos.DrawLine(pos, to);
					}
				}
			}
		}

	}
}
