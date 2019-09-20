using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Status.Interfaces;
using System;
using UnityEngine;

namespace Assets.GameFramework.Status.Core
{
    public abstract class StatusBase : IStatus
    {
        public StatusTypes Type;
        public int Current;
        public int Treshold;

        public StatusBase(StatusTypes type, int current, int treshold)
        {
            Type = type;
            Current = current;
            Treshold = treshold;
        }

        public virtual void UpdateStatus(int value)
        {
            Current += value;
            if (Current < Treshold)
                Debug.Log($"StatusBase --- Current value is lower than treshold --- ");
        }
    }
}
