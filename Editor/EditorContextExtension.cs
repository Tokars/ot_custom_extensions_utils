using UnityEditor;
using UnityEngine;

namespace OT.Extensions
{
    public static class EditorContextExtension
    {
        [MenuItem("Assets/Save RenderTexture to file")]
        public static void ContextSaveRenderTextureToFile()
        {
            RenderTexture rt = Selection.activeObject as RenderTexture;
            SaveFile(rt, GetSavePath( rt.name + PNG), TextureFormat.RGB24);
        }

        public static void Save(this RenderTexture rt, TextureFormat format, string path, string name)
        {
            SaveFile(rt, path+name+PNG, format);
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