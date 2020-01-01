using UnityEngine;

namespace Assets.GameProject_1.Status
{
    public enum CritterSpecie { Penguin, Dog, Cat }

    [CreateAssetMenu(fileName = "CritterData", menuName = "GameProject_1/CritterData", order = 0)]
    public class CritterData : ScriptableObject
    {
        public CritterSpecie Specie;
        public string Name;
        public float Speed;
        public float Acceleration;
        public float StopingDistance;
        public HostilityData Hostility;
    }

    //[CreateAssetMenu(fileName = "CritterDataHostility", menuName = "GameProject_1/CritterDataHostility", order = 1)]
    //public class CritterDataWithHostility : CritterData
    //{
    //    public HostilityData Hostility;
    //}

    [CreateAssetMenu(fileName = "HostilityData", menuName = "GameProject_1/HostilityData", order = 1)]
    public class HostilityData : ScriptableObject
    {
        [Range(0, 100)]
        public int Value;
        public float IncrementSpeedFactor_MED = 2f;
        public float IncrementSpeedFactor_MAX = 2.5f;
    }
}