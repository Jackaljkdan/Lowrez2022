using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class LateFollow : FollowBehaviour
    {
        #region Inspector



        #endregion

        private void LateUpdate()
        {
            transform.position = target.position + offset;
        }
    }
}