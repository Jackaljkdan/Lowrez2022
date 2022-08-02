using JK.Injection;
using JK.Observables;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.Tension
{
    [DisallowMultipleComponent]
    public class TensionInstaller : Installer
    {
        #region Inspector

        [Range(0, 1)]
        public float editorTension;

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                var tension = Context.Find(this)?.Get<ObservableProperty<float>>("tension");

                if (tension != null)
                    tension.Value = editorTension;
            }
        }

        #endregion

        public override void Install(Context context)
        {
            var tension = new ObservableProperty<float>();
            context.Bind(tension, "tension");

            if (PlatformUtils.IsEditor)
                tension.onChange.AddListener(OnTensionChanged);
        }

        private void OnTensionChanged(ObservableProperty<float>.Changed arg)
        {
            editorTension = arg.updated;
        }
    }
}