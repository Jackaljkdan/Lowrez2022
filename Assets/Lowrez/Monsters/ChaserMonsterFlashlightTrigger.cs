using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.Monsters
{
    [DisallowMultipleComponent]
    public class ChaserMonsterFlashlightTrigger : MonoBehaviour
    {
        #region Inspector

        public ChaserMonster target;

        public float aggroRangeMultiplier = 2;

        #endregion

        private void Start()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            target.aggroRange *= aggroRangeMultiplier;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            target.aggroRange /= aggroRangeMultiplier;
        }
    }
}