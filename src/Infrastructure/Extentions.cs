using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace StepByStep.Infrastructure
{
    public static class Extentions
    {
        public static string Serialize<T>(this HttpSessionStateBase httpSessionStateBase)
        {
            return JsonConvert.SerializeObject(httpSessionStateBase[typeof(T).Name]);
        }
    }
}