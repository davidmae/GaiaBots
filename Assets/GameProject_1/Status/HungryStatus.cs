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
        public HungryStatus(int value, int limit) : base(value, limit) { }

        public void IsHungry(BaseActor actor)
        {
            if (Value < Limit)
            {
                Debug.Log($"{actor.Name} is hungry!!");
            }
        }

    }
}
