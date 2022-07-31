using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.World
{
    [DisallowMultipleComponent]
    public class ColliderStayEvent : MonoBehaviour
    {
        #region Inspector

        public UnityEvent<Collider, Collision> onCollisionStay = new UnityEvent<Collider, Collision>();

        #endregion

        public Collider Collider { get; private set; }

        private void Awake()
        {
            Collider = GetComponent<Collider>();
        }

        private void OnCollisionStay(Collision collision)
        {
            onCollisionStay.Invoke(Collider, collision);
        }
    }
}