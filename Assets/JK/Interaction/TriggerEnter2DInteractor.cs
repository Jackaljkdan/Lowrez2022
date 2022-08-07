using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    [DisallowMultipleComponent]
    public class TriggerEnter2DInteractor : MonoBehaviour
    {
        #region Inspector

        public InteractableBehaviour target;

        #endregion

        private void Start()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("trigger enter on: " + collision.name);

            if (collision.TryGetComponent(out IInteractable interactable))
                interactable.Interact(default);
        }
    }
}