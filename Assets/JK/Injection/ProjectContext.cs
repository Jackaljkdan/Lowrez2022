using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection
{
    [DisallowMultipleComponent]
    public class ProjectContext : MonoContext
    {
        #region Inspector



        #endregion

        public const string ObjectName = nameof(ProjectContext);

        public static ProjectContext Get()
        {
            if (projectContext == null && firstGet)
            {
                firstGet = false;
                ProjectContext prefab = Resources.Load<ProjectContext>(ObjectName);

                if (prefab != null)
                {
                    projectContext = Instantiate(prefab, parent: null);
                    projectContext.name = ObjectName;
                    DontDestroyOnLoad(projectContext.gameObject);
                }
            }

            return projectContext;
        }

        private static ProjectContext projectContext;
        private static bool firstGet = true;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            projectContext = null;
            firstGet = true;
        }
    }
}