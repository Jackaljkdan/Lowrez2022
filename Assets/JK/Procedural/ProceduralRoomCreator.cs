using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public abstract class ProceduralRoomCreator : MonoBehaviour
    {
        #region Inspector

        [Header("Debug")]

        public Room dbgTarget;

        [ContextMenu("Create rooms adjacent to dbgTarget")]
        private void CreateRoomsInEditMode()
        {
            dbgTarget.Awake();
            CreateRoomsAdjacentTo(dbgTarget);
        }

        #endregion

        public void CreateRoomsAdjacentTo(Room room)
        {
            foreach (Room _ in CreateAndEnumerateRoomsAdjacentTo(room)) ;
        }

        public IEnumerable<Room> CreateAndEnumerateRoomsAdjacentTo(Room room)
        {
            if (room == null)
                yield break;

            foreach (var connection in room.Connections)
            {
                if (connection.Connected != null)
                    continue;

                if (TryInstantiatingFittingRoomConnectedTo(connection, out Room instance))
                    yield return instance;
            }
        }

        public bool TryCreateRoomForConnection(RoomConnection connection, out Room room)
        {
            if (connection.Connected != null)
            {
                room = connection.Connected.GetRoom();
                return false;
            }

            return TryInstantiatingFittingRoomConnectedTo(connection, out room);
        }

        private bool TryInstantiatingFittingRoomConnectedTo(RoomConnection connection, out Room instance)
        {
            foreach (var prefab in EnumerateRoomPrefabsForInstantiation())
            {
                instance = InstantiateRoom(prefab);

                instance.Connections.ShuffleInPlace();

                foreach (var instanceConnection in instance.Connections)
                {
                    instanceConnection.MoveTo(connection);
                    Physics2D.SyncTransforms();

                    if (instance.IsFittingCurrentPosition())
                    {
                        instanceConnection.Connect(connection);
                        return true;
                    }
                }

                instance.gameObject.SetActive(false);

                if (PlatformUtils.IsEditor && !Application.isPlaying)
                    DestroyImmediate(instance.gameObject);
                else
                    Destroy(instance.gameObject);
            }

            FillUnfittableConnection(connection);

            instance = null;
            return false;
        }

        public abstract IEnumerable<Room> EnumerateRoomPrefabsForInstantiation();

        public abstract void FillUnfittableConnection(RoomConnection connection);

        private Room InstantiateRoom(Room prefab)
        {
            Room instance = Instantiate(prefab, transform);
            return instance;
        }
    }
}