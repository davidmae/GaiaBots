using Assets.GameFramework.Item.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameFramework.Item.Core
{
    public class Consumable : MonoBehaviour, IConsumable
    {
        [SerializeField]
        public string Satiety;
    }
}
