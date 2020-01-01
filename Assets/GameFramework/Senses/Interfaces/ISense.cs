using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameFramework.Senses.Interfaces
{
    public interface ISense
    {
        void Detect(ActorBase actor = null, IDetectable detectable = null);
        void Stop();
        void Restart();
    }
}
