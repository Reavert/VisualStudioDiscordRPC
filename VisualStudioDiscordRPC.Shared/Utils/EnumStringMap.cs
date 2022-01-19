using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualStudioDiscordRPC.Shared.Utils
{
    public class EnumStringMap<T> where T : Enum
    {
        private readonly Dictionary<T, string> _enumMap;

        private static Dictionary<T, string> BuildEnumStringMap()
        {
            var stringEnumMap = new Dictionary<T, string>();
            Array enumValues = Enum.GetValues(typeof(T));

            foreach (T enumValue in enumValues)
            {
                stringEnumMap.Add(enumValue, enumValue.ToString());
            }

            return stringEnumMap;
        }

        public EnumStringMap()
        {
            _enumMap = BuildEnumStringMap();
        }

        public string GetString(T enumValue)
        {
            return _enumMap[enumValue];
        }

        public T GetEnumValue(string enumValueName)
        {
            return _enumMap.FirstOrDefault(iconOption => iconOption.Value == enumValueName).Key;
        }
    }
}
