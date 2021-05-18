using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Yorozu.EditorTool.SceneDrawer
{
	/// <summary>
	/// Graphic の頂点カラーとUVを表示
	/// </summary>
	internal static class EditorGraphicVertexDrawer
	{
		private const string MENU_PATH = SceneDrawerUtility.TOOL_PATH + "Show Graphic Vertex";

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

		private static List<UIVertex> _vertexList = new List<UIVertex>();
		private static Dictionary<Vector3, List<int>> _dic = new Dictionary<Vector3, List<int>>();

		private static int _cacheId;
		private static VertexHelper _cacheHelper;

		private static class Cache
		{
			public static GUIStyle Style;
			public static FieldInfo FieldInfo;

			static Cache()
			{
				Style = new GUIStyle(GUI.skin.window);

				Style.padding.bottom -= 20;
				Style.padding.left -= 7;
				Style.padding.right -= 7;
				Style.margin = new RectOffset();
				Style.richText = true;

				FieldInfo = typeof(Graphic).GetField("s_VertexHelper", BindingFlags.NonPublic | BindingFlags.Static);
			}
		}

		[DrawGizmo(GizmoType.Selected)]
		private static void DrawGizmo(Graphic graphic, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			if (_cacheId != graphic.GetInstanceID())
			{
				_cacheHelper = Cache.FieldInfo.GetValue(graphic) as VertexHelper;
				if (_cacheHelper == null)
					return;

				_cacheId = graphic.GetInstanceID();
				graphic.SetVerticesDirty();
			}

			_cacheHelper.GetUIVertexStream(_vertexList);

			_dic.Clear();
			for (var index = 0; index < _vertexList.Count; index++)
			{
				var pos = _vertexList[index].position;
				if (!_dic.ContainsKey(pos))
					_dic.Add(pos, new List<int>());

				_dic[pos].Add(index);
			}

			using (new HandlesMatrixScope(graphic.transform))
			{
				foreach (var pair in _dic)
				{
					var uv = _vertexList[pair.Value.First()].uv0;
					var color = ColorUtility.ToHtmlStringRGB(_vertexList[pair.Value.First()].color);
					Handles.Label(pair.Key, $"<color=#{color}>uv:{uv}</color>", Cache.Style);
				}
			}
		}
	}
}
