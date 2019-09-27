using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Status.Interfaces;
using System;
using UnityEngine;

namespace Assets.GameFramework.Status.Core
{
    [Serializable]
    public class StatusBase : IStatus
    {
        public StatusTypes Type;
        public int Current;
        public int Treshold;
        public int? MaxValue;

        public StatusBase(StatusTypes type, int current, int treshold) : this(type, current, treshold, null)
        {
        }

        public StatusBase(StatusTypes type, int current, int treshold, int? maxvalue)
        {
            Type = type;
            Current = current;
            Treshold = treshold;
            MaxValue = maxvalue;
        }

        public virtual void UpdateStatus(int value)
        {
            Current += value;
            if (Current < Treshold)
                Debug.Log($"StatusBase --- Current value is lower than treshold --- ");
        }
    }
}
