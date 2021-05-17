using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool.SceneDrawer
{
	/// <summary>
	/// MeshFilter の 頂点カラーとUVを表示
	/// </summary>
	internal static class EditorMeshInfoDrawer
	{
		private const string MENU_PATH = SceneDrawerUtility.TOOL_PATH + "Show Mesh Info";

		private static readonly StringBuilder _builder = new StringBuilder();

		private static class Styles
		{
			internal static GUIStyle Label;

			static Styles()
			{
				Label = new GUIStyle(GUI.skin.window);
				Label.padding.bottom -= 27;
				Label.padding.left -= 7;
				Label.padding.right -= 7;
				Label.margin = new RectOffset();
				Label.richText = true;
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
		private static void DrawGizmo(MeshFilter meshFilter, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			if (meshFilter.sharedMesh == null || !meshFilter.gameObject.activeSelf)
				return;

			// 同じ座標が存在するのでキャッシュ
			var dic = new Dictionary<Vector3, List<int>>();
			var mesh = meshFilter.sharedMesh;
			for (var index = 0; index < mesh.uv.Length; index++)
			{
				var pos = mesh.vertices[index];
				if (!dic.ContainsKey(pos))
					dic.Add(pos, new List<int>());

				dic[pos].Add(index);
			}

			using (new HandlesMatrixScope(meshFilter.transform))
			{
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

					Handles.Label(pair.Key, _builder.ToString(), Styles.Label);
				}
			}
		}
	}
}
