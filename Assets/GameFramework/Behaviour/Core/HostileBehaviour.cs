using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileBehaviour : IBehaviour
{
    private int hungry;
    public event Action<int> onHungry;

    public HostileBehaviour(int hungry)
    {
        this.hungry = hungry;
    }

    public void CheckCurrentStats(int hungry)
    {
        if (onHungry != null)
        {
            onHungry(hungry);
        }


        //if (hungry < 50)
        //{
        //    Debug.Log(this.hungry);
        //}
    }

}
