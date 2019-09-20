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

        private StatusBase _instance;
        public StatusBase StatusInstance
        {
            get => _instance ?? InitializeStatusBaseType();
        }

        private StatusBase InitializeStatusBaseType()
        {
            switch (StatusType)
            {
                case StatusTypes.Hungry:
                    _instance = new HungryStatus(StatusType, Current, Treshold);
                    return _instance;

                default:
                    break;
            }

            return null;
        }

    }
}
