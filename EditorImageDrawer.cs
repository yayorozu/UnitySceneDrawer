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

		private static GUIStyle _style;
		private static List<UIVertex> _vertexList = new List<UIVertex>();
		private static FieldInfo _fieldInfo;

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

			if (_fieldInfo == null)
				_fieldInfo = typeof(Graphic).GetField("s_VertexHelper", BindingFlags.NonPublic | BindingFlags.Static);

			var helper = _fieldInfo.GetValue(image) as VertexHelper;
			if (helper == null)
				return;

			helper.GetUIVertexStream(_vertexList);

			if (_style == null)
			{
				_style = new GUIStyle(GUI.skin.window);

				_style.padding.bottom -= 20;
				_style.padding.left -= 7;
				_style.padding.right -= 7;
				_style.margin = new RectOffset();
				_style.richText = true;
			}

			var dic = new Dictionary<Vector3, List<int>>();
			for (var index = 0; index < _vertexList.Count; index++)
			{
				var pos = image.transform.position + image.transform.rotation * _vertexList[index].position;
				if (!dic.ContainsKey(pos))
					dic.Add(pos, new List<int>());

				dic[pos].Add(index);
			}

			foreach (var pair in dic)
			{
				var uv = _vertexList[pair.Value.First()].uv0;
				Handles.Label(pair.Key, string.Format("u:{0:F1} v:{1:F1}", uv.x, uv.y), _style);
			}
		}
	}
}
