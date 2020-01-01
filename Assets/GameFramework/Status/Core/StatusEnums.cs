using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.GameFramework.Status.Core
{
    public enum StatusTypes
    {
        Undefined = -1,
        Hungry = 0,
        Rage = 1,
        Defecate = 2,
        Health = 3,
        Libido = 4
        //...
    }

    // Estados como "hambre" se actualizarian como decrementalmente ya que el "hambre" decrece con el tiempo
    // Estados como "libido" se actualizarian como incrementalmente ya que el "libido" aumenta con el tiempo
    public enum StatusUpdateMethod
    {
        Incremental = 0,
        Decremental = 1
    }
}
