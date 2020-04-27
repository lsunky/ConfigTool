using RhConfigTool;
using System;
using System.Collections.Generic;
using System.Reflection;
public class ConfigMgr
{
    private static ConfigMgr instance;
    public static ConfigMgr Instance
    {
        get
        {
            if (instance == null)
                instance = new ConfigMgr();
            return instance ;
        }
    }
    private IConfigUnSerialize configUnSerialize;

    /// <summary>
    /// 卡牌注释
    /// </summary>
    public ConfigBaseClass<CardConfig> m_CardConfig;
    public readonly string[] ConfigNamesArr = { "CardConfig",};
    public void Init(IConfigUnSerialize configUnSerialize)
    {
        this.configUnSerialize = configUnSerialize;
    }

    public void CreatData()
    {
        m_CardConfig = new ConfigBaseClass<CardConfig>();
    }

    /// <summary>
    /// 反序列化某一个config
    /// <summary>
    /// <param name="serializeData">序列化data</param>
    public void UnSerializeConfig(SerializeData serializeData )
    {
        Type type = this.GetType();
        string fieldName = "m_" + serializeData.ConfigName;
        FieldInfo fiPrivate = type.GetField(fieldName);
        object baseConfig = fiPrivate.GetValue(this);
        object[] args = { serializeData, configUnSerialize };
        baseConfig.GetType().InvokeMember("SetData", BindingFlags.Default | BindingFlags.InvokeMethod, null, baseConfig, args);
    }

}
