using JK.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace JK.Injection.Editor
{
    public static class InjectionMenuItems
    {
        [MenuItem("Assets/Create/JK/Injection/Project Context", false, 40)]
        public static void CreateProjectContext()
        {
            var absoluteFolder = MenuItemUtils.GetSelectionAbsoluteFolder();
            var dirName = new DirectoryInfo(absoluteFolder).Name;

            if (dirName != "Resources")
            {
                absoluteFolder = Path.Combine(absoluteFolder, "Resources");
                Directory.CreateDirectory(absoluteFolder);
            }

            GameObject gameObject = null;

            try
            {
                gameObject = new GameObject(ProjectContext.ObjectName, typeof(ProjectContext));
                gameObject.isStatic = true;

                GameObject signalBusObject = new GameObject(nameof(SignalBusInstaller), typeof(SignalBusInstaller));
                signalBusObject.isStatic = true;
                signalBusObject.transform.SetParent(gameObject.transform);

                GameObject sceneParametersObject = new GameObject(nameof(SceneParametersBusInstaller), typeof(SceneParametersBusInstaller));
                sceneParametersObject.isStatic = true;
                sceneParametersObject.transform.SetParent(gameObject.transform);

                var assetPath = MenuItemUtils.ConvertAbsolutePathToAssetPath(absoluteFolder);
                var prefabPath = Path.Combine(assetPath, gameObject.name) + ".prefab";

                var prefab = PrefabUtility.SaveAsPrefabAsset(gameObject, prefabPath);
                Selection.activeObject = prefab;
            }
            finally
            {
                if (gameObject != null)
                    GameObject.DestroyImmediate(gameObject);
            }
        }

        [MenuItem("GameObject/JK/Injection/Root Context", false, 9)]
        public static void CreateSceneContext(MenuCommand menuCommand)
        {
            var rootContext = new GameObject("RootContext").AddComponent<MonoContext>();
            rootContext.gameObject.isStatic = true;
            Selection.activeGameObject = rootContext.gameObject;

            GameObject installers = new GameObject("Installers");
            installers.isStatic = true;
            installers.transform.SetParent(rootContext.transform);
            rootContext.installersParent = installers.transform;

            GameObject systems = new GameObject("Systems");
            systems.isStatic = true;
            systems.transform.SetParent(rootContext.transform);

            EditorGUIUtility.PingObject(installers);

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}