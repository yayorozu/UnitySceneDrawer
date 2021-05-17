using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool.SceneDrawer
{
	/// <summary>
	/// 選択してないカメラの描画範囲を描画
	/// </summary>
	internal static class EditorCameraViewDrawer
	{
		private const string MENU_PATH = SceneDrawerUtility.TOOL_PATH + "Show Camera View";

		[MenuItem(MENU_PATH)]
		static void MenuAction()
		{
			EditorPrefs.SetBool(MENU_PATH, !EditorPrefs.GetBool(MENU_PATH, false));
		}

		[MenuItem(MENU_PATH, true)]
		static bool MenuValidate()
		{
			Menu.SetChecked(MENU_PATH, EditorPrefs.GetBool(MENU_PATH, false));
			return true;
		}

		private static readonly Color color = new Color(0f, 1.0f, 0f, 1f);

		[DrawGizmo(GizmoType.NonSelected)]
		private static void DrawGizmo(Camera camera, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			var transform = camera.transform;

			var position = transform.position;
			var fov = camera.fieldOfView;
			var near = camera.nearClipPlane;
			var far = camera.farClipPlane;

			var matrix = Matrix4x4.TRS(position, transform.rotation, new Vector3(camera.aspect, 1f, 1f));
			using (new GizmoMatrixScope(matrix))
			{
				using (new GizmoColorScope(color))
				{
					if (camera.orthographic)
					{
						var size = camera.orthographicSize;
						Gizmos.DrawWireCube(
							new Vector3(0f, 0f, (far - near) / 2f + near),
							new Vector3(size * 2f, size * 2f,
							far - near)
						);
					}
					else
					{
						Gizmos.DrawFrustum(Vector3.zero, fov, near, far, 1f);
					}
				}
			}
		}
	}
}
