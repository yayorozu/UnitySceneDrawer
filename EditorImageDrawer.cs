using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Yorozu.EditorTool
{
	public class EditorImageDrawer
	{
		private const string MENU_PATH = "Tools/Scene Drawer/Show Image Info";

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

		[DrawGizmo(GizmoType.Selected)]
		private static void DrawGizmo(Image image, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			if (_cacheId != image.GetInstanceID())
			{
				_cacheHelper = Cache.FieldInfo.GetValue(image) as VertexHelper;
				if (_cacheHelper == null)
					return;

				_cacheId = image.GetInstanceID();
				image.SetVerticesDirty();
			}

			_cacheHelper.GetUIVertexStream(_vertexList);

			_dic.Clear();
			for (var index = 0; index < _vertexList.Count; index++)
			{
				var pos = image.transform.position + image.transform.rotation * _vertexList[index].position;
				if (!_dic.ContainsKey(pos))
					_dic.Add(pos, new List<int>());

				_dic[pos].Add(index);
			}

			foreach (var pair in _dic)
			{
				var uv = _vertexList[pair.Value.First()].uv0;
				var color = ColorUtility.ToHtmlStringRGB(_vertexList[pair.Value.First()].color);
				Handles.Label(pair.Key, $"<color=#{color}>uv:{uv}</color>", Cache.Style);
			}
		}
	}
}
