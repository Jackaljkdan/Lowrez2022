using JK.Utils;
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

        [Header("Runtime")]

        [SerializeField]
        private bool isInEffect;

        #endregion

        private void Start()
        {
            
        }

        private static RaycastHit2D[] buffer = new RaycastHit2D[2];

        private bool IsInLineOfSightFrom(Vector3 position)
        {
            int hitCount = Physics2D.Raycast(
                origin: position,
                direction: transform.position - position,
                new ContactFilter2D()
                {
                    useLayerMask = true,
                    layerMask = 1 | (1 << gameObject.layer),
                    useTriggers = true,
                },
                buffer
            );

            bool isHit = hitCount >= 2 && buffer[1].collider.TryGetComponent(out ChaserMonsterFlashlightTrigger other) && other == this;

            return isHit;
        }


        private void OnTriggerStay2D(Collider2D collision)
        {
            bool isInLineOfSight = IsInLineOfSightFrom(collision.transform.position);
            
            if (isInLineOfSight && !isInEffect)
            {
                isInEffect = true;
                target.aggroRange *= aggroRangeMultiplier;
            }
            else if (!isInLineOfSight && isInEffect)
            {
                OnTriggerExit2D(collision);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (isInEffect)
            {
                target.aggroRange /= aggroRangeMultiplier;
                isInEffect = false;
            }
        }
    }
}