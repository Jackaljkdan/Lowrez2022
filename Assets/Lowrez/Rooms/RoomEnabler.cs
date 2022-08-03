using JK.Procedural;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.Rooms
{
    [DisallowMultipleComponent]
    public class RoomEnabler : MonoBehaviour
    {
        #region Inspector



        #endregion

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponentInParent(out Room room))
                room.gameObject.SetActive(true);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponentInParent(out Room room))
                room.gameObject.SetActive(false);
        }
    }
}