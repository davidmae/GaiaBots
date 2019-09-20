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
        bool ArrivedToPosition(Vector3 position, float stoppingDistance);
        void MoveToPosition(Vector3 position);

        //void SetEvaluateStatusAction<T>(Action<ActorBase> evaluate) where T : StatusBase;
        //void EvaluateStatus();
    }
}