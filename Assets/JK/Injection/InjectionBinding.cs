using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection
{
    [DisallowMultipleComponent]
    public class InjectionBinding : MonoBehaviour
    {
        #region Inspector

        public List<Component> components;

        public string id;

        public InjectionType injectionType = InjectionType.Self;

        #endregion

        [Serializable]
        public enum InjectionType
        {
            Self,
            Base,
            Interfaces,
        }

        private void Awake()
        {
            if (components == null || components.Count == 0)
                return;

            Context context = Context.Find(this);

            switch (injectionType)
            {
                default:
                case InjectionType.Self:
                    BindToSelf(context);
                    break;
                case InjectionType.Base:
                    BindToSuper(context);
                    break;
                case InjectionType.Interfaces:
                    BindToInterfaces(context);
                    break;
            }
        }

        private void BindToSelf(Context context)
        {
            foreach (var comp in components)
                context.BindUnsafe(comp, comp.GetType(), id);
        }

        private void BindToSuper(Context context)
        {
            foreach (var comp in components)
                context.BindUnsafe(comp, comp.GetType().BaseType, id);
        }

        private void BindToInterfaces(Context context)
        {
            foreach (var comp in components)
                foreach (Type interfaceType in comp.GetType().GetInterfaces())
                    context.BindUnsafe(comp, interfaceType, id);
        }
    }
}