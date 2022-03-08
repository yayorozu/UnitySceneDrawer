using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Yorozu.EditorTool.SceneDrawer
{
    internal static class EditorGraphicSelectDrawer
    {
		private const string MENU_PATH = SceneDrawerUtility.TOOL_PATH + "Show Graphic Object";
		private static HashSet<Graphic> _hashSet;

		static EditorGraphicSelectDrawer()
		{
			SceneView.duringSceneGui += SceneGUI;
			_hashSet = new HashSet<Graphic>();
			SceneManager.activeSceneChanged += (arg0, scene) =>
			{
				_hashSet.Clear();
			};
		}

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

		[DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
		private static void DrawGizmo(Graphic graphic, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			if (!_hashSet.Contains(graphic))
			{
				_hashSet.Add(graphic);
			}
		}
		
		private static float? ribbonHeight;
		
		private static void SceneGUI(SceneView sceneView)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;
			
			if (_hashSet.Count <= 0)
				return;
			
			if (!ribbonHeight.HasValue)
			{
				var toolbar = new GUIStyle("GV Gizmo DropDown").CalcSize(sceneView.titleContent);
				ribbonHeight = toolbar.y;
			}
			
			var windowSize = sceneView.position.size;
			windowSize.y -= ribbonHeight.Value;

			var guiContent = new GUIContent();
			Handles.BeginGUI();
			foreach (var graphic in _hashSet)
			{
				if (graphic == null)
				{
					continue;
				}
				guiContent.text = graphic.transform.name;
				var size = GUI.skin.button.CalcSize(guiContent);
				
				var pointInView = sceneView.camera.WorldToViewportPoint(graphic.transform.position);
				var pointInSceneView = pointInView * windowSize;
				var screenPoint = pointInSceneView;
				screenPoint.y = sceneView.position.height - screenPoint.y;
				var rect = new Rect(screenPoint.x - size.x / 2f, screenPoint.y - size.y / 2f, size.x, size.y);
				if (GUI.Button(rect, graphic.transform.name))
				{
					Selection.activeGameObject = graphic.gameObject;
				}
			}
			Handles.EndGUI();
		}

    }
}
