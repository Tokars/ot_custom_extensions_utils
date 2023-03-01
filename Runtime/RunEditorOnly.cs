using System.Reflection;

namespace OT.Extensions
{
    public static class RunEditorOnly
    {
        /// <summary>
        /// Runtime clear logs in editor.
        /// </summary>
        public static void ClearLog()
        {
#if UNITY_EDITOR

            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);

#endif
        }
    }
}