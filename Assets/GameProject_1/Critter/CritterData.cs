using UnityEngine;

namespace Assets.GameProject_1.Status
{
    [CreateAssetMenu(fileName = "CritterData", menuName = "GameProject_1/CritterData", order = 0)]
    public class CritterData : ScriptableObject
    {
        public string Name;
        public float EatingTime;
        public float Speed;
        public float Acceleration;
        public float StopingDistance;
    }
}