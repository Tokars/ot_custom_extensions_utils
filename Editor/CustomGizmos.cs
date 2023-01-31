using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OT.Extensions
{
    public static class CustomGizmos
    {
        public static void DrawGizmoDirection(IList<Vector3> points, Vector3[] directions, float rayLength, Color ray,
            Color source)
        {
            for (int i = 0; i < points.Count; i++)
            {
                Gizmos.color = source;
                Gizmos.DrawWireCube(points[i], Vector3.one * 0.32F);
                Gizmos.color = ray;
                Gizmos.DrawLine(points[i], directions[i] * rayLength);
            }
        }

        public static void DrawGizmoCurve(IList<Vector3> points, Color path, Color dot, float radius = 1)
        {
            if (points == null) return;

            for (int j = 1; j < points.Count; j++)
            {
                Gizmos.color = path;

                var start = points[j - 1];
                var target = points[j];
                Gizmos.DrawLine(start, target);

                Gizmos.color = dot;
                Gizmos.DrawSphere(points[j - 1], radius);
            }
        }

        private static readonly GUIStyle labelGUIStyle = new GUIStyle();

        /// <summary>
        /// Draw editor label.
        /// </summary>
        /// <param name="pos">label position.</param>
        /// <param name="str">label text.</param>
        /// <param name="color">label color.</param>
        public static void DrawLabel(Vector3 pos, string str, Color color)
        {
            labelGUIStyle.normal.textColor = color;
            labelGUIStyle.fontSize = 9;
            Handles.Label(pos, str, labelGUIStyle);
        }

        private static Color32 _textColor;
        private static Color32 _bgColor;

        public static void DrawString(string text, Vector3 worldPos)
        {
            _textColor = Color.green;
            _bgColor = Color.blue;
            Handles.BeginGUI();

            GUI.color = Color.green;
            GUI.backgroundColor = Color.blue;

            var view = SceneView.currentDrawingSceneView;
            if (view != null && view.camera != null)
            {
                Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
                if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width ||
                    screenPos.z < 0)
                {
                    GUI.color = _textColor;
                    Handles.EndGUI();
                    return;
                }

                Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
                var r = new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y);
                GUI.Box(r, text, EditorStyles.numberField);
                GUI.Label(r, text);
                GUI.color = _textColor;
                GUI.backgroundColor = _bgColor;
            }

            Handles.EndGUI();
        }
    }
}