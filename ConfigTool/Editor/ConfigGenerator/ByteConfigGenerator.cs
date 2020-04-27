using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static RhConfigTool.Editor.ClassGenerator;
using ConfigTool;
using LitJson;

namespace RhConfigTool.Editor
{
    public class ByteConfigGenerator : IConfigGenerator
    {
        private void WriteOneValue(BinaryWriter writer, string strContent, ToolValueType valueType)
        {
            switch (valueType)
            {
                case ToolValueType.Type_Int:
                    writer.Write(int.Parse(strContent));
                    break;
                case ToolValueType.Type_String:
                    writer.Write(strContent);
                    break;
                case ToolValueType.Type_Bool:
                    writer.Write("1".Equals(strContent));
                    break;
                case ToolValueType.Type_Double:
                    writer.Write(double.Parse(strContent));
                    break;
                case ToolValueType.Type_Long:
                    writer.Write(long.Parse(strContent));
                    break;
            }
        }
        private void WriteData(BinaryWriter writer,string strContent, DataInfo dataInfo)
        {
            if (dataInfo.isList)
            {
                JsonData jsonArr = JsonMapper.ToObject(strContent);
                writer.Write(jsonArr.Count);
                for (int i = 0,length = jsonArr.Count; i < length; i++)
                {
                    WriteOneValue(writer, jsonArr[i].ToString(), dataInfo.valueType);
                }
            }
            else
            {
                WriteOneValue(writer, strContent,dataInfo.valueType);
            }
        }

        public void Excel2Config(SheetData sheetData, string filePath)
        {
            List<string[]> strContentList = sheetData.StrContentList;
            DataInfo[] dataInfoArr = Util.GetSheetTittleList(sheetData);
            DataInfo dataInfo;
            string[] strContentArr;
            string strContent;
            int columCount = dataInfoArr.Length;
            BinaryWriter writer = null;
            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(filePath, FileMode.Create);
                writer = new BinaryWriter(fileStream);
            }
            catch (IOException e)
            {
                ConfigToolLog.LogError(e.Message + "\n Cannot create file.");
                return;
            }

            writer.Write(strContentList.Count-2);
            for (int j = 2, length = strContentList.Count; j < length; j++)
            {
                strContentArr = strContentList[j];
                for (int i = 0; i < columCount; i++)
                {
                    dataInfo = dataInfoArr[i];
                    if (dataInfo != null)
                    {
                        strContent = strContentArr[i];
                        WriteData(writer,strContent,dataInfo);
                    }
                }
            }
            writer.Close();
            fileStream.Close();

        }
    }
}
