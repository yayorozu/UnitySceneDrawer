using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool
{
	public class EditorCanvasRectDrawer 
	{
		private const string MENU_PATH = "Tools/Scene Drawer/Show Canvas OverDraw";

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
		
		private static readonly Color parentColor = new Color(0f, 1.0f, 0f, 0.05f);
		private static readonly Color childColor = new Color(1f, 0f, 0f, 0.2f);
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

			Canvas rootCanvas = null;
			foreach (var canvas in canvases)
			{
				if (canvas.rootCanvas == null || canvas == canvas.rootCanvas)
				{
					rootCanvas = canvas;
					break;
				}
			}

			if (rootCanvas == null)
				return;

			Recursive(rootCanvas.transform, obj.transform);
		}

		private static void Recursive(Transform parent, Transform ignore, bool isChild = false)
		{
			Draw(parent.transform as RectTransform, isChild);
			foreach (Transform child in parent)
				Recursive(child, ignore, child == ignore || isChild);
		}

		private static void Draw(RectTransform rectTransform, bool isChild)
		{
			Vector2 size = rectTransform.rect.size;
			size.x *= rectTransform.lossyScale.x;
			size.y *= rectTransform.lossyScale.y;
			var rect = new Rect
			{
				center = (Vector2) rectTransform.position - new Vector2(
					         rectTransform.pivot.x * size.x, 
					         rectTransform.pivot.y * size.y),
				size = size,
			};

			Gizmos.color = isChild ? childColor : parentColor;
			Gizmos.DrawCube(rect.center, rect.size);
			Gizmos.color = wireColor;
			Gizmos.DrawWireCube(rect.center, rect.size);
		}
	}
}
