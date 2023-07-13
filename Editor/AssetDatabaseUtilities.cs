using System;
using System.Collections.Generic;
using System.IO;
using OT.Extensions.Types;
using UnityEditor;
using UnityEngine;

namespace OT.Extensions
{
    /// <summary>
    /// Custom Unity directory, file, unity based assets help-utils.
    /// </summary>
    public static class AssetDatabaseUtilities
    {
        /// <summary>
        /// EDITOR load assets by filter and paths.
        /// </summary>
        /// <param name="searchFilter">filters as in editor "t:prefab" for example.</param>
        /// <param name="paths">project directory paths.</param>
        /// <typeparam name="T">unity object type based.</typeparam>
        /// <returns>array of T[]</returns>
        public static T[] LoadFilteredAssetsByPaths<T>(string searchFilter, string[] paths) where T : UnityEngine.Object
        {
            string[] guids = AssetDatabase.FindAssets(searchFilter, paths);
            var assets = new T[guids.Length];
            for (var i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                assets[i] = AssetDatabase.LoadAssetAtPath<T>(path);
                // Debug.Log($"asset path : {path}");
            }

            return assets;
        }
        
        
        #region Path

        public static string GetProjectRoot()
        {
            DirectoryInfo dataInfo = new DirectoryInfo(Application.dataPath);
            return dataInfo.Parent.ToString();
        }

        public static string GetRelativeToProjectRoot(string path)
        {
            return Path.Combine(GetProjectRoot(), path);
        }

        public static string CreateFolder(string pathFolder)
        {
            Directory.CreateDirectory(pathFolder);
            return pathFolder;
        }

        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }
        public static bool DirectoryIsEmpty(string path)
        {
            return Directory.GetFiles(path).Length <= 0;
        }
        
        /// <summary>
        /// Editor project window util.
        /// </summary>
        /// <returns>path of selected Project directory.</returns>
        public static string GetPathSelectedDir()
        {
            string result = string.Empty;
            UnityEngine.Object[] objs = Selection.GetFiltered<UnityEngine.Object>(SelectionMode.Assets);
            foreach (UnityEngine.Object obj in objs)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    result = path;
                    Debug.Log(result);
                    break;
                }
            }
            return result;
        }

        public static void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (dir.Exists == false)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (DirectoryExists(destDirName) == false)
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    CopyDirectory(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        public static void ClearDirectory(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles())
                file.Delete();

            foreach (DirectoryInfo dir in di.GetDirectories())
                dir.Delete(true);
        }

        #endregion

        #region File

        public static void CreateJSONFile(object obj, string parentPath, string name)
        {
            string objStr = JsonUtility.ToJson(obj, true);
            CreateTextFile(objStr, parentPath, name);
        }

        public static void CreateTextFile(string str, string parentPath, string name)
        {
            string asset = Path.Combine(parentPath, name);
            StreamWriter writer = new StreamWriter(asset, false);
            writer.Write(str);
            writer.Close();
        }

        public static string ReadTextFile(string parentPath, string name)
        {
            string asset = Path.Combine(parentPath, name);

            if (!File.Exists(asset))
            {
                return null;
            }

            try
            {
                using (StreamReader reader = new StreamReader(asset))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return null;
            }
        }

        #endregion

        #region Unity based assets

        public static void CreatePrefab(string path, GameObject gameObject)
        {
            PrefabUtility.SaveAsPrefabAsset(gameObject, path);
        }


        public static T CreateScriptable<T>(string path) where T : ScriptableObject
        {
            if (DirectoryExists(path) == false)
                CreateFolder(path);

            string filePath = $"{path}{typeof(T).Name}{Paths.AssetExt}";

            if (File.Exists(filePath))
                File.Delete(filePath);

            var instance = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(instance, filePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = instance;
            return instance;
        }


        /// <summary>
        /// Create unity assembly definition.
        /// </summary>
        /// <param name="path">Path create.</param>
        /// <param name="name">Title of Assembly definition.</param>
        /// <param name="rootNamespace">Assembly root namespace.</param>
        /// <param name="editor">Platform: for Editor if true, false for any.</param>
        /// <param name="references">Assemblies references.</param>
        /// <returns></returns>
        public static AssemblyDefinition CreateAssemblyDef(string path, string name, string rootNamespace,
            bool editor,
            List<string> references)
        {
            string folder = CreateFolder(path);
            AssemblyDefinition def = new AssemblyDefinition();

            def.name = name;
            def.rootNamespace = rootNamespace;

            if (editor)
                def.includePlatforms.Add("Editor");

            def.references = references;

            CreateJSONFile(def, folder, def.name + Paths.AssemblyDefExt);


            string AssemblyDefinition = @"
using System.Reflection;

[assembly: AssemblyTitle(""" + def.name + @""")]
[assembly: AssemblyProduct(""" + def.name + @""")]
";

            CreateTextFile(AssemblyDefinition, folder, "AssemblyInfo.cs");
            return def;
        }

        #endregion

        #region Asset Database

        public static void UpdateAssetDatabase(
            ImportAssetOptions importOption = ImportAssetOptions.ForceSynchronousImport)
        {
            AssetDatabase.Refresh(importOption);
        }

        #endregion
    }
}
