using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NPOI.SS.Util;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

namespace Amy.Unit.NPOI
{
    public static class ExcelExtensions
    {
        /// <summary>
        /// 工作表扩展方法（设置单元格为下拉框并限制输入值）
        /// </summary>
        public static void SetCellDownloadList(this Sheet sheet, int firstRow, int lastRow, int firstCol, int lastCol, string[] explicitListValues)
        {
            //设置作用行和列
            var cellRangeAddressList = new CellRangeAddressList(firstRow, lastRow, firstCol, lastCol);

            //设置下拉框内容
            DVConstraint dvConstraint = DVConstraint.CreateExplicitListConstraint(explicitListValues);

            //绑定下拉框和作用区域，并设置错误提示信息
            HSSFDataValidation dataValidation = new HSSFDataValidation(cellRangeAddressList, dvConstraint);
            dataValidation.CreateErrorBox("输入不合法", "请输入下拉列表中的值！");
            dataValidation.ShowPromptBox = true;

            ((HSSFSheet)sheet).AddValidationData(dataValidation);
        }

        /// <summary>
        /// 工作表扩展方法（设置单元格只能输入数字）
        /// </summary>
        public static void SetCellInputNumber(this Sheet sheet, int firstRow, int lastRow, int firstCol, int lastCol, string min, string max)
        {
            //设置作用行和列
            var cellRangeAddressList = new CellRangeAddressList(firstRow, lastRow, firstCol, lastCol);

            //设置下拉框内容
            DVConstraint dvConstraint = DVConstraint.CreateNumericConstraint(DVConstraint.ValidationType.INTEGER, DVConstraint.OperatorType.BETWEEN, min, max);

            //绑定下拉框和作用区域，并设置错误提示信息
            HSSFDataValidation dataValidation = new HSSFDataValidation(cellRangeAddressList, dvConstraint);
            dataValidation.CreateErrorBox("输入不合法", string.Format("请输入{0}-{1}的数字！", min, max));
            dataValidation.ShowPromptBox = true;

            ((HSSFSheet)sheet).AddValidationData(dataValidation);
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        public static void AddMergedRegion(this Sheet sheet, int firstRow, int lastRow, int firstCol, int lastCol)
        {
            sheet.AddMergedRegion(new CellRangeAddress(firstRow, lastRow, firstCol, lastCol));
        }
    }
}
