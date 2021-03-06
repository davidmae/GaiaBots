﻿using Assets.GameFramework.Common;
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
        event Action OnUpdateSatiety;

        int GetSacietyPoints();
        int MinusOneSacietyPoint();
    }

}
