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

        private Dictionary<Room, int> refenceCount;

        private void Awake()
        {
            refenceCount = new Dictionary<Room, int>(8);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out RoomFitterTrigger trigger))
            {
                trigger.room.gameObject.SetActive(true);

                if (refenceCount.ContainsKey(trigger.room))
                    refenceCount[trigger.room] += 1;
                else
                    refenceCount[trigger.room] = 1;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out RoomFitterTrigger trigger))
            {
                if (!refenceCount.TryGetValue(trigger.room, out int count))
                    count = 0;

                if (count > 1)
                {
                    refenceCount[trigger.room] -= 1;
                }
                else
                {
                    refenceCount[trigger.room] = 0;
                    trigger.room.gameObject.SetActive(false);
                }
            }
        }
    }
}