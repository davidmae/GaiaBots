using Assets.GameFramework.Behaviour.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.GameFramework.Movement.Core
{
    public class Movable : IMovable
    {
        public Vector3 Target { get; set; }
        public NavMeshAgent Navigator { get; set; }

        public void SetNextTarget(float x, float y, float z)
             => Target = new Vector3(x, 0, z);

        public void SetNextTarget(Vector3 position)
            => SetNextTarget(position.x, 0, position.z);

        public void SetNextTarget(Transform transform)
            => SetNextTarget(transform.position);

        public bool ArrivedToPosition(Vector3 position, float stoppingDistance)
            => (position - Target).sqrMagnitude < stoppingDistance * stoppingDistance;

        public virtual void NavigateToTarget() => Navigator.SetDestination(Target);



        // ------------------  Out of IMovable ------------------

        public virtual void MoveToPosition(Vector3 position = new Vector3())
        {
            if (position != Vector3.zero)
            {
                SetNextTarget(position);
                NavigateToTarget();
            }
        }
    }
}
