using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameFramework
{
    public interface IGFrameworkEntityBase : IEquatable<IGFrameworkEntityBase>
    {
        event Action OnUpdateEntity;

        void OnUpdate();
        void InvokeUpdateEntityOverTime(float time = 0f, float repeatRate = 1f);
        void CancelUpdateEntityOverTime();
        void DestroyEntity();
        void OnUpdateEntityToNull();

        string GetOriginalName();
        GameObject GetGameObject();
        List<KeyValuePair<string, object>> GetEntityFields();
        GFrameworkEntityBase DeepCopy();
        GFrameworkEntityBase DeepCopy(GFrameworkEntityBase entity);
    }
}
