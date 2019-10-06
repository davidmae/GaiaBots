using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.GameFramework.Movement.Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MyNavigator : MonoBehaviour
    {
        NavMeshAgent navigator;

        public MyNavigator(NavMeshAgent navigator, Action<NavMeshAgent> navoptions)
        {
            this.navigator = navigator;
            navoptions(this.navigator);
        }

        public void Restart() => navigator.isStopped = false;
        public void Stop() => navigator.isStopped = true;
        public void SetDestination(Vector3 target) => navigator.SetDestination(target);

        public NavMeshAgent NavigatorProps { get => navigator; }
    }
}
