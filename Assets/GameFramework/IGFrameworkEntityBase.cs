using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameFramework
{
    public interface IGFrameworkEntityBase
    {
        event Action OnUpdateEntity;

        void OnUpdate();
        void InvokeUpdateEntityOverTime(float time = 0f, float repeatRate = 1f);
        void CancelUpdateEntityOverTime();
        void DestroyEntity();
        void SetUpdateToNull();
    }
}
