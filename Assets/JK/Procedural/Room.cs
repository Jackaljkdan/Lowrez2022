using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public class Room : MonoBehaviour
    {
        #region Inspector

        public Transform connectionsParent;

        public RoomFitter fitter;

        private void Reset()
        {
            connectionsParent = transform;
            fitter = GetComponentInChildren<RoomFitter>();
        }

        [ContextMenu("Log is fitting")]
        private void LogIsFittingInEditMode()
        {
            fitter.Awake();
            Debug.Log("is room fitting: " + IsFittingCurrentPosition());
        }

        #endregion

        public List<RoomConnection> Connections { get; private set; }

        public void Awake()
        {
            Connections = new List<RoomConnection>(8);
            connectionsParent.GetComponentsInChildren(Connections);
        }

        private void Start()
        {
            
        }

        public RoomConnection GetRandomConnection()
        {
            int index = UnityEngine.Random.Range(0, Connections.Count);
            return Connections[index];
        }

        public bool IsFittingCurrentPosition()
        {
            return fitter.IsFitting();
        }
    }
}