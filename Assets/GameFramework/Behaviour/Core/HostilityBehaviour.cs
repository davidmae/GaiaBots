using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameFramework.Behaviour.Core
{
    public class HostilityBehaviour
    {
        public int Value;
        public float IncrementSpeedFactor_MED = 2f;
        public float IncrementSpeedFactor_MAX = 2.5f;

        const int KHostilityTresholdMIN = 0;
        const int KHostilityTresholdMED = 20;
        const int KHostilityTresholdMAX = 80;

        public HostilityBehaviour() { }

        public HostilityBehaviour(int value, float incrmed, float incrmax)
        {
            Value = value;
            IncrementSpeedFactor_MED = incrmed;
            IncrementSpeedFactor_MAX = incrmax;
        }

        //TODO: Mover a otro sitio
        public bool OutOfSight(int value, float currDistance, float sensorDistance) =>
            currDistance > SightDistance(value, sensorDistance);


        public float SightDistance(float value, float sensorDistance) =>
            value > KHostilityTresholdMAX ? sensorDistance :
            value > KHostilityTresholdMED ? sensorDistance * .6f :
            value > KHostilityTresholdMIN ? sensorDistance * .3f : 0f;

        public float SecondsStandTo(float value) =>
            value > KHostilityTresholdMAX ? 0 : 10 - (value * .1f);

        // Ej:
        // velocidad + 0.0033(valor-20) (*)
        // (*) (0.0033 = 1.2 - 1 / 80-20)
        public float IncrSpeed(float incrFactor, float initializedSpeed, int value_max, int value_min, int valueCurrent) =>
            ((incrFactor * initializedSpeed) - initializedSpeed) / (value_max - value_min) * (valueCurrent - value_min);

        public float SpeedToFight(int value, float speed) =>
            value > KHostilityTresholdMAX ? speed + IncrSpeed(IncrementSpeedFactor_MED, speed, KHostilityTresholdMAX, KHostilityTresholdMED, value)
                                                  + IncrSpeed(IncrementSpeedFactor_MAX, speed, 100                  , KHostilityTresholdMAX, value) :

            value > KHostilityTresholdMED ? speed + IncrSpeed(IncrementSpeedFactor_MED, speed, KHostilityTresholdMAX, KHostilityTresholdMED, value) :
            value > KHostilityTresholdMIN ? speed : speed;


        //public float HostilitySpeedToFight(int hostility, float speed) =>
        //    hostility > 80 ? (speed + .033f * (80 - 20)) + .032f * (hostility - 80) :
        //    hostility > 20 ? speed + .033f * (hostility - 20) :
        //    hostility > 0 ? speed : speed;
        //
    }
}
