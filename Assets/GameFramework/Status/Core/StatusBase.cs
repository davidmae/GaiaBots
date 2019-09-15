using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Status.Interfaces;
using System;
using UnityEngine;

namespace Assets.GameFramework.Status.Core
{
    [Serializable]
    public class StatusBase : IStatus
    {
        public int Value { get; set; }
        public int Limit { get; set; }


        public event Action<ActorBase> onStatusChange;

        public StatusBase(int value, int limit)
        {
            Value = value;
            Limit = limit;
        }

        public void EvaluateStatus(ActorBase actor)
        {
            if (onStatusChange != null)
            {
                onStatusChange(actor);
            }

        }
    }
}
