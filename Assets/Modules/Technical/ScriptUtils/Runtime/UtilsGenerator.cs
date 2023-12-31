using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Random = UnityEngine.Random;

namespace Modules.Technical.ScriptUtils.Runtime
{
    public static class UtilsGenerator
    {
        public static string GenerateAlphaNum4Code() => Path.GetRandomFileName()[..4].ToUpper();

        public static List<int> GenerateRandomNumbersInRange(int min, int max, int amount)
        {
            var generated = new List<int>();
            for (var i = min; i < amount; i++)
            {
                var rnd = Random.Range(min, max);
                while (generated.Contains(rnd)) rnd = Random.Range(min, max);
                generated.Add(rnd);
            }

            return generated;
        }

        public static T RandomInEnum<T>(T[] blackList = null) where T : struct
        {
            var values = Enum.GetNames(typeof(T)).ToList();
            if (blackList != null)
                foreach (var elem in blackList)
                    values.Remove(elem.ToString());
            var rnd = Random.Range(0, values.Count);
            return Enum.Parse<T>(values[rnd]);
        }
    }
}