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

        public bool goToItem = false;
        public bool goToFight = false;
        public bool escaping = false;

        public bool stayFront = false;
        public bool fighting = false;


        public bool IsMoving => moving == true;
        public bool IsGoingToItem => goToItem == true;
        public bool IsGoingToFight => goToFight == true;
        public bool IsStayFront => stayFront == true;
        public bool IsFighting => fighting == true;



    }
}
