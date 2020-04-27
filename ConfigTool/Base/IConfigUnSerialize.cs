
using System.Collections.Generic;

namespace RhConfigTool
{
    public interface IConfigUnSerialize
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializeData">序列化的数据</param>
        /// <returns></returns>
        Dictionary<string, T> Config2Dic<T>(SerializeData serializeData) where T : ConfigBaseData, new();
    }
}
