using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OT.Extensions
{
    public static class EditorContextExtension
    {
        #region Editor menu: File

        /// <summary>
        /// Ping active scene in Project section.
        /// </summary>
        [MenuItem("File/Ping Active Scene  &s", false, 0)]
        private static void PingActiveScene()
        {
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(SceneManager.GetActiveScene().path);
            EditorGUIUtility.PingObject(Selection.activeObject);
        }

        #endregion

        #region Inspector

        /// <summary>
        /// Inspector context: Rename gameobject as script/component.
        /// </summary>
        /// <param name="menuCommand">Context menu command.</param>
        [MenuItem("CONTEXT/Component/Rename Object As Component", false, -100)]
        private static void RenameGameObjectAsScript(MenuCommand menuCommand)
        {
            Component c = (Component) menuCommand.context;
            Undo.RegisterCompleteObjectUndo(c.gameObject, "Player name change");
            c.gameObject.name = c.GetType().Name;
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
    }
}