using Assets.GameFramework.Status.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.GameProject_1.Status
{
    [CreateAssetMenu(fileName = "StatusData", menuName = "GameProject_1/StatusData", order = 0)]
    public class StatusData : ScriptableObject
    {
        public StatusTypes StatusType;
        public StatusUpdateMethod UpdateMethod;
        public int Current;
        public int Treshold;
        public int MaxValue;

        public StatusBase StatusInstance
        {
            get => InitializeStatusBaseType();
        }

        private StatusBase InitializeStatusBaseType()
        {
            switch (StatusType)
            {
                case StatusTypes.Hungry:
                    return new HungryStatus(StatusType, UpdateMethod, Current, Treshold, MaxValue);

                case StatusTypes.Rage:
                    return new RageStatus(StatusType, UpdateMethod, Current, Treshold);

                case StatusTypes.Health:
                    return new HealthStatus(StatusType, UpdateMethod, Current, Treshold, MaxValue);

                case StatusTypes.Libido:
                    return new LibidoStatus(StatusType, UpdateMethod, Current, Treshold, MaxValue);

                default:
                    break;
            }

            return null;
        }

    }
}
