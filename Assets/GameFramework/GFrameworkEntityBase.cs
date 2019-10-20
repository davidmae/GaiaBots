﻿using System;
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
            //if (OnUpdateEntity == null && OnUpdateEntity.Target == null)
            //    OnUpdateEntity = null;
        }

        public void SetUpdateToNull()
        {
            OnUpdateEntity = null;
            CancelInvoke("OnUpdate");
        }

        public void DestroyEntity()
        {
            Destroy(gameObject);
        }

        public Action GetOnUpdateVal()
        {
            return OnUpdateEntity;
        }
        
    }
}
