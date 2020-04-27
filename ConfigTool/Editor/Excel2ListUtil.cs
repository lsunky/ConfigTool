using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RhConfigTool.Editor
{
    public class Excel2ListUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet">表</param>
        /// <param name="justTittle">如果是创建构造类，则只需要读取前两行</param>
        /// <returns></returns>
        private static SheetData ReadSheet(ISheet sheet, bool justTittle)
        {
            int curRow = 0;
            IRow irow = sheet.GetRow(0); //first row is property
            int columCount = irow.PhysicalNumberOfCells;
            string[] strContentArr;
            bool emptyBreak = false;
            string[] strArr = sheet.SheetName.Split('|');
            string notes = strArr.Length > 1 ? strArr[1]:string.Empty;
            SheetData sheetData = new SheetData(strArr[0], notes, columCount);
            while (true)
            {
                irow = sheet.GetRow(curRow);
                if (emptyBreak && (irow == null || irow.Cells.Count  == 0 || string.IsNullOrEmpty(irow.Cells[0].ToString())))
                {
                    //此行为无效行，终止
                    break;
                }
                
                strContentArr = new string[columCount];
                for (int i = 0,j = 0; i < columCount; i++)//i为excel列,j为有效数值的index
                {
                    if (j >= irow.Cells.Count)//这行填充比较少的情况
                    {
                        break;
                    }
                    while (irow.Cells[j].Address.Column < columCount && irow.Cells[j].Address.Column != i)//空行要跳过
                    {
                        i++;
                    }
                    strContentArr[i] = irow.Cells[j].ToString();
                    j++;
                }
                sheetData.AddRowContent(strContentArr);

                if (curRow == 1)
                {
                    if (justTittle)
                        break;
                    else
                        emptyBreak = true;
                }
                curRow++;
            }
            return sheetData;
        }

        /// <summary>
        /// 把excel里的数据读取到list里，供逻辑处理。其他逻辑里不要有读取excel的逻辑
        /// </summary>
        /// <param name="fileFullPath">文件路径</param>
        /// <param name="justTittle">创建class的时候只需要读取标题</param>
        /// <returns></returns>
        public static List<SheetData> Excel2List(string fileFullPath,bool justTittle)
        {
            List<SheetData> list = new List<SheetData>();
            IWorkbook book = null;
            FileStream stream;
            SheetData sheetData;
            stream = File.Open(fileFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            if (Path.GetExtension(fileFullPath) == ".xls")
            {
                book = new HSSFWorkbook(stream);
            }
            else if (Path.GetExtension(fileFullPath) == ".xlsx")
            {
                book = new XSSFWorkbook(stream);
            }
            else
            {
                return list;
            }
           
            for (int i = 0; i < book.NumberOfSheets; i++)
            {
                ISheet sheet = book.GetSheetAt(i);
                if (Util.IsAvailableSheetName(sheet.SheetName))
                {
                    sheetData = ReadSheet(sheet, justTittle);
                    list.Add(sheetData);
                }
            }
            return list;
        }
    }
}
