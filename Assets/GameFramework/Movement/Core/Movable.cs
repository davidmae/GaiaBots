using Assets.GameFramework.Behaviour.Interfaces;
using Assets.GameProject_1.Status;
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
        public MyNavigator Navigation { get; set; }
        public CritterData CritterData { get; set; }


        public void SetNextTarget(float x, float y, float z)
             => Target = new Vector3(x, 0, z);

        public void SetNextTarget(Vector3 position)
            => SetNextTarget(position.x, 0, position.z);

        public void SetNextTarget(Transform transform)
            => SetNextTarget(transform.position);

        public bool ArrivedToPosition(Vector3 position, float stoppingDistance)
        {
            //Debug.Log(Vector3.Distance(position, Target));
            //Debug.Log(stoppingDistance * 1.5f);
            return Vector3.Distance(position, Target) <= stoppingDistance * 1.5f; //(position - Target).sqrMagnitude < stoppingDistance * stoppingDistance;
        }

        public virtual void NavigateToTarget() => Navigation.SetDestination(Target);



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
