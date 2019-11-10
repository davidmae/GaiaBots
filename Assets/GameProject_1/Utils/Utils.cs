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
        
    }
}
