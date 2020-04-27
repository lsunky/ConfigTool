using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace RhConfigTool
{
    public class ByteConfigUnSerialize : IConfigUnSerialize
    {

        private void FieldSetValue<T>(BinaryReader binaryReader, FieldInfo field,T t)
        {
            if (field.FieldType == typeof(int))
            {
                field.SetValue(t, binaryReader.ReadInt32());
            }
            else if (field.FieldType == typeof(string))
            {
                string tempStr = binaryReader.ReadString();
                field.SetValue(t, tempStr);
            }
            else if (field.FieldType == typeof(double))
            {
                field.SetValue(t, binaryReader.ReadDouble());
            }
            else if (field.FieldType == typeof(bool))
            {
                field.SetValue(t, binaryReader.ReadBoolean());
            }
            else if (field.FieldType == typeof(long))
            {
                field.SetValue(t, binaryReader.ReadInt64());
            }
            else if (field.FieldType == typeof(List<int>))
            {
                List<int> list = new List<int>();
                int listSize = binaryReader.ReadInt32();
                for (int k = 0; k < listSize; k++)
                {
                    list.Add(binaryReader.ReadInt32());
                }
                field.SetValue(t,list);
            }
            else if (field.FieldType == typeof(List<string>))
            {
                List<string> list = new List<string>();
                int listSize = binaryReader.ReadInt32();
                for (int k = 0; k < listSize; k++)
                {
                    list.Add(binaryReader.ReadString());
                }
                field.SetValue(t, list);
            }
            else if (field.FieldType == typeof(List<double>))
            {
                List<double> list = new List<double>();
                int listSize = binaryReader.ReadInt32();
                for (int k = 0; k < listSize; k++)
                {
                    list.Add(binaryReader.ReadDouble());
                }
                field.SetValue(t, list);
            }
            else if (field.FieldType == typeof(List<bool>))
            {
                List<bool> list = new List<bool>();
                int listSize = binaryReader.ReadInt32();
                for (int k = 0; k < listSize; k++)
                {
                    list.Add(binaryReader.ReadBoolean());
                }
                field.SetValue(t, list);
            }
            else if (field.FieldType == typeof(List<long>))
            {
                List<long> list = new List<long>();
                int listSize = binaryReader.ReadInt32();
                for (int k = 0; k < listSize; k++)
                {
                    list.Add(binaryReader.ReadInt64());
                }
                field.SetValue(t, list);
            }
            else
            {
                throw new Exception("Error!!! Type Error");
            }
        }
        public Dictionary<string, T> Config2Dic<T>(SerializeData serializeData) where T : ConfigBaseData,new()
        {
            if (serializeData == null)
            {
                return null;
            }
            byte[] content = serializeData.ContentByte;
            MemoryStream pStream = new MemoryStream(content);
            BinaryReader binaryReader = new BinaryReader(pStream);
            Dictionary<string, T> dicResult = new Dictionary<string, T>();
            int listCount = binaryReader.ReadInt32();
            FieldInfo fieldInfo; 
            for (int i = 0; i < listCount; i++)
            {
                T t = new T();
                FieldInfo[] pis = t.GetType().GetFields();
                for (int j = 0, length = pis.Length;  j < length;  j++)
                {
                    fieldInfo = pis[j];
                    FieldSetValue<T>(binaryReader, fieldInfo, t);
                    if (j == 0)
                    {
                        dicResult[fieldInfo.GetValue(t).ToString()] = t;
                    }
                }
            }
            binaryReader.Close();
            pStream.Close();
            return dicResult;
        }
    }
          
}
