using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public class RoomConnection : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private RoomConnection _connected;

        [ContextMenu("Move To Connected")]
        private void MoveToConnectedInEditMode()
        {
            // TODO: undo
            MoveTo(Connected);
        }

        [ContextMenu("Erase Connection")]
        private void EraseConnectionInEditMode()
        {
            // TODO: undo
            if (Connected != null)
            {
                Connected.Connected = null;
                Connected = null;
            }
        }

        #endregion

        public RoomConnection Connected
        {
            get => _connected;
            private set => _connected = value;
        }

        private void Start()
        {
            
        }

        public Room GetRoom()
        {
            return GetComponentInParent<Room>();
        }

        public void Connect(RoomConnection target)
        {
            Connected = target;
            target.Connected = this;
        }

        public void ConnectAndMoveSelf(RoomConnection target)
        {
            Connect(target);
            MoveTo(target);
        }

        public void MoveTo(RoomConnection target)
        {
            Room room = GetRoom();
            Transform targetTransform = target.transform;

            //Quaternion connectionRotation = Quaternion.LookRotation(-targetTransform.forward, targetTransform.up);
            //Quaternion rotationOffset = Quaternion.FromToRotation(room.transform.InverseTransformDirection(transform.forward), Vector3.forward);
            //room.transform.rotation = rotationOffset * connectionRotation;

            // https://answers.unity.com/questions/1408415/rotating-a-parent-object-to-achieve-a-specific-chi.html
            //Quaternion targetConnectionRotation = Quaternion.LookRotation(-targetTransform.forward, targetTransform.up);
            //Quaternion lookRotationVar = targetConnectionRotation * Quaternion.Inverse(transform.rotation);
            //lookRotationVar = lookRotationVar * room.transform.rotation;
            //room.transform.rotation = lookRotationVar;

            Quaternion targetConnectionRotation = Quaternion.LookRotation(-targetTransform.forward, targetTransform.up);
            room.transform.rotation = targetConnectionRotation * Quaternion.Inverse(transform.rotation) * room.transform.rotation;

            Vector3 positionOffset = room.transform.position - transform.position;
            room.transform.position = targetTransform.position + positionOffset;
        }

        public void ConnectAndMoveTarget(RoomConnection target)
        {
            target.ConnectAndMoveSelf(this);
        }
    }
}