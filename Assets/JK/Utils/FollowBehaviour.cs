using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public abstract class FollowBehaviour : MonoBehaviour
    {
        #region Inspector

        public Transform target;

        public Vector3 offset = Vector3.zero;

        #endregion
    }
}