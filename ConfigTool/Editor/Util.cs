using System;
using System.Collections.Generic;
using System.Text;
using static RhConfigTool.Editor.ClassGenerator;

namespace RhConfigTool.Editor
{
    public class Util
    {
        private static Dictionary<string, ToolValueType> dicTypeMapping = new Dictionary<string, ToolValueType>
        {
                { "i",ToolValueType.Type_Int},
                { "s",ToolValueType.Type_String},
                { "b",ToolValueType.Type_Bool},
                { "d",ToolValueType.Type_Double},
                { "l",ToolValueType.Type_Long},
        };

        public static DataInfo CreateDataInfo(string strContent,int i,string sheetName)
        {
            DataInfo dataInfo = new DataInfo();
            string[] strArr = strContent.Split('_');
            DataInfo data = new DataInfo();
            data.titleName = strArr[0];
            data.isList = strArr.Length > 2;
            ToolValueType toolValueType;
            if (!dicTypeMapping.TryGetValue(strArr[1], out toolValueType))
            {
                throw new Exception(string.Format("{0}表第{1}列表头设计有问题", sheetName, i));
            }
            data.valueType = toolValueType;
            return data;
        }

        public static bool IsEmptyTittle(string tittleContent)
        {
            return string.IsNullOrEmpty(tittleContent) || "skip".Equals(tittleContent);
        }

        public static bool IsAvailableSheetName(string sheetName)
        {
            if (string.IsNullOrEmpty(sheetName) || sheetName.Contains("Sheet"))
            {
                return false;
            }
            return true;
        }

        public static DataInfo[] GetSheetTittleList(SheetData sheetData)
        {
            List<string[]> strContentList = sheetData.StrContentList;
            DataInfo dataInfo;
            string[] strContentArr = strContentList[0];
            int columCount = strContentArr.Length;
            DataInfo[] dataInfoArr = new DataInfo[columCount];
            string strContent;
            for (int i = 0; i < columCount; i++)
            {
                strContent = strContentArr[i];
                if (!Util.IsEmptyTittle(strContent))
                {
                    dataInfo = Util.CreateDataInfo(strContent, i, sheetData.SheetName);
                    dataInfoArr[i] = dataInfo;
                }
            }
            return dataInfoArr;
        }
    }
}
