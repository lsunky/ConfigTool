using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using static RhConfigTool.Editor.ClassGenerator;

namespace RhConfigTool.Editor
{
    public class JsonConfigGenerator : IConfigGenerator
    {
        public void Excel2Config(SheetData sheetData, string filePath)
        {
            List<string[]> strContentList = sheetData.StrContentList;
            DataInfo[] dataInfoArr = Util.GetSheetTittleList(sheetData);
            DataInfo dataInfo;
            string[] strContentArr;
            string strContent;
            int columCount = dataInfoArr.Length;
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            string json;
            for (int j = 2,length = strContentList.Count; j < length; j++)
            {
                strContentArr = strContentList[j];
                sb.Append("\n{");
                for (int i = 0; i < columCount; i++)
                {
                    dataInfo = dataInfoArr[i];
                    if (dataInfo != null)
                    {
                        strContent = strContentArr[i];
                        json = ToJson(strContent, dataInfo);
                        sb.Append(json);
                        if (i != columCount - 1)
                        {
                            sb.Append(',');
                        }
                    }
                }
                if (j != length - 1)
                {
                    sb.Append("},");
                }
                else
                {
                    sb.Append("}");
                }
            }
            sb.Append("\n]");
            File.WriteAllText(filePath, sb.ToString(),Encoding.UTF8);
        }



        private string ToJson(string strContent, DataInfo dataInfo)
        {
            if (!dataInfo.isList)
            {
                switch (dataInfo.valueType)
                {
                    case ToolValueType.Type_String:
                        return string.Format("\"{0}\":\"{1}\"", dataInfo.titleName, strContent);
                    case ToolValueType.Type_Bool:
                        return string.Format("\"{0}\":{1}", dataInfo.titleName, strContent == "1"?"true":"false");
                }
            }
            return string.Format("\"{0}\":{1}", dataInfo.titleName, strContent);
        }
    }
}
