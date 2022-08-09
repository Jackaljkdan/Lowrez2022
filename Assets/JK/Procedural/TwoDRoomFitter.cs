using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public class TwoDRoomFitter : RoomFitter
    {
        #region Inspector

        public LayerMask mask;

        [Header("Runtime")]

        public Room room;

        #endregion

        [NonSerialized]
        public List<Collider2D> colliders;

        public override void Awake()
        {
            colliders = new List<Collider2D>(8);
            GetComponentsInChildren(colliders);
        }

        private void Start()
        {
            if (!Application.isPlaying)
                return;

            room = GetComponentInParent<Room>();

            foreach (var coll in colliders)
            {
                var trigger = coll.gameObject.AddComponent<RoomFitterTrigger>();
                trigger.room = room;
            }

            Transform targetParent = Context.Find(this).Get<Transform>("room.fitters");
            transform.SetParent(targetParent, worldPositionStays: true);
        }

        private static Collider2D[] overlapBuffer = new Collider2D[1];

        public override bool IsFitting()
        {
            foreach (var collider in colliders)
            {
                int overlapping = collider.OverlapCollider(
                    new ContactFilter2D()
                    {
                        useLayerMask = true,
                        layerMask = mask
                    }, 
                    overlapBuffer
                );

                if (overlapping > 0)
                    return false;
            }

            return true;
        }
    }
}