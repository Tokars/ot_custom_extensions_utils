using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace OT.Extensions
{
    public static class EditorContextExtension
    {
        #region Editor menu: File

        /// <summary>
        /// Ping active scene in Project section.
        /// </summary>
        [MenuItem("Assets/Ping Active Scene  &s", false, 0)]
        private static void PingActiveScene()
        {
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(SceneManager.GetActiveScene().path);
            if (Selection.activeObject == null) return;
            EditorGUIUtility.PingObject(Selection.activeObject);
        }

        #endregion

        #region Inspector

        [MenuItem("CONTEXT/Component/Rm. All Components", false, -101)]
        public static void ClearComponents(MenuCommand menuCommand)
        {
            var component = (Component) menuCommand.context;

            var all = component.GetComponents<Component>();

            for (int i = all.Length - 1; i > 0; i--)
            {
                var comp = all[i];
                if (comp is Transform) continue;
                GameObject.DestroyImmediate(comp);
            }
        }


        /// <summary>
        /// Renames serialized gameobjects as component's fields declared.  
        /// </summary>
        [MenuItem("CONTEXT/Component/Rename Fields As Declared", false, -100)]
        public static void RenameFieldsAsDefined(MenuCommand menuCommand)
        {
            var component = (MonoBehaviour) menuCommand.context;

            FieldInfo[] fisInfos = component.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (var info in fisInfos)
            {
                if (IsSerializedField(info.GetCustomAttributes()))
                {
                    // Debug.Log(info.Name);
                    var value = info.GetValue(component);
                    if (value is Component == false) continue;
                    var go = value as Component;
                    Undo.RegisterCompleteObjectUndo(go, "renamed serialized fields as defined");
                    (value as Component).name = info.Name.ToPascalCase();
                }
            }

            bool IsSerializedField(IEnumerable<Attribute> attr)
            {
                foreach (var attribute in attr)
                    if (attribute is SerializeField)
                        return true;

                return false;
            }
        }

        [MenuItem("CONTEXT/Component/Rename Fields As Declared", true)]
        private static bool RenameFieldsAsDefinedValidation(MenuCommand menuCommand)
        {
            return menuCommand.context is MonoBehaviour;
        }

        /// <summary>
        /// Inspector context: Rename component as script/component.
        /// </summary>
        /// <param name="menuCommand">Context menu command.</param>
        [MenuItem("CONTEXT/Component/Rename As Component", false, -100)]
        private static void RenameGameObjectAsScript(MenuCommand menuCommand)
        {
            Component component = (Component) menuCommand.context;
            Undo.RegisterCompleteObjectUndo(component.gameObject, "gameObject name change");
            component.gameObject.name = component.GetType().Name;
        }

        [MenuItem("CONTEXT/Component/Rename As Component", true)]
        private static bool RenameGameObjectAsScriptValidation(MenuCommand menuCommand)
        {
            return (Component) menuCommand.context != null;
        }


        /// <summary>
        /// Inspector context: Transform round scale to int.
        /// </summary>
        /// <param name="menuCommand">Context menu command.</param>
        [MenuItem("CONTEXT/Transform/RoundScaleToInt", false, -100)]
        private static void RoundScale(MenuCommand menuCommand)
        {
            Transform tr = menuCommand.context as Transform;
            var inputScale = tr.localScale;
            tr.localScale = Vector3Int.RoundToInt(tr.localScale);

            var scale = tr.localScale;
            var x = scale.x * 2;

            x -= scale.y;
            x -= scale.z;

            if (Mathf.Abs(x) > 0)
            {
                var result = EditorUtility.DisplayDialogComplex("Non-uniform scale",
                    $"Object name: {tr.gameObject.name} \n Input scale: {inputScale}",
                    "Force apply to uniform",
                    "Cancel",
                    "Restore"
                );
                if (result == 0)
                    tr.localScale = new Vector3(scale.x, scale.x, scale.x);
                else
                    tr.localScale = inputScale;
            }
        }

        [MenuItem("GameObject/Clear", true, -100)]
        private static bool ClearChildValidate()
        {
            return Selection.activeObject is Transform;
        }

        [MenuItem("GameObject/Clear", false, -100)]
        private static void ClearChild()
        {
            Selection.gameObjects[0].transform.Clear();
        }

        /// <summary>
        /// Context: Component Move to top.
        /// </summary>
        /// <param name="menuCommand">Context menu command.</param>
        [MenuItem("CONTEXT/Component/Move To Top", false, -100)]
        private static void MoveToTop(MenuCommand menuCommand)
        {
            Component c = (Component) menuCommand.context;
            Component[] allComponents = c.GetComponents<Component>();
            int iOffset = 0;
            for (int i = 0; i < allComponents.Length; i++)
            {
                if (allComponents[i] == c)
                {
                    iOffset = i;
                    break;
                }
            }

            for (int i = 0; i < iOffset - 1; i++)
                UnityEditorInternal.ComponentUtility.MoveComponentUp(c);

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        /// <summary>
        /// Context: Component Move to bottom.
        /// </summary>
        /// <param name="menuCommand">Context menu command.</param>
        [MenuItem("CONTEXT/Component/Move To Bottom", false, -100)]
        private static void MoveToBottom(MenuCommand menuCommand)
        {
            Component c = (Component) menuCommand.context;
            Component[] allComponents = c.GetComponents<Component>();
            int iOffset = 0;
            for (int i = 0; i < allComponents.Length; i++)
                if (allComponents[i] == c)
                {
                    iOffset = i;
                    break;
                }

            for (; iOffset < allComponents.Length; iOffset++)
                UnityEditorInternal.ComponentUtility.MoveComponentDown(c);

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        #endregion


        /// <summary>
        /// Context Assets: Save render texture to picture.  
        /// </summary>
        [MenuItem("Assets/Save RenderTexture to file")]
        public static void ContextSaveRenderTextureToFile()
        {
            RenderTexture rt = Selection.activeObject as RenderTexture;
            SaveFile(rt, GetSavePath(rt.name + PNG), TextureFormat.RGB24);
        }

        public static void Save(this RenderTexture rt, TextureFormat format, string path, string name)
        {
            SaveFile(rt, path + name + PNG, format);
        }

        private static void SaveFile(RenderTexture rt, string path, TextureFormat format = TextureFormat.ARGB4444)
        {
            if (rt == null)
            {
                Debug.LogError("Render Texture is null");
                return;
            }

            RenderTexture.active = rt;
            Texture2D tex = new Texture2D(rt.width, rt.height, format, false);
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            RenderTexture.active = null;

            byte[] bytes;
            bytes = tex.EncodeToPNG();

            System.IO.File.WriteAllBytes(path, bytes);
            AssetDatabase.ImportAsset(path);
            // Debug.Log("Saved to " + path);
        }


        [MenuItem("Assets/Save RenderTexture to file", true)]
        public static bool ContextSaveRenderTextureToFileValidation()
        {
            return Selection.activeObject is RenderTexture;
        }


        private static string GetSavePath(string preferredName)
        {
            var path = EditorUtility.SaveFilePanel("Save Image", "Assets/", preferredName, PNG);
            return FileUtil.GetProjectRelativePath(path);
        }

        private const string PNG = ".png";


        [MenuItem("Assets/Find missing components")]
        private static void FindMissingComponents()
        {
            string[] paths = AssetDatabase.GetAllAssetPaths()
                .Where(path => path.EndsWith(".prefab", System.StringComparison.OrdinalIgnoreCase)).ToArray();

            foreach (var path in paths)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                foreach (var component in prefab.GetComponentsInChildren<Component>())
                {
                    if (component == null)
                        Debug.Log($"Prefab has missing component: {path}", prefab);
                }
            }
        }
    }
}