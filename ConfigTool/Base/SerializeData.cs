using System;
using System.Collections.Generic;
using System.Text;

namespace RhConfigTool
{
    /// <summary>
    /// 序列化的data，如果是二进制解析，请用二进制的构造函数
    /// </summary>
    public class SerializeData
    {
        private byte[] contentByte;
        private string contentStr;
        private string configName;
        public string ContentStr { get { return contentStr; } }
        public string ConfigName { get { return configName; } }

        public byte[] ContentByte { get { return contentByte; }}

        public SerializeData(string content,string configName)
        {
            this.contentStr = content;
            this.configName = configName;
        }

        public SerializeData(byte[] content, string configName)
        {
            this.contentByte = content;
            this.configName = configName;
        }

    }
}
