using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.GameProject_1.Status
{
    public abstract class StatusBase : GameFramework.Status.Core.Status
    {
        public StatusData.StatusTypes Type;
        public int Current;
        public int Treshold;

        public StatusBase(StatusData.StatusTypes type, int current, int treshold)
        {
            Type = type;
            Current = current;
            Treshold = treshold;
        }

        public override void UpdateStatus(int value)
        {
            Current += value;
            if (Current < Treshold)
                Debug.Log($"StatusBase --- Current value is lower than treshold --- ");
        }
    }
}
