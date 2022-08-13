using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.Monsters
{
    [DisallowMultipleComponent]
    public class DisablerOnEndGame : EndgameListener
    {
        #region Inspector

        public Behaviour target;

        #endregion

        protected override void OnEndGame(bool win)
        {
            target.enabled = false;
        }
    }
}