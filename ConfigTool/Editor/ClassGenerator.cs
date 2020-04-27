using RhConfigTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RhConfigTool.Editor
{
    public class ClassGenerator
    {
        const string ConfigMgrName = "ConfigMgr";
        private static Dictionary<ToolValueType, string> dicTypeMapping = new Dictionary<ToolValueType, string>
        {
                { ToolValueType.Type_Int,"int"},
                { ToolValueType.Type_String,"string"},
                { ToolValueType.Type_Bool,"bool"},
                { ToolValueType.Type_Double,"double"},
                { ToolValueType.Type_Long,"long"},
        };
        private class ClassFormation
        {
            private List<DataInfo> dataList;
            private string className;
            private string notes;
            public List<DataInfo> DataList { get { return dataList; }  }

            public string ClassName { get { return className; } }
            public string Notes { get { return notes; } }
            public ClassFormation(string className,string notes)
            {
                dataList = new List<DataInfo>();
                this.className = className;
                this.notes = notes;
            }
            public void AddDataInfo(DataInfo dataInfo)
            {
                dataList.Add(dataInfo);
            }
        }

        public class DataInfo
        {
            /// <summary>
            /// 注释
            /// </summary>
            public string desc;

            /// <summary>
            /// 是否是列表
            /// </summary>
            public bool isList;

            /// <summary>
            /// 字段名
            /// </summary>
            public string titleName;

            /// <summary>
            /// 数值类型
            /// </summary>
            public ToolValueType valueType;
        }

        /// <summary>
        /// 数值类型
        /// </summary>
        public enum ToolValueType
        {
            Type_Int,
            Type_String,
            Type_Bool,
            Type_Double,
            Type_Long,
        }

        public static void CreateClass(List<SheetData> list, string classDir )
        {
            List<ClassFormation> classListAll = new List<ClassFormation>();
            ClassFormation classFormation;
            foreach (var sheetData in list)
            {
                classFormation = ReadSheet(sheetData);
                classListAll.Add(classFormation);
            }
            WriteClassAll(classListAll, classDir);
        }
        
        private static ClassFormation ReadSheet(SheetData sheetData)
        {
            ClassFormation classFormation = new ClassFormation(sheetData.SheetName,sheetData.Notes);
            string strContent;
            List<string[]> strContentList = sheetData.StrContentList;
            for (int i = 0; i < sheetData.ColumCount; i++)
            {
                strContent = strContentList[0][i];
                if (!Util.IsEmptyTittle(strContent))
                {
                    DataInfo data = Util.CreateDataInfo(strContent, i, sheetData.SheetName);
                    data.desc = strContentList[1][i];
                    classFormation.AddDataInfo(data);
                }
            }
            return classFormation;
        }

        private static string GetFieldStr(DataInfo dataInfo)
        {
            string strResult = "    public ";
            string typeString = dicTypeMapping[dataInfo.valueType];
            if (dataInfo.isList)
            {
                strResult += "List<";
                strResult += typeString;
                strResult += ">";
            }
            else
            {
                strResult += typeString;
            }
            strResult += (" "+ dataInfo.titleName+";");
            return strResult;
        }

        private static void WriteOneClass(ClassFormation classFormation, string classDir)
        {
            DataInfo dataInfo;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Clear();
            stringBuilder.AppendLine("using System.Collections.Generic;");
            stringBuilder.AppendLine(string.Empty);
            if (!string.IsNullOrEmpty(classFormation.Notes))
            {
                stringBuilder.AppendLine("/// </summary>");
                stringBuilder.AppendLine("/// " + classFormation.Notes);
                stringBuilder.AppendLine("/// </summary>");
            }
            stringBuilder.AppendLine(string.Format("public class {0} : ConfigBaseData", classFormation.ClassName));
            stringBuilder.AppendLine("{");
            for (int i = 0, length = classFormation.DataList.Count; i < length; i++)
            {
                dataInfo = classFormation.DataList[i];
                stringBuilder.AppendLine("    /// <summary>");
                stringBuilder.AppendLine("    /// " + dataInfo.desc);
                stringBuilder.AppendLine("    /// <summary>");
                stringBuilder.AppendLine(GetFieldStr(dataInfo));
                stringBuilder.AppendLine(string.Empty);
            }
            stringBuilder.AppendLine("}");
            File.WriteAllText(classDir + classFormation.ClassName + ".cs", stringBuilder.ToString());
        }

        private static void WriteClassAll(List<ClassFormation> classListAll,string classDir)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("using RhConfigTool;");
            stringBuilder.AppendLine("using System;");
            stringBuilder.AppendLine("using System.Collections.Generic;");
            stringBuilder.AppendLine("using System.Reflection;");
            stringBuilder.AppendLine(string.Format("public class {0}", ConfigMgrName));
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine(string.Format("    private static {0} instance;", ConfigMgrName));
            stringBuilder.AppendLine("    public static "+ ConfigMgrName+" Instance" );
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine("        get");
            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine("            if (instance == null)");
            stringBuilder.AppendLine(string.Format("                instance = new {0}();", ConfigMgrName));
            stringBuilder.AppendLine("            return instance ;");
            stringBuilder.AppendLine("        }");
            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine("    private IConfigUnSerialize configUnSerialize;");
            stringBuilder.AppendLine(string.Empty);

            StringBuilder stringBuilderArr = new StringBuilder();
            foreach (ClassFormation classFormation in classListAll)
            {
                if (!string.IsNullOrEmpty(classFormation.Notes))
                {
                    stringBuilder.AppendLine("    /// <summary>");
                    stringBuilder.AppendLine("    /// " + classFormation.Notes);
                    stringBuilder.AppendLine("    /// </summary>");
                    stringBuilder.AppendLine(string.Format("    public ConfigBaseClass<{0}> {1};", classFormation.ClassName, "m_"+ classFormation.ClassName));

                    stringBuilderArr.Append(string.Format("\"{0}\",", classFormation.ClassName));
                }
                WriteOneClass(classFormation, classDir);
            }
            stringBuilder.Append("    public readonly string[] ConfigNamesArr = { ");
            stringBuilder.Append(stringBuilderArr.ToString());
            stringBuilder.Append("};");
            stringBuilder.AppendLine(string.Empty);

            stringBuilder.AppendLine("    public void Init(IConfigUnSerialize configUnSerialize)");
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine("        this.configUnSerialize = configUnSerialize;");
            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine(string.Empty);

            stringBuilder.AppendLine("    public void CreatData()");
            stringBuilder.AppendLine("    {");
            foreach (ClassFormation classFormation in classListAll)
            {
                stringBuilder.AppendLine(string.Format("        m_{0} = new ConfigBaseClass<{1}>();", classFormation.ClassName, classFormation.ClassName));
            }
            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine(string.Empty);

            stringBuilder.AppendLine("    /// <summary>");
            stringBuilder.AppendLine("    /// 反序列化某一个config");
            stringBuilder.AppendLine("    /// <summary>");
            stringBuilder.AppendLine("    /// <param name=\"serializeData\">序列化data</param>");
            stringBuilder.AppendLine("    public void UnSerializeConfig(SerializeData serializeData )");
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine("        Type type = this.GetType();");
            stringBuilder.AppendLine("        string fieldName = \"m_\" + serializeData.ConfigName;");
            stringBuilder.AppendLine("        FieldInfo fiPrivate = type.GetField(fieldName);");
            stringBuilder.AppendLine("        object baseConfig = fiPrivate.GetValue(this);");
            stringBuilder.AppendLine("        object[] args = { serializeData, configUnSerialize };");
            stringBuilder.AppendLine("        baseConfig.GetType().InvokeMember(\"SetData\", BindingFlags.Default | BindingFlags.InvokeMethod, null, baseConfig, args);");
            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine(string.Empty);

            stringBuilder.AppendLine("}");
            File.WriteAllText(classDir + ConfigMgrName + ".cs", stringBuilder.ToString());
        }
    }
}
