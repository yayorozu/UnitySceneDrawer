using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool
{
	public class EditorMeshInfoDrawer
	{
		private const string MENU_PATH = "Tools/Scene Drawer/Show Mesh Info";
		
		private static readonly StringBuilder _builder = new StringBuilder();
		private static GUIStyle _style;

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
		private static void DrawGizmo(MeshFilter meshFilter, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			if (meshFilter.sharedMesh == null)
				return;

			if (_style == null)
			{
				_style = new GUIStyle(GUI.skin.window);

				_style.padding.bottom -= 27;
				_style.padding.left -= 7;
				_style.padding.right -= 7;
				_style.margin = new RectOffset();
				_style.richText = true;
			}

			// 同じ座標が存在するのでキャッシュ
			var dic = new Dictionary<Vector3, List<int>>();
			var mesh = meshFilter.sharedMesh;
			for (var index = 0; index < mesh.uv.Length; index++)
			{
				var pos = meshFilter.transform.position + meshFilter.transform.rotation * mesh.vertices[index];
				if (!dic.ContainsKey(pos))
					dic.Add(pos, new List<int>());

				dic[pos].Add(index);
			}

			foreach (var pair in dic)
			{
				_builder.Clear();
				foreach (var index in pair.Value)
					if (mesh.colors.Length > index)
					{
						var color = ColorUtility.ToHtmlStringRGB(mesh.colors[index]);
						_builder.AppendLine($"<color=#{color}>uv:{mesh.uv[index]}</color>");
					}
					else
					{
						_builder.AppendLine("uv:" + mesh.uv[index]);
					}

				Handles.Label(pair.Key, _builder.ToString(), _style);
			}
		}
	}
}