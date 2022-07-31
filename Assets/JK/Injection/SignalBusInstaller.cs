using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection
{
    [DisallowMultipleComponent]
    public class SignalBusInstaller : Installer
    {
        #region Inspector



        #endregion

        public override void Install(Context context)
        {
            context.Bind(new SignalBus());
        }
    }
}