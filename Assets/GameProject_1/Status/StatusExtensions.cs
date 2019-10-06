using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.GameFramework.Status.Core;
using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Item.Core;

namespace Assets.GameProject_1.Status
{
    public static class StatusExtensions
    {
        public static IDictionary<StatusTypes, StatusBase> InitializeStatusInstancesFromStatusData(this List<StatusData> statusData)
        {
            IDictionary<StatusTypes, StatusBase> statusInstances = new Dictionary<StatusTypes, StatusBase>();

            foreach (var data in statusData)
                statusInstances.Add(data.StatusType, data.StatusInstance);

            return statusInstances;
        }

        //TODO
        public static Action PlusOnePointToActor(this IDictionary<StatusTypes, StatusBase> statusDictionary, ActorBase actor, IConsumable consumable)
        {
            //foreach (var status in statusDictionary)
            //{
            //    status.Value.ParseConsumable(consumable);
            //}

            if (consumable is Consumable<HungryStatus>)
                return actor.PlusOnePointToActor<HungryStatus>(consumable);

            else if (consumable is Consumable<HealthStatus>)
                return actor.PlusOnePointToActor<HealthStatus>(consumable);

            return null;
        }

        //TODO
        public static StatusBase GetStatusFromConsumableType(this IDictionary<StatusTypes, StatusBase> statusDictionary, IConsumable consumable)
        {
            //foreach (var status in statusDictionary)
            //{
            //    status.Value.ParseConsumable(consumable);
            //}
            if (consumable == null)
                return null;

            if (consumable is Consumable<HungryStatus>)
                return new HungryStatus();

            if (consumable is Consumable<HealthStatus>)
                return new HealthStatus();

            return null;
        }
    }
}
