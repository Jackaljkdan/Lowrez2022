using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.Doors
{
    [DisallowMultipleComponent]
    public class DoorTrigger : MonoBehaviour
    {
        #region Inspector

        public DoorInteractable target;

        public bool openOnEnter = true;

        #endregion

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (openOnEnter)
                target.OpenAsInteraction();
            else
                target.CloseAsInteraction();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (openOnEnter)
                target.CloseAsInteraction();
            else
                target.OpenAsInteraction();
        }
    }
}