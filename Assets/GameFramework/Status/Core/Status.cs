using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Status.Interfaces;
using System;
using UnityEngine;

namespace Assets.GameFramework.Status.Core
{
    [Serializable]
    public abstract class Status : IStatus
    {
        public virtual void UpdateStatus(int value) { }
    }
}
