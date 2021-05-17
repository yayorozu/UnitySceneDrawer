using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool.SceneDrawer
{
	/// <summary>
	/// Root Canvas 以下の RectTransform の Rect を描画する
	/// </summary>
	internal static class EditorCanvasRectTransformDrawer
	{
		private const string MENU_PATH = SceneDrawerUtility.TOOL_PATH + "Show Canvas RectTransform Rect";

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

		private static readonly Color parentColor = new Color(0f, 1f, 0f, 0.05f);
		private static readonly Color childColor = new Color(1f, 1f, 0f, 0.2f);
		private static readonly Color wireColor = new Color(0f, 0f, 0f, 0.5f);

		[DrawGizmo (GizmoType.Selected)]
		private static void DrawGizmo(RectTransform obj, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			var rectTransform = obj.transform as RectTransform;
			if (rectTransform == null)
				return;

			var canvases = rectTransform.GetComponentsInParent<Canvas>();
			if (canvases == null || canvases.Length <= 0)
				return;

			var rootCanvas = canvases.First().rootCanvas;
			DrawRecursive(rootCanvas.transform, obj.transform);
		}

		private static void DrawRecursive(Transform parent, Transform ignore, bool isChild = false)
		{
			Draw(parent.transform as RectTransform, isChild);

			foreach (Transform child in parent)
				DrawRecursive(child, ignore, child == ignore || isChild);
		}

		private static void Draw(RectTransform rectTransform, bool isChild)
		{
			if (!rectTransform.gameObject.activeSelf)
				return;
			
			using (new GizmoMatrixScope(rectTransform.transform))
			{
				// pivot に応じてずらす
				var x = rectTransform.rect.width;
				var y = rectTransform.rect.height;
				var position = new Vector3(
					Mathf.Lerp(x / 2f, -x / 2f, rectTransform.pivot.x),
					Mathf.Lerp(y / 2f, -y / 2f, rectTransform.pivot.y),
					0f
				);
				using (new GizmoColorScope(isChild ? childColor : parentColor))
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
