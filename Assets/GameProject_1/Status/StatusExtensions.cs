using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


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
    }
}
