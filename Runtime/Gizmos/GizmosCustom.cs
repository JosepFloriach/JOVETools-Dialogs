using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class GizmosCustom
{
   public static void DrawCircle(Vector3 position, float radius, int segments)
    {
        float diffAngle = (2 * Mathf.PI)/ segments;
        float currAngle = 0.0f;

        var points = new List<Vector2>();
        for (int segment = 0; segment < segments; ++segment)
        {
            float x = (radius * Mathf.Cos(currAngle)) + position.x;
            float y = (radius * Mathf.Sin(currAngle)) + position.y;
            points.Add(new Vector2(x, y));
            currAngle += diffAngle;
        }

        for (int idx = 0; idx < points.Count - 1; ++idx)
        {
            Gizmos.DrawLine(points[idx], points[idx + 1]);
        }
        Gizmos.DrawLine(points[points.Count - 1], points[0]);
    }

#if UNITY_EDITOR
    static MethodInfo setIconEnabled;
    static MethodInfo SetIconEnabled => setIconEnabled = setIconEnabled ??
    Assembly.GetAssembly(typeof(Editor))
    ?.GetType("UnityEditor.AnnotationUtility")
    ?.GetMethod("SetIconEnabled", BindingFlags.Static | BindingFlags.NonPublic);
    public static void SetGizmoIconEnabled(Type type, bool on)
    {
        if (SetIconEnabled == null) return;
        const int MONO_BEHAVIOR_CLASS_ID = 114; // https://docs.unity3d.com/Manual/ClassIDReference.html
        SetIconEnabled.Invoke(null, new object[] { MONO_BEHAVIOR_CLASS_ID, type.Name, on ? 1 : 0 });
    }
#endif
}
