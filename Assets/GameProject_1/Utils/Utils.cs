using Assets.GameFramework.Actor.Core;
using Assets.GameProject_1.Critter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameProject_1.Utils
{
    public static class Utils
    {
        public static string GetOriginalName(this GameObject go)
        {
            var index = go.name.IndexOf('(') - 1;
            return go.name.Substring(0, index < 0 ? go.name.Length : index);
        }

        public static void GiveBirthBabyCritter(this ActorBase mom, ActorBase dad)
        {
            if (mom.Genre == ActorBase.ActorGenre.Female)
            {
                var name = ((CritterBase)mom).critterData.Name;
                var childPrefab = Resources.Load($"Critters/{ name }") as GameObject;

                var child = GameObject.Instantiate(childPrefab.GetComponent<ActorBase>(), mom.transform.position, mom.transform.rotation);
                child.Genre = (ActorBase.ActorGenre)new System.Random().Next(0, 2);
                child.Dad = dad;
                child.Mom = mom;
                child.Generation = Math.Max(child.Dad.Generation, child.Mom.Generation) + 1;

                //todo
                child.transform.localScale = mom.transform.localScale * .5f;
            }
        }

        public static bool CanGiveBirth(this ActorBase actor, ActorBase actorTarget)
        {
            if ((actor.Dad == null || actor.Mom == null) && (actorTarget.Dad == null || actorTarget.Mom == null))
                return true;

            if (actor.Dad == actorTarget) return false;
            if (actor.Mom == actorTarget) return false;
            if (actor == actorTarget.Dad) return false;
            if (actor == actorTarget.Mom) return false;

            if (actor.Dad == actorTarget.Dad && actor.Mom == actorTarget.Mom) return false;
            if (Math.Abs(actor.Generation - actorTarget.Generation) > 1) return false;

            return true;
        }
    }
}
