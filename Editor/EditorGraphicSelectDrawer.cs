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

		private static GUIContent content = new GUIContent();

		[DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
		private static void DrawGizmo(Graphic graphic, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			var sceneView = SceneView.currentDrawingSceneView;
			if (sceneView == null)
				return;

			var camera = sceneView.camera;

			content.text = graphic.transform.name;
			var size = GUI.skin.button.CalcSize(content);
			var screenPoint = camera.WorldToScreenPoint(graphic.transform.position);
			var buttonRect = new Rect(
				screenPoint.x - size.x / 2f,
				sceneView.position.height - screenPoint.y - size.y / 2f,
				size.x,
				size.y);

			Handles.BeginGUI();
			GUI.Button(buttonRect, content.text);
			Handles.EndGUI();
		}
    }
}
