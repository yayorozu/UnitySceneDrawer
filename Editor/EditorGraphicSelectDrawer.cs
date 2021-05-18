using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Yorozu.EditorTool.SceneDrawer
{
    internal static class EditorGraphicSelectDrawer
    {
		private const string MENU_PATH = SceneDrawerUtility.TOOL_PATH + "Show Graphic Object";

		[MenuItem(MENU_PATH)]
		private static void Menu()
		{
			EditorPrefs.SetBool(MENU_PATH, !EditorPrefs.GetBool(MENU_PATH, false));
		}

		[MenuItem(MENU_PATH, true)]
		private static bool MenuValidate()
		{
			UnityEditor.Menu.SetChecked(MENU_PATH, EditorPrefs.GetBool(MENU_PATH, false));
			return true;
		}

		private static class Styles
		{
			internal static GUIStyle Label;

			static Styles()
			{
				Label = new GUIStyle(GUI.skin.button);
				Label.padding = new RectOffset(0, 0, 0, 0);
			}
		}

		[DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
		private static void DrawGizmo(Graphic graphic, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			Handles.Label(graphic.transform.position, graphic.transform.name, Styles.Label);
		}
    }
}
