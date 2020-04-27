using System.Collections.Generic;

/// </summary>
/// 卡牌注释
/// </summary>
public class CardConfig : ConfigBaseData
{
    /// <summary>
    /// id
    /// <summary>
    public int id;

    /// <summary>
    /// 名字
    /// <summary>
    public string name;

    /// <summary>
    /// cd
    /// <summary>
    public int cd;

    /// <summary>
    /// 攻击力
    /// <summary>
    public int atk;

    /// <summary>
    /// 血量
    /// <summary>
    public int hp;

    /// <summary>
    /// 星级
    /// <summary>
    public double star;

    /// <summary>
    /// 描述
    /// <summary>
    public string des;

    /// <summary>
    /// 防御
    /// <summary>
    public List<int> def;

    /// <summary>
    /// 是否男人
    /// <summary>
    public bool isMan;

}
