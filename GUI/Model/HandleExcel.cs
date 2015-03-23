using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using System.Reflection;

namespace GUI.Model
{
    public class HandleExcel
    {

        /// <summary>
        /// 读取Excel数据到DataTable
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <param name="handerRowIndex"></param>
        /// <returns></returns>
        public static System.Data.DataTable ImportExcel(string filePath, string sheetName, int handerRowIndex)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                HSSFWorkbook workbook = new HSSFWorkbook(file);
                //获取excel的第一个sheet  
                HSSFSheet sheet = (HSSFSheet)workbook.GetSheet(sheetName);
                //获取sheet的首行  
                HSSFRow headerRow = (HSSFRow)sheet.GetRow(handerRowIndex);

                //一行最后一个方格的编号 即总的列数  
                int cellCount = headerRow.LastCellNum;
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    HSSFCell cell = (HSSFCell)headerRow.GetCell(i);
                    if (cell != null)
                    {
                        DataColumn column = new DataColumn(cell.StringCellValue);
                        table.Columns.Add(column);
                    }
                }
                //最后一列的标号  即总的行数  
                //      int rowCount = sheet.LastRowNum;  
                //cellCount = table.Columns.Count;
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    HSSFRow row = (HSSFRow)sheet.GetRow(i);
                    if (row == null)
                        continue;
                    DataRow dataRow = table.NewRow();
                    //bool isAdd = false;
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        //if (row.GetCell(j) == null)
                        //continue;
                        //if (row.GetCell(j) != null && row.GetCell(j).StringCellValue.Trim() != "") 
                        dataRow[j] = row.GetCell(j).ToString();
                        //isAdd = true;
                    }
                    //if (isAdd)
                    table.Rows.Add(dataRow);
                    // MessageBox.Show(table.Rows[0][1].ToString());
                }
                //workbook = null;
                //sheet = null;
                //MessageBox.Show(table.Rows[0][1].ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return table;
        }
        /// <summary>
        /// 获得模版工作表
        /// </summary>
        /// <param name="modefilepath"></param>
        /// <returns></returns>
        public static Worksheet GetWorkSheet(string modefilepath)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            excelApp.Visible = false;

            Microsoft.Office.Interop.Excel.Workbook wb = excelApp.Workbooks.Open(modefilepath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            Microsoft.Office.Interop.Excel.Worksheet ws = (Worksheet)wb.Worksheets[1];
            return ws;

        }
        /// <summary>
        /// 向word文档写入数据
        /// </summary>
        /// <param name="parLableName"></param>
        /// <param name="parFillName"></param>
        public static void WriteIntoDocument(Document doc, object parLableName, object parFillName)
        {
            object lableName = parLableName;
            Bookmark bm = doc.Bookmarks.get_Item(ref lableName);
            if (parFillName is DBNull)
            {
                bm.Range.Text = "";
            }
            else
            {
                bm.Range.Text = parFillName as string;
            }
        }
        /// <summary>  
        /// 保存数据  
        /// </summary>  
        /// <param name="workbook"></param>  
        /// <param name="strFileName"></param>  
        private static void saveData(HSSFWorkbook workbook, string strFileName)
        {
            //保存       
            using (MemoryStream ms = new MemoryStream())
            {
                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fs);
                }
            }
        }

    }
}
