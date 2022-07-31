using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection
{
    [DisallowMultipleComponent]
    public class MonoContext : MonoBehaviour
    {
        #region Inspector

        public Transform installersParent;

        private void Reset()
        {
            installersParent = transform;
        }

        #endregion

        private static List<Installer> list = new List<Installer>(16);

#if UNITY_EDITOR
        [SerializeReference]
#else
        [NonSerialized]
#endif
        public Context context;

        private void Awake()
        {
            InitIfNeeded();
        }

        private void InitIfNeeded()
        {
            if (context != null)
                return;

            Context container = null;

            if (transform.parent != null)
            {
                MonoContext ancestor = transform.parent.GetComponentInParent<MonoContext>();

                if (ancestor != null)
                {
                    ancestor.InitIfNeeded();
                    container = ancestor.context;
                }
            }

            if (container == null)
            {
                ProjectContext projectContext = ProjectContext.Get();
                if (projectContext != null)
                    container = projectContext.context;
            }

            context = new Context(container, name);

            if (installersParent != null)
            {
                installersParent.GetComponentsInChildren(list);

                foreach (Installer installer in list)
                    installer.Install(context);
            }
        }
    }
}