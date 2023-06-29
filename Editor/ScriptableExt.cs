using UnityEditor;
using UnityEngine;

namespace OT.Extensions
{
    public static class ScriptableExt
    {
        public static void Save(this ScriptableObject scriptable, bool ping = false)
        {
            EditorUtility.SetDirty(scriptable);
            AssetDatabase.Refresh();

            if (ping)
                EditorGUIUtility.PingObject(scriptable);
        }
    }
}