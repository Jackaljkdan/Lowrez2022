using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection.Tests
{
    [DisallowMultipleComponent]
    public class TestProjectInstaller : Installer
    {
        #region Inspector



        #endregion

        public override void Install(Context context)
        {
            context.Bind("project", id: "project");
        }
    }
}