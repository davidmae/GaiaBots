using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameFramework
{
    public abstract class GFrameworkEntityBase : MonoBehaviour, IGFrameworkEntityBase
    {
        public event Action OnUpdateEntity;

        public void OnUpdate()
        {
            if (OnUpdateEntity != null)
                OnUpdateEntity();
        }

        public void InvokeUpdateEntityOverTime(float time = 0f, float repeatRate = 1f)
        {
            if (!IsInvoking("OnUpdate"))
                InvokeRepeating("OnUpdate", time, repeatRate);
        }

        public void CancelUpdateEntityOverTime()
        {
            CancelInvoke("OnUpdate");
        }

        public void DestroyEntity()
        {
            Destroy(gameObject);
        }
    }
}
