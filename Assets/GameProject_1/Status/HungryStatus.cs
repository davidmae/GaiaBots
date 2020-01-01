using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Senses.Core;
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
        public HungryStatus() { }
        public HungryStatus(StatusTypes type) : base(type) { }
        public HungryStatus(StatusTypes type, StatusUpdateMethod updateMethod, int current, int treshold, int maxvalue) : base(type, current, treshold, maxvalue)
        {
            UpdateMethod = updateMethod;
        }

        public override StatusBase GetStatusFrom(IConsumable consumable) => consumable is Consumable<HungryStatus> ? this : null;

        public override void UpdateStatus(int value)
        {
            base.UpdateStatus(value);
            if (Current < Treshold)
                Debug.Log($" ------ TENGO HAMBRE !!!!! ------ ");
        }

    }
}
