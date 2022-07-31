using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection
{
    public abstract class Installer : MonoBehaviour
    {
        #region Inspector



        #endregion

        public abstract void Install(Context context);
    }
}