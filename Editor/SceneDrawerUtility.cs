using System;
using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTool.SceneDrawer
{
	internal class SceneDrawerUtility
	{
		internal const string TOOL_PATH = "Tools/SceneDrawer/";
	}

	internal class GizmoMatrixScope : IDisposable
	{
		private readonly Matrix4x4 _cache;

		internal GizmoMatrixScope(Matrix4x4 matrix4X4)
		{
			_cache = Gizmos.matrix;
			Gizmos.matrix = matrix4X4;
		}

		internal GizmoMatrixScope(Transform transform)
		{
			_cache = Gizmos.matrix;
			Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		}

		void IDisposable.Dispose()
		{
			Gizmos.matrix = _cache;
		}
	}

	internal class GizmoColorScope : IDisposable
	{
		private readonly Color _cache;

		internal GizmoColorScope(Color color)
		{
			_cache = Gizmos.color;
			Gizmos.color = color;
		}

		void IDisposable.Dispose()
		{
			Gizmos.color = _cache;
		}
	}

	internal class HandlesMatrixScope : IDisposable
	{
		private readonly Matrix4x4 _cache;

		internal HandlesMatrixScope(Transform transform)
		{
			_cache = Handles.matrix;
			Handles.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		}

		void IDisposable.Dispose()
		{
			Handles.matrix = _cache;
		}
	}

	internal class HandlesColorScope : IDisposable
	{
		private readonly Color _cache;

		internal HandlesColorScope(Color color)
		{
			_cache = Handles.color;
			Handles.color = color;
		}

		void IDisposable.Dispose()
		{
			Handles.color = _cache;
		}
	}
}
