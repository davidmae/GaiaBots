using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Senses.Core;
using Assets.GameFramework.Senses.Interfaces;
using Assets.GameFramework.Status.Interfaces;
using System;
using UnityEngine;

namespace Assets.GameFramework.Status.Core
{
    [Serializable]
    public class StatusBase : IStatus
    {
        public StatusTypes Type;
        public StatusUpdateMethod UpdateMethod;
        public float Current;
        public float Treshold;
        public float MaxValue;

        public StatusBase() { }

        public StatusBase(StatusTypes type, int current, int treshold) : this(type, current, treshold, 0)
        {
        }

        public StatusBase(StatusTypes type) : this(type, 0, 0, 0)
        {
        }

        public StatusBase(StatusTypes type, int current, int treshold, int maxvalue)
        {
            Type = type;
            Current = current;
            Treshold = treshold;
            MaxValue = maxvalue;
        }
        public virtual StatusBase GetStatusFrom(IConsumable consumable) => this;

        public virtual void UpdateStatus(int value)
        {
            Current = Mathf.Clamp(Current + value, 0, MaxValue);
            if (Current < Treshold)
                Debug.Log($"StatusBase --- Current value is lower than treshold --- ");
        }

        public virtual void UpdateStatus(float value)
        {
            Current = Mathf.Clamp(Current + value, 0, MaxValue);
            if (Current < Treshold)
                Debug.Log($"StatusBase --- Current value is lower than treshold --- ");
        }

        public virtual void UpdateStatus(IConsumable consumable, RadiusSense senseFrom)
        {
            var bonus = consumable.GetBonusValue();

            var ratio = MaxValue / senseFrom.MaxDistance;
            ratio = (float)Math.Ceiling(ratio * 100) / 100;

            senseFrom.Distance = Mathf.Clamp(senseFrom.Distance - (bonus / ratio), 0, senseFrom.MaxDistance);

            if (UpdateMethod == StatusUpdateMethod.Incremental)
                bonus = -1 * bonus;

            UpdateStatus(bonus);
        }

        public virtual void UpdateStatus(RadiusSense senseFrom)
        {
            senseFrom.Distance++;

            var ratio = MaxValue / senseFrom.MaxDistance;
            ratio = (float)Math.Ceiling(ratio * 100) / 100;

            if (UpdateMethod == StatusUpdateMethod.Decremental)
                ratio = -1 * ratio;

            UpdateStatus(ratio);
        }

        public virtual bool LimitReached() => UpdateMethod == StatusUpdateMethod.Decremental ? Current <= 0 : Current >= MaxValue;

    }
}
