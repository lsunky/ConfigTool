using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Text;
namespace RhConfigTool.Editor
{
    /// <summary>
    /// 配置文件生成器
    /// </summary>
    public interface IConfigGenerator 
    {
        /// <summary>
        /// 配置文件生成,
        /// excel转对应配置文件
        /// </summary>
        void Excel2Config(SheetData sheetData, string filePath);
    }
}
