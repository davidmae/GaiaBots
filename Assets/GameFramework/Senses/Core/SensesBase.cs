using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameFramework.Senses.Core
{
    public abstract class SensesBase
    {
        public abstract void Detect(ActorBase actor, IDetectable detectable);
    }
}
