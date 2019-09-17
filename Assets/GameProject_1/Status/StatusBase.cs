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
        private StatusData Data = new StatusData();

        public StatusBase(StatusData.StatusTypes type, int current, int treshold)
        {
            Data.Type = type;
            Data.Current = current;
            Data.Treshold = treshold;
        }

        public override void UpdateStatus(int value)
        {
            Data.Current += value;
            if (Data.Current < Data.Treshold)
                Debug.Log($"StatusBase::{Data.name} --- Current value is lower than treshold --- ");
        }
    }
}
