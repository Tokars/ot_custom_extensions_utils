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

        internal static string CreateFolder(string pathFolder)
        {
            string newFolder = Path.Combine(pathFolder);
            Directory.CreateDirectory(newFolder);
            return newFolder;
        }

        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
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


        private static T CreateScriptable<T>(string path) where T : ScriptableObject
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