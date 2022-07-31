using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    public abstract class RoomFitter : MonoBehaviour
    {
        #region Inspector

        [ContextMenu("Log is fitting")]
        public void LogIsFittingInEditMode()
        {
            Awake();
            Debug.Log("is fitting: " + IsFitting());
        }

        #endregion

        public virtual void Awake()
        {

        }

        public abstract bool IsFitting();
    }
}