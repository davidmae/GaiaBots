using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehaviour
{
    event Action<int> onHungry;
    void CheckCurrentStats(int hungry);
}
