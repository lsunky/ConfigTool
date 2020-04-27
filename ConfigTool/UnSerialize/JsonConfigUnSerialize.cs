using LitJson;
using System.Collections.Generic;
using System.Reflection;

namespace RhConfigTool
{
    public class JsonConfigUnSerialize : IConfigUnSerialize
    {
        public Dictionary<string, T> Config2Dic<T>(SerializeData serializeData) where T : ConfigBaseData, new()
        {
            if (serializeData == null)
            {
                return null;
            }
            string json = serializeData.ContentStr;

            Dictionary<string, T> dicResult = new Dictionary<string, T>();
            List<T> list = JsonMapper.ToObject<List<T>>(json);
            FieldInfo pi;
            foreach (var item in list)
            {
                pi = item.GetType().GetFields()[0];
                var value = pi.GetValue(item);
                dicResult[value.ToString()] = item;
            }
            return dicResult;
        }
    }
}
