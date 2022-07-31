using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace JK.Editor
{
    public static class MenuItemUtils
    {
        public static string GetSelectionAbsolutePath()
        {
            var selected = Selection.GetFiltered<UnityEngine.Object>(SelectionMode.Assets);

            if (selected == null || selected.Length == 0)
                return null;

            foreach (var item in selected)
            {
                var relativePath = AssetDatabase.GetAssetPath(item);

                if (string.IsNullOrEmpty(relativePath))
                    continue;

                var fullPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", relativePath));
                return fullPath;
            }

            return null;
        }

        public static string GetSelectionAbsoluteFolder()
        {
            var absolutePath = GetSelectionAbsolutePath();
            //Debug.Log("abs: " + absolutePath);

            if (string.IsNullOrEmpty(absolutePath))
                return null; ;

            bool isFile = File.Exists(absolutePath);

            var absoluteFolder = isFile ? Path.GetDirectoryName(absolutePath) : absolutePath;
            return absoluteFolder;
        }

        public static string ConvertAbsolutePathToAssetPath(string absolutePath)
        {
            var assetFolderFullPath = Path.GetFullPath(Application.dataPath);

            if (absolutePath == assetFolderFullPath)
                return "Assets";

            var assetPath = absolutePath.Remove(0, assetFolderFullPath.Length + 1).Replace("\\", "/");
            return "Assets/" + assetPath;
        }
    }
}