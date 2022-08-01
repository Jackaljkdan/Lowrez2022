using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class FollowBehaviourInjector : MonoBehaviour
    {
        #region Inspector

        public string injectionId;

        #endregion

        private void Awake()
        {
            GetComponent<FollowBehaviour>().target = Context.Find(this).Get<Transform>(injectionId);
        }
    }
}