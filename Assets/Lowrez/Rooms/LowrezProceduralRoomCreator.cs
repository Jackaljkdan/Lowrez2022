using JK.Injection;
using JK.Procedural;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.Rooms
{
    [DisallowMultipleComponent]
    public class LowrezProceduralRoomCreator : ProceduralRoomCreator
    {
        #region Inspector

        public List<RoomsForDistance> roomsForDistances;

        public Transform wallFillerPrefab;

        #endregion

        [Serializable]
        public struct RoomsForDistance
        {
            public float distance;
            public List<Room> prefabs;
        }

        private Transform playerTransform;

        private Vector3 initialPosition;

        private void Awake()
        {
            playerTransform = Context.Find(this).Get<Transform>("player");
            initialPosition = playerTransform.position;

            roomsForDistances = roomsForDistances.OrderByDescending(entry => entry.distance).ToList();
        }

        public override IEnumerable<Room> EnumerateRoomPrefabsForInstantiation()
        {
            float playerDistance = Vector3.Distance(playerTransform.position, initialPosition);

            foreach (var entry in roomsForDistances)
            {
                if (playerDistance < entry.distance)
                    continue;

                entry.prefabs.ShuffleInPlace();

                foreach (var prefab in entry.prefabs)
                    yield return prefab;

                yield break;
            }
        }

        public override void FillUnfittableConnection(RoomConnection connection)
        {
            Transform wallFiller = Instantiate(wallFillerPrefab, connection.transform);
            wallFiller.localPosition = Vector3.zero;
            wallFiller.localEulerAngles = Vector3.zero;
        }
    }
}