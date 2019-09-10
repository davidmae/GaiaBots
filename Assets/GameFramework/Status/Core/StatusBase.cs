using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Status.Interfaces;
using System;

namespace Assets.GameFramework.Status.Core
{
    public class StatusBase : IStatus
    {
        public int Value { get; set; }
        public int Limit { get; set; }


        public event Action<BaseActor> onStatusChange;

        public StatusBase(int value, int limit)
        {
            Value = value;
            Limit = limit;
        }

        public void EvaluateStatus(BaseActor actor)
        {
            if (onStatusChange != null)
            {
                onStatusChange(actor);
            }

        }
    }
}
