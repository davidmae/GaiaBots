
using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Senses.Core;
using UnityEngine;

namespace Assets.GameFramework.Common
{
    public interface IDetectable : IGFrameworkEntityBase
    {
        bool Detect(ActorBase currentActor, SenseBase senseFrom);
        Vector3 GetPosition();
        GameObject GetGameObject();
    }
}
