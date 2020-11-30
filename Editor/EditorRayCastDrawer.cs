using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Yorozu.EditorTool
{
	public class EditorRayCastDrawer 
	{
		private const string MENU_PATH = "Tools/Scene Drawer/Show graphic Raycast Overdraw";

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
			
		private static readonly Color Color = new Color(1f, 0f, 0f, 0.1f);
		private static readonly Color wireColor = new Color(0f, 0f, 0f, 0.5f);
			
		[DrawGizmo (GizmoType.NonSelected | GizmoType.Selected)]
		private static void DrawGizmo(Graphic graphic, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;
				
			if (!graphic.raycastTarget)
				return;

			var rectTransform = graphic.transform as RectTransform;
			var size = rectTransform.rect.size;
			size.x *= rectTransform.lossyScale.x;
			size.y *= rectTransform.lossyScale.y;
			var rect = new Rect
			{
				center = (Vector2) rectTransform.position - new Vector2(rectTransform.pivot.x * size.x, rectTransform.pivot.y * size.y),
				size = size,
			};

			Gizmos.color = Color;
			Gizmos.DrawCube(rect.center, rect.size);
			Gizmos.color = wireColor;
			Gizmos.DrawWireCube(rect.center, rect.size);
		}
	}
}
