using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Yorozu.EditorTool.SceneDrawer
{
	/// <summary>
	/// Graphic の Raycast が有効なオブジェクトを表示
	/// </summary>
	internal static class EditorGraphicRayCastDrawer
	{
		private const string MENU_PATH = SceneDrawerUtility.TOOL_PATH + "Show Graphic Raycast Overdraw";

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

		private static readonly Color Color = new Color(1f, 0f, 0f, 0.1f);
		private static readonly Color wireColor = new Color(0f, 0f, 0f, 0.5f);

		[DrawGizmo (GizmoType.NonSelected | GizmoType.Selected)]
		private static void DrawGizmo(Graphic graphic, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			if (!graphic.raycastTarget || !graphic.enabled || !graphic.gameObject.activeSelf)
				return;

			using (new GizmoMatrixScope(graphic.transform))
			{
				// pivot に応じてずらす
				var x = graphic.rectTransform.rect.width;
				var y = graphic.rectTransform.rect.height;
				var position = new Vector3(
					Mathf.Lerp(x / 2f, -x / 2f, graphic.rectTransform.pivot.x),
					Mathf.Lerp(y / 2f, -y / 2f, graphic.rectTransform.pivot.y),
					0f
				);
				using (new GizmoColorScope(Color))
				{
					Gizmos.DrawCube(position, new Vector3(x, y, 0f));
				}

				using (new GizmoColorScope(wireColor))
				{
					Gizmos.DrawWireCube(position, new Vector3(x, y, 0f));
				}
			}
		}
	}
}
