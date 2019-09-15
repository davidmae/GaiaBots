using Assets.GameFramework.Behaviour.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.GameFramework.Behaviour.Core
{
    //[RequireComponent(typeof(NavMeshAgent))]
    public class MovableAI : IMovable
    {
        public NavMeshAgent Navigator { get; set; }
        public Vector3 Target { get; set; }

        public void SetNextTarget(float x, float y, float z)
            => Target = new Vector3(x, 0, z);

        public void SetNextTarget(Vector3 position)
            => SetNextTarget(position.x, 0, position.z);

        public void SetNextTarget(Transform transform) 
            => SetNextTarget(transform.position);

        public void MoveToTarget()
            => Navigator.SetDestination(Target);

    }
}
