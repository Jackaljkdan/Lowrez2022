using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public class RoomFitterTrigger : MonoBehaviour
    {
        #region Inspector

        [Header("Runtime")]

        public Room room;

        #endregion
    }
}