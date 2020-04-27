using System;
using System.Collections.Generic;
using System.Text;

namespace RhConfigTool.Editor
{
    public class SheetData
    {
        private string sheetName;
        private string notes;
        private List<string[]> strContentList;
        private int columCount;
        public List<string[]> StrContentList { get { return strContentList; } }
        public string SheetName { get { return sheetName; } }
        public int ColumCount { get { return columCount; } }
        public string Notes { get { return notes; } }

        public SheetData(string sheetName, string notes,int columCount)
        {
            this.sheetName = sheetName;
            this.strContentList = new List<string[]>();
            this.columCount = columCount;
            this.notes = notes;
        }
        
        public void AddRowContent(string[] contentArr)
        {
            strContentList.Add(contentArr);
        }
    }
}
