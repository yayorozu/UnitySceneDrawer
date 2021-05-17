using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool.SceneDrawer
{
	internal static class EditorColliderDrawer
	{
		private const string MENU_PATH = SceneDrawerUtility.TOOL_PATH + "Show Collider";

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

		private static readonly Color Color = new Color(1f, 0f, 1f, 1f);

		[DrawGizmo(GizmoType.NotInSelectionHierarchy)]
		private static void DrawBoxCollider2D(BoxCollider2D collider, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			using (new GizmoMatrixScope(collider.transform))
			{
				var size = collider.size / 2f;
				using (new GizmoColorScope(Color))
				{
					var offset = collider.offset;
					Gizmos.DrawLine(new Vector2(-size.x, -size.y) + offset, new Vector2(size.x, -size.y) + offset);
					Gizmos.DrawLine(new Vector2(size.x, -size.y) + offset, new Vector2(size.x, size.y) + offset);
					Gizmos.DrawLine(new Vector2(size.x, size.y) + offset, new Vector2(-size.x, size.y) + offset);
					Gizmos.DrawLine(new Vector2(-size.x, size.y) + offset, new Vector2(-size.x, -size.y) + offset);
				}
			}
		}

		[DrawGizmo(GizmoType.NonSelected)]
		private static void DrawCircleCollider2D(CircleCollider2D collider, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			using (new HandlesColorScope(Color))
			{
				var lossyScale = collider.transform.lossyScale;
				var radius = collider.radius * Mathf.Max(lossyScale.x, lossyScale.y);
				Handles.DrawWireArc(collider.bounds.center, Vector3.forward, Vector3.up * radius, 360f, radius);
			}
		}

		[DrawGizmo(GizmoType.NonSelected)]
		private static void DrawCapsuleCollider2D(CapsuleCollider2D collider, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			using (new HandlesColorScope(Color))
			{
				var sideHeight = (collider.size.y - collider.size.x) / 2f;
				var circleHeight = (collider.size.x - collider.size.y) / 2f;
				if (sideHeight < 0f)
					sideHeight = 0f;
				if (circleHeight > 0f)
					circleHeight = 0f;

				var top = collider.bounds.center;
				top.y -= circleHeight;
				Handles.DrawWireArc(top, Vector3.forward, Vector3.right * collider.size.x / 2f, 180f,
					collider.size.x / 2f);

				var bottom = collider.bounds.center;
				bottom.y += circleHeight;
				Handles.DrawWireArc(bottom, Vector3.forward, Vector3.left * collider.size.x / 2f, 180f,
					collider.size.x / 2f);


				var leftTop = collider.bounds.center;
				var leftBottom = collider.bounds.center;

				leftTop.x += collider.size.x / 2f;
				leftTop.y += sideHeight;

				leftBottom.x += collider.size.x / 2f;
				leftBottom.y -= sideHeight;
				Handles.DrawLine(leftTop, leftBottom);

				var rightTop = collider.bounds.center;
				var rightBottom = collider.bounds.center;

				rightTop.x -= collider.size.x / 2f;
				rightTop.y += sideHeight;

				rightBottom.x -= collider.size.x / 2f;
				rightBottom.y -= sideHeight;
				Handles.DrawLine(rightTop, rightBottom);
			}
		}

		[DrawGizmo(GizmoType.NonSelected)]
		private static void DrawPolygonCollider2D(PolygonCollider2D collider, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			using (new GizmoMatrixScope(collider.transform))
			{
				using (new GizmoColorScope(Color))
				{
					for (var i = 0; i < collider.pathCount; i++)
					{
						var paths = collider.GetPath(i);
						for (var j = 0; j < paths.Length; j++)
						{
							var next = j + 1 % paths.Length;
							if (next >= paths.Length)
								next = 0;

							var offset = collider.offset;
							Gizmos.DrawLine(paths[j] + offset, paths[next] + offset);
						}
					}
				}
			}

		}

		[DrawGizmo(GizmoType.NonSelected)]
		private static void DrawMeshCollider(MeshCollider collider, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			if (collider.sharedMesh == null)
				return;

			using (new GizmoMatrixScope(collider.transform))
			{
				using (new GizmoColorScope(Color))
				{
					Gizmos.DrawWireMesh(collider.sharedMesh, Vector3.zero, Quaternion.identity);
				}
			}
		}

		[DrawGizmo(GizmoType.NonSelected)]
		private static void DrawSphereCollider(SphereCollider collider, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			using (new GizmoMatrixScope(collider.transform))
			{
				using (new GizmoColorScope(Color))
				{
					Gizmos.DrawWireSphere(collider.center, collider.radius);
				}
			}
		}

		[DrawGizmo(GizmoType.NonSelected)]
		private static void DrawBoxCollider(BoxCollider collider, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			using (new GizmoMatrixScope(collider.transform))
			{
				using (new GizmoColorScope(Color))
				{
					var center = collider.center;
					var min = Vector3.zero;
					var max = Vector3.zero;
					for (var i = 0; i < 3; i++)
					{
						min[i] = center[i] - collider.size[i] / 2f;
						max[i] = center[i] + collider.size[i] / 2f;
					}

					Gizmos.DrawLine(min, new Vector3(max.x, min.y, min.z));
					Gizmos.DrawLine(min, new Vector3(min.x, max.y, min.z));
					Gizmos.DrawLine(min, new Vector3(min.x, min.y, max.z));
					Gizmos.DrawLine(max, new Vector3(min.x, max.y, max.z));
					Gizmos.DrawLine(max, new Vector3(max.x, max.y, min.z));
					Gizmos.DrawLine(max, new Vector3(max.x, min.y, max.z));
					Gizmos.DrawLine(
						new Vector3(min.x, max.y, min.z),
						new Vector3(max.x, max.y, min.z)
					);
					Gizmos.DrawLine(
						new Vector3(min.x, max.y, min.z),
						new Vector3(min.x, max.y, max.z)
					);
					Gizmos.DrawLine(
						new Vector3(max.x, min.y, min.z),
						new Vector3(max.x, min.y, max.z)
					);
					Gizmos.DrawLine(
						new Vector3(max.x, min.y, min.z),
						new Vector3(max.x, max.y, min.z)
					);
					Gizmos.DrawLine(
						new Vector3(min.x, min.y, max.z),
						new Vector3(max.x, min.y, max.z)
					);
					Gizmos.DrawLine(
						new Vector3(min.x, min.y, max.z),
						new Vector3(min.x, max.y, max.z)
					);
				}
			}
		}

		[DrawGizmo(GizmoType.NonSelected)]
		private static void DrawCapsuleCollider(CapsuleCollider collider, GizmoType type)
		{
			if (!EditorPrefs.GetBool(MENU_PATH, false))
				return;

			var top = collider.center;
			var bottom = collider.center;
			top.y += collider.height / 2f - collider.radius;
			bottom.y -= collider.height / 2f - collider.radius;
			top += collider.transform.position;
			bottom += collider.transform.position;

			var forward = bottom - top;
			var rot = Quaternion.LookRotation(forward);
			var pointOffset = collider.radius / 2f;
			var length = forward.magnitude;
			var center2 = new Vector3(0f, 0, length);
			var matrix = Matrix4x4.TRS(top, rot, Handles.matrix.lossyScale);
			using (new Handles.DrawingScope(Color, matrix))
			{
				Handles.DrawWireDisc(Vector3.zero, Vector3.forward, collider.radius);
				Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.left * pointOffset, -180f, collider.radius);
				Handles.DrawWireArc(Vector3.zero, Vector3.left, Vector3.down * pointOffset, -180f, collider.radius);
				Handles.DrawWireDisc(center2, Vector3.forward, collider.radius);
				Handles.DrawWireArc(center2, Vector3.up, Vector3.right * pointOffset, -180f, collider.radius);
				Handles.DrawWireArc(center2, Vector3.left, Vector3.up * pointOffset, -180f, collider.radius);
				DrawLine(collider.radius, 0f, length);
				DrawLine(-collider.radius, 0f, length);
				DrawLine(0f, collider.radius, length);
				DrawLine(0f, -collider.radius, length);
			}
		}

		private static void DrawLine(float x, float y, float forward)
		{
			Handles.DrawLine(new Vector3(x, y, 0f), new Vector3(x, y, forward));
		}
	}
}
