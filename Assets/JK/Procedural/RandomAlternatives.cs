using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public class RandomAlternatives : MonoBehaviour
    {
        #region Inspector

        public bool canSelectNone = true;

        [ContextMenu("Disable all")]
        private void DisableAllInEditMode()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
                Undo.SetDirty(child.gameObject);
            }
        }

        [ContextMenu("Enable all")]
        private void EnableAllInEditMode()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
                Undo.SetDirty(child.gameObject);
            }
        }

        #endregion

        private void Awake()
        {
            int randomIndex = UnityEngine.Random.Range(0, canSelectNone ? transform.childCount + 1 : transform.childCount);

            if (randomIndex < transform.childCount)
                transform.GetChild(randomIndex).gameObject.SetActive(true);
        }
    }
}