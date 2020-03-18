using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.GameFramework.Movement.Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MyNavigator : MonoBehaviour
    {
        NavMeshAgent navigator;

        public MyNavigator() { }
        public MyNavigator(NavMeshAgent navigator, Action<NavMeshAgent> navoptions)
        {
            this.navigator = navigator;
            navoptions(this.navigator);
        }

        public virtual void Restart() => navigator.isStopped = false;
        public virtual void Stop() => navigator.isStopped = true;
        public virtual void SetDestination(Vector3 target) => navigator.SetDestination(target);
        public virtual void SetSpeed(float speed) => navigator.speed = speed;

        public NavMeshAgent NavigatorProps { get => navigator; }
    }
}
