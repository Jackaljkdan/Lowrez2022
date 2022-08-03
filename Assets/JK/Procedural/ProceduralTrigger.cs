using JK.Injection;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public class ProceduralTrigger : MonoBehaviour
    {
        #region Inspector



        #endregion

        private ProceduralRoomCreator roomCreator;

        private void Awake()
        {
            roomCreator = Context.Find(this).Get<ProceduralRoomCreator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponentInParent(out RoomConnection connection) && connection.Connected == null)
                roomCreator.TryCreateRoomForConnection(connection, out _);
        }
    }
}