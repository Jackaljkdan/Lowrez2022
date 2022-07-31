using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.World
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public class ColliderUnfrequentEvents : MonoBehaviour
    {
        #region Inspector

        public UnityEvent<Collider, Collision> onCollisionEnter = new UnityEvent<Collider, Collision>();
        public UnityEvent<Collider, Collision> onCollisionExit = new UnityEvent<Collider, Collision>();

        #endregion

        public Collider Collider { get; private set; }

        private void Awake()
        {
            Collider = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            onCollisionEnter.Invoke(Collider, collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            onCollisionExit.Invoke(Collider, collision);
        }
    }
}