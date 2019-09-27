using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameFramework.Behaviour.Core
{
    [Serializable]
    public class States
    {
        public bool moving = false;

        public bool goToEat = false;
        public bool goToFight = false;
        public bool escaping = false;

        public bool eating = false;
        public bool fighting = false;


        public bool IsMoving => moving == true;
        public bool IsGoingToEat => goToEat == true;
        public bool IsGoingToFight => goToFight == true;
        public bool IsEating => eating == true;
        public bool IsFighting => fighting == true;
    }
}
