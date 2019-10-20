using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Senses.Core;
using Assets.GameProject_1.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameProject_1.Senses
{
    public class RadiusSensor : RadiusSense
    {
        private void Awake()
        {
            Actor = GetComponentInParent<ActorBase>();
        }
    }
}
