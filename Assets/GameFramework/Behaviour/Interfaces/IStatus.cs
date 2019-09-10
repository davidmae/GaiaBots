using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Status.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.GameFramework.Behaviour.Interfaces
{
    public interface IActorBehaviour
    {
        void SetEvaluateStatusAction<T>(Action<BaseActor> evaluate) where T : StatusBase;
        void EvaluateStatus();
    }
}