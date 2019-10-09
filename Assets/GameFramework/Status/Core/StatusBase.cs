using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Item.Interfaces;
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

        public StatusBase() { }

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
        public virtual StatusBase GetStatusFrom(IConsumable consumable) => this;

        public virtual void UpdateStatus(int value)
        {
            Current += value;
            if (Current < Treshold)
                Debug.Log($"StatusBase --- Current value is lower than treshold --- ");
        }

        public virtual int UpdateStatus(int value, string opt = "")
        {
            UpdateStatus(value);
            return Current;
        }

    }
}
