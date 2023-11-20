using AdventOfCodeCore.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdventOfCodeCore.Solutions._2015
{
    internal class Day12
    {
        [Solution(12, 1)]
        public int Solution1(string input)
        {
            var jToken = JsonConvert.DeserializeObject<JToken>(input)!;

            return GetValueOfElement(jToken);
        }

        [Solution(12, 2)]
        public int Solution2(string input)
        {
            var jToken = JsonConvert.DeserializeObject<JToken>(input)!;

            return GetValueOfElement(jToken, true);
        }

        private int GetValueOfElement(JToken element, bool skipRed = false)
        {

            if (element.Type == JTokenType.Integer)
                return element.Value<int>();

            if (element.Type == JTokenType.Object)
            {
                if (skipRed)
                {
                    var containsRed = element.Select(it => (JProperty)it).Any(it => it.Value.Type == JTokenType.String && it.Value.ToString() == "red");

                    if (containsRed)
                        return 0;
                }

                var sum = 0;

                foreach (JProperty item in element)
                {
                    sum += GetValueOfElement(item.Value, skipRed);
                }

                return sum;
            }

            if (element.Type == JTokenType.Array)
            {
                var sum = 0;

                foreach (var item in element.Value<JArray>()!)
                {
                    sum += GetValueOfElement(item, skipRed);
                }

                return sum;
            }

            return 0;
        }
    }
}
