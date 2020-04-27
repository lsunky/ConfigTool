using RhConfigTool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
public class ConfigBaseData{}
public class ConfigBaseClass<T> : IEnumerable where T : ConfigBaseData, new()
{
    private Dictionary<string, T> m_data1;

    /// <summary>
    /// 通过反射调用，不要删除引用
    /// </summary>
    /// <param name="data"></param>
    /// <param name="configGenerator"></param>
    /// <param name="tempStr"></param>
    public void SetData(SerializeData data, IConfigUnSerialize configUnSerialize)
    {
        this.m_data1 = configUnSerialize.Config2Dic<T>(data);
    }
    
    /// <summary>
    /// 判断一维Dict中是否包含对应的键值
    /// </summary>
    /// <param name="key1">重载了键值的类型</param>
    /// <returns></returns>
    public bool ContainsKey(long key1)
    {
        return ContainsKey(key1.ToString());
    }

    public bool ContainsKey(string key1)
    {
        if (m_data1 == null)
        {
            return false;
        }
        return m_data1.ContainsKey(key1);
    }

    public T getValue(string key1)
    {
        if (ContainsKey(key1))
            return m_data1[key1];
        else
            return null;

    }

    /// <summary>
    /// 获得一维Dict中对应key的值,由于现在所有Dict的Key都是string,因此重载了此函数
    /// </summary>
    /// <param name="key">key的类型被重载,支持long,string,最终被转换为string</param>
    /// <returns>获得一维Dict中对应key的值</returns>
    public T getValue(long key)
    {
        return getValue(key.ToString());
    }

    /// <summary>
    /// 支持遍历
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetEnumerator()
    {
        foreach (var data1 in m_data1)
        {
            yield return data1.Value;
        }
    }
}