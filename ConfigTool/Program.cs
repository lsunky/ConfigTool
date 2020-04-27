using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Data;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using RhConfigTool;
using LitJson;
using RhConfigTool.Editor;

namespace ConfigTool
{

    class Program
    {
        static string GetSuffixName(ConfigFormationType configFormationType)
        {
            if (configFormationType == ConfigFormationType.Byte)
            {
                return "byte";
            }
            else
            {
                return "json";
            }
        }

        static List<SheetData> GetAllSheets(string excelDir, bool justTittle)
        {
            string[] files = Directory.GetFiles(excelDir);
            List<SheetData> sheetDataAll = new List<SheetData>();
            foreach (var fileFullPath in files)
            {
                List<SheetData> list = Excel2ListUtil.Excel2List(fileFullPath, justTittle);
                sheetDataAll.AddRange(list);
            }
            return sheetDataAll;
        }

        /// <summary>
        /// 把excel转为对应的config,editor环境下用
        /// </summary>
        /// <param name="excelDir"></param>
        /// <param name="configDir"></param>
        /// <param name="configGenerator"></param>
        static void Excel2Configs(string excelDir,string configDir, IConfigGenerator configGenerator,string suffixName)
        {
            List<SheetData> sheetDataAll = GetAllSheets(excelDir,false);
            if (Directory.Exists(configDir))
            {
                Directory.Delete(configDir, true);
            }
            Directory.CreateDirectory(configDir);
            foreach (var item in sheetDataAll)
            {
                configGenerator.Excel2Config(item, string.Format(configDir + "{0}.{1}", item.SheetName, suffixName));
            }
            ConfigToolLog.LogInfo("Excel2Configs complete");
        }

        /// <summary>
        /// 把excel生成对应的Class文件,editor环境下用
        /// </summary>
        /// <param name="excelDir"></param>
        /// <param name="classDir"></param>
        static void Excel2Class(string excelDir, string classDir)
        {
            List<SheetData> sheetDataAll = GetAllSheets(excelDir, true);

            if (Directory.Exists(classDir))
            {
                Directory.Delete(classDir, true);
            }
            Directory.CreateDirectory(classDir);
            ClassGenerator.CreateClass(sheetDataAll, classDir);
            ConfigToolLog.LogInfo("Excel2Class complete");
        }

        /// <summary>
        /// 读取所有配置到configMgr，运行时
        /// </summary>
        /// <param name="configDir"></param>
        /// <param name="configGenerator"></param>
        static void ReadAllConfig(string configDir, IConfigUnSerialize configUnSerialize, string suffixName)
        {
            ConfigMgr.Instance.Init(configUnSerialize);
            ConfigMgr.Instance.CreatData();
            string[] configArr = ConfigMgr.Instance.ConfigNamesArr;
            foreach (var configName in configArr)
            {
                byte[] content = File.ReadAllBytes(string.Format(configDir+"{0}.{1}", configName, suffixName));
                SerializeData serializeData = new SerializeData(content, configName);
                ConfigMgr.Instance.UnSerializeConfig(serializeData);
                //ConfigToolLog.LogInfo(ConfigMgr.Instance.m_CardConfig.getValue("100007").name);
                ConfigToolLog.LogInfo("All config readCompleted");
            }  
        }

        enum GenerType
        {
            /// <summary>
            /// 只创建config
            /// </summary>
            Config = 1,

            /// <summary>
            /// 只创建解析类
            /// </summary>
            Class = 2,

            /// <summary>
            /// 创建config和解析类
            /// </summary>
            All = 3,
        }

        enum ConfigFormationType
        {
            /// <summary>
            /// 配置类型为json
            /// </summary>
            Json = 1,

            /// <summary>
            /// 配置类型为二进制
            /// </summary>
            Byte = 2,
        }

        static void Main(string[] args)
        {
            ConfigToolLog.Init(Console.WriteLine, Console.WriteLine);
            string configToolDirRoot = Path.GetFullPath("../../../../");
            string excelDir = configToolDirRoot + "/excelDir/";
            string configDir = configToolDirRoot + "/configDir/";
            string classDir = configToolDirRoot + "/classDir/";
            IConfigGenerator configGenerator = null;
            GenerType generType = GenerType.All;
            ConfigFormationType configFormationType = ConfigFormationType.Byte;
            if (args.Length != 0)//通过传参进来的调用
            {
                configFormationType = (ConfigFormationType)int.Parse(args[0]);
                generType = (GenerType)int.Parse(args[1]);
                switch (generType)
                {
                    case GenerType.Config:
                        excelDir = args[2];
                        configDir = args[3];
                        break;
                    case GenerType.Class:
                        excelDir = args[2];
                        classDir = args[3];
                        break;
                    default:
                        excelDir = args[2];
                        configDir = args[3];
                        classDir = args[4];
                        break;
                }
            }

            string suffixName = GetSuffixName(configFormationType);
            switch (configFormationType)
            {
                case ConfigFormationType.Json:
                    configGenerator = new JsonConfigGenerator();
                    break;
                case ConfigFormationType.Byte:
                    configGenerator = new ByteConfigGenerator();
                    break;
            }

            switch (generType)
            {
                case GenerType.Config:
                    Excel2Configs(excelDir, configDir, configGenerator, suffixName);
                    break;
                case GenerType.Class:
                    Excel2Class(excelDir, classDir);
                    break;
                default:
                    Excel2Configs(excelDir, configDir, configGenerator, suffixName);
                    Excel2Class(excelDir, classDir);
                    break;
            }

            //测试解析  读取配置测试,代码路径必须要引入到工程里才能用
            //IConfigUnSerialize configUnSerialize;
            //switch (configFormationType)
            //{
            //    case ConfigFormationType.Json:
            //        configUnSerialize = new JsonConfigUnSerialize();
            //        break;
            //    case ConfigFormationType.Byte:
            //        configUnSerialize = new ByteConfigUnSerialize();
            //        break;
            //}
            //ReadAllConfig(configDir, configUnSerialize, suffixName);
            Console.ReadKey();
        }

        
    }
}
