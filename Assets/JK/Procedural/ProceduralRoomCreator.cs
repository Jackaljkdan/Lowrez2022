using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public class ProceduralRoomCreator : MonoBehaviour
    {
        #region Inspector

        public List<Room> roomPrefabs;

        [Header("Debug")]

        public Room dbgTarget;

        [ContextMenu("Create rooms adjacent to dbgTarget")]
        private void CreateRoomsInEditMode()
        {
            dbgTarget.Awake();
            CreateRoomsAdjacentTo(dbgTarget);
        }

        #endregion

        private void Start()
        {
            
        }

        public void CreateRoomsAdjacentTo(Room room)
        {
            foreach (Room _ in CreateAndEnumerateRoomsAdjacentTo(room)) ;
        }

        public IEnumerable<Room> CreateAndEnumerateRoomsAdjacentTo(Room room)
        {
            if (room == null)
            {
                yield return InstantiateRandomRoom();
                yield break;
            }

            foreach (var connection in room.Connections)
            {
                if (connection.Connected != null)
                    continue;

                Room instance = InstantiateRandomRoom();
                instance.GetRandomConnection().ConnectAndMoveSelf(connection);
                yield return instance;
            }

        }

        private Room InstantiateRandomRoom()
        {
            int randomIndex = UnityEngine.Random.Range(0, roomPrefabs.Count);
            Room instance = Instantiate(roomPrefabs[randomIndex], transform);
            return instance;
        }
    }
}