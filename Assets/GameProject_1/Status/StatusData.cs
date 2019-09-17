using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Status.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.GameProject_1.Status
{
    [CreateAssetMenu(fileName = "StatusData", menuName = "GameProject_1/StatusData", order = 0)]
    public class StatusData : ScriptableObject
    {
        public enum StatusTypes
        {
            Hungry = 0,
            Rage = 1
            //...
        }

        public StatusTypes Type;
        public int Current;
        public int Treshold;

        private StatusBase _instance;
        public StatusBase Status
        {
            get => _instance ?? InitializeStatusBaseType();
        }

        private StatusBase InitializeStatusBaseType()
        {
            switch (Type)
            {
                case StatusTypes.Hungry:
                    _instance = new HungryStatus(Type, Current, Treshold);
                    return _instance;

                //case StatusTypes.Rage:
                //    new RageStatus(Type, Current, Treshold);
                //    break;

                default:
                    break;
            }

            return null;
        }

    }
}
