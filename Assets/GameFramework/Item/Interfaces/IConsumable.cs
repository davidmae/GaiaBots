using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Status.Core;
using Assets.GameFramework.Status.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameFramework.Item.Interfaces
{
    public interface IConsumable : IItem, IDetectable
    {
        StatusBase StatusModified { get; set; }

        event Func<StatusBase> OnGetStatusModified;
        StatusBase GetStatusModified();
        int GetCurrentPoints();
        int MinusOnePoint();
    }
}
