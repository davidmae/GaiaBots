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
    public class HungryStatus : StatusBase
    {
        public HungryStatus(StatusTypes type, int current, int treshold, int maxvalue) : base(type, current, treshold, maxvalue) { }

        public override void UpdateStatus(int value)
        {
            base.UpdateStatus(value);
            if (Current < Treshold)
                Debug.Log($" ------ TENGO HAMBRE !!!!! ------ ");
        }

    }
}
