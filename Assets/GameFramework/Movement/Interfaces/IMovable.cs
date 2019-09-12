using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.GameFramework.Behaviour.Interfaces
{
    public interface IMovable
    {
        //TODO: Cambiar
        NavMeshAgent Navigator { get; set; }
        Vector3 Target { get; set; }
        void SetNextTarget(float x, float y, float z);
        void SetNextTarget(Vector3 position);
        void SetNextTarget(Transform transform);
        void MoveToTarget();
    }
}
