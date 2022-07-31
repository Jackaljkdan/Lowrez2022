using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection.Tests
{
    [DisallowMultipleComponent]
    public class Test2Installer : Installer
    {
        #region Inspector



        #endregion

        public override void Install(Context context)
        {
            context.Bind("test2", "test2");
        }
    }
}