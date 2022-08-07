using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    [DisallowMultipleComponent]
    public class TriggerExit2DInteractor : MonoBehaviour
    {
        #region Inspector

        public InteractableBehaviour target;

        #endregion

        private void Start()
        {
            
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log("trigger exit on: " + collision.name + " target: " + target.GetName());

            if (target != null)
                target.Interact(default);
            else if (collision.TryGetComponent(out IInteractable interactable))
                interactable.Interact(default);
        }
    }
}