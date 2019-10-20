using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Status.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameProject_1.Status
{
    public class RageStatus : StatusBase
    {
        public RageStatus() { }
        public RageStatus(StatusTypes type) : base(type) { }
        public RageStatus(StatusTypes type, int current, int treshold) : base(type, current, treshold) { }
        public override StatusBase GetStatusFrom(IConsumable consumable) => consumable is Consumable<RageStatus> ? this : null;

        public override void UpdateStatus(int value)
        {
            base.UpdateStatus(value);
            if (Current < Treshold)
                Debug.Log($" ------ QUE ASSCO OSTIA YA !!!!! ------ ");
        }

    }
}
