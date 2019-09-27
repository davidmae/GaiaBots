using Assets.GameFramework.Status.Core;
using UnityEngine;


namespace Assets.GameProject_1.Status
{
    [CreateAssetMenu(fileName = "StatusData", menuName = "GameProject_1/StatusData", order = 0)]
    public class StatusData : ScriptableObject
    {
        public StatusTypes StatusType;
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
                    return new HungryStatus(StatusType, Current, Treshold, MaxValue);

                case StatusTypes.Rage:
                    return new RageStatus(StatusType, Current, Treshold);

                case StatusTypes.Defecate:
                    return new DefecateStatus(StatusType, Current, Treshold);

                default:
                    break;
            }

            return null;
        }

    }
}
