
using Assets.GameFramework.Actor.Core;
using UnityEngine;

namespace Assets.GameFramework.Common
{
    public interface IDetectable : IGFrameworkEntityBase
    {
        void Detect(ActorBase currentActor);
        Vector3 GetPosition();
        GameObject GetGameObject();
    }
}
