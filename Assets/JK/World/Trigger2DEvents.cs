using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.World
{
    [DisallowMultipleComponent]
    public class Trigger2DEvents : MonoBehaviour
    {
        #region Inspector

        public UnityEvent<Collider2D> onEnter = new UnityEvent<Collider2D>();
        public UnityEvent<Collider2D> onExit = new UnityEvent<Collider2D>();

        #endregion

        private void Start()
        {
            // leave this even if empty so that this behaviour can be enabled/disabled in the inspector
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (enabled)
                onEnter.Invoke(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (enabled)
                onExit.Invoke(collision);
        }
    }
}