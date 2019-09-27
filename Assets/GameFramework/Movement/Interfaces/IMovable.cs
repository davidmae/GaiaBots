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
        void SetNextTarget(float x, float y, float z);
        void SetNextTarget(Vector3 position);
        void SetNextTarget(Transform transform);
        void NavigateToTarget();
        bool ArrivedToPosition(Vector3 position, float stoppingDistance);
    }
}
