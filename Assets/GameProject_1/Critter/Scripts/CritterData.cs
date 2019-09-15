using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Assets.GameProject_1.Critter.Scripts;
using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Status.Core;
using System;

namespace Assets.GameProject_1.Status.Scripts
{
    [Serializable]
    public struct CritterData
    {
        public string Name;
        public float EatingTime;
    }
}