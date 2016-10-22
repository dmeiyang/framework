using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Amy.WebUI.Controllers;
using Amy.Unit.NPOI;


namespace Amy.WebUI.Areas.Admin.Controllers
{
    public class NPOIController : BasicController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerateExcel(string path)
        {
            Amy.Unit.NPOI.IExcel excelFactory = new Amy.Unit.NPOI.ExcelBase();

            //初始化摘要信息
            excelFactory.InitializeWorkbook();

            //创建Sheet
            var sheet = excelFactory.CreateSheet("云社保业务申请单");

            #region 绘制表单Title
            var title = sheet.CreateRow(0);

            var titleStyle = excelFactory.CreateCellStyle("微软雅黑", 30, 8, alignment: 2);

            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(title, 0, "云社保业务申请单").CellStyle = titleStyle;

            sheet.AddMergedRegion(0, 0, 0, 9);
            #endregion

            #region 绘制表单头部
            var header = sheet.CreateRow(1);

            var headerStyle = excelFactory.CreateCellStyle("微软雅黑", 12);

            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 0, "员工姓名").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 1, "所属公司").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 2, "证件编号").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 3, "入职时间").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 4, "月薪（元）").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 5, "联系方式").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 6, "员工状态").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 7, "办理业务").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 8, "服务类型").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 9, "开始时间").CellStyle = headerStyle;
            #endregion

            #region 绘制COL宽度
            sheet.SetColumnWidth(0, 6000);
            sheet.SetColumnWidth(1, 10000);
            sheet.SetColumnWidth(2, 10000);
            sheet.SetColumnWidth(3, 6000);
            sheet.SetColumnWidth(4, 6000);
            sheet.SetColumnWidth(5, 6000);
            sheet.SetColumnWidth(6, 6000);
            sheet.SetColumnWidth(7, 6000);
            sheet.SetColumnWidth(8, 6000);
            sheet.SetColumnWidth(9, 6000);
            #endregion

            #region 绘制表格数据
            var tableStyle = excelFactory.CreateCellStyle("微软雅黑");

            var list = Student.Init();

            int n = 2;

            foreach (var v in list)
            {
                var row = sheet.CreateRow(n);

                //row.CreateCell(0).SetCellValue(v.Sno);
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 0, v.Name).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 1, v.CompanyName).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 2, v.Sno).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 3, v.HireDate.ToFormatDay()).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 4, v.Salary.ToString()).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 5, v.Phone).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 6, v.Status).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 7, v.Business).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 8, v.BusinessType).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 9, v.StartDate.ToFormatDay()).CellStyle = tableStyle;

                n++;
            }
            #endregion

            #region 绘制下拉菜单
            sheet.SetCellDownloadList(2, 100, 1, 1, new string[] { "Angle天使医药有限公司", "Angle天使科技有限公司" });
            sheet.SetCellDownloadList(2, 100, 6, 6, new string[] { "试用", "正式", "离职" });
            sheet.SetCellDownloadList(2, 100, 7, 7, new string[] { "标准五险", "五险一金" });
            sheet.SetCellDownloadList(2, 100, 8, 8, new string[] { "新增" });
            #endregion

            //#region 验证数据有效性
            //sheet.SetCellInputNumber(0, 100, 4, 4, "20", "100");
            //#endregion

            //#region 绘制Excel公式
            //#endregion

            return JRCommonHandleResult(excelFactory.WriteToFile(path));
        }

        public void GenerateAndDownload()
        {
            string filename = "云社保业务申请单.xls";
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            Response.Clear();

            Amy.Unit.NPOI.IExcel excelFactory = new Amy.Unit.NPOI.ExcelBase();

            //初始化摘要信息
            excelFactory.InitializeWorkbook();

            //创建Sheet
            var sheet = excelFactory.CreateSheet("云社保业务申请单");

            #region 绘制表单Title
            var title = sheet.CreateRow(0);

            var titleStyle = excelFactory.CreateCellStyle("微软雅黑", 30, 8, alignment: 2);

            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(title, 0, "云社保业务申请单").CellStyle = titleStyle;

            sheet.AddMergedRegion(0, 0, 0, 9);
            #endregion

            #region 绘制表单头部
            var header = sheet.CreateRow(1);

            var headerStyle = excelFactory.CreateCellStyle("微软雅黑", 12);

            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 0, "员工姓名").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 1, "所属公司").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 2, "证件编号").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 3, "入职时间").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 4, "月薪（元）").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 5, "联系方式").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 6, "员工状态").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 7, "办理业务").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 8, "服务类型").CellStyle = headerStyle;
            NPOI.HSSF.Util.HSSFCellUtil.CreateCell(header, 9, "开始时间").CellStyle = headerStyle;
            #endregion

            #region 绘制COL宽度
            sheet.SetColumnWidth(0, 6000);
            sheet.SetColumnWidth(1, 10000);
            sheet.SetColumnWidth(2, 10000);
            sheet.SetColumnWidth(3, 6000);
            sheet.SetColumnWidth(4, 6000);
            sheet.SetColumnWidth(5, 6000);
            sheet.SetColumnWidth(6, 6000);
            sheet.SetColumnWidth(7, 6000);
            sheet.SetColumnWidth(8, 6000);
            sheet.SetColumnWidth(9, 6000);
            #endregion

            #region 绘制表格数据
            var tableStyle = excelFactory.CreateCellStyle("微软雅黑");

            var list = Student.Init();

            int n = 2;

            foreach (var v in list)
            {
                var row = sheet.CreateRow(n);

                //row.CreateCell(0).SetCellValue(v.Sno);
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 0, v.Name).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 1, v.CompanyName).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 2, v.Sno).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 3, v.HireDate.ToFormatDay()).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 4, v.Salary.ToString()).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 5, v.Phone).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 6, v.Status).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 7, v.Business).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 8, v.BusinessType).CellStyle = tableStyle;
                NPOI.HSSF.Util.HSSFCellUtil.CreateCell(row, 9, v.StartDate.ToFormatDay()).CellStyle = tableStyle;

                n++;
            }
            #endregion

            #region 绘制下拉菜单
            sheet.SetCellDownloadList(2, 100, 1, 1, new string[] { "Angle天使医药有限公司", "Angle天使科技有限公司" });
            sheet.SetCellDownloadList(2, 100, 6, 6, new string[] { "试用", "正式", "离职" });
            sheet.SetCellDownloadList(2, 100, 7, 7, new string[] { "标准五险", "五险一金" });
            sheet.SetCellDownloadList(2, 100, 8, 8, new string[] { "新增" });
            #endregion

            Response.BinaryWrite(excelFactory.WriteToStream().GetBuffer());
            Response.End();
        }

        public ActionResult FontColor()
        {
            var color = new List<NPOI.HSSF.Util.HSSFColor> {
                new NPOI.HSSF.Util.HSSFColor.BLACK(), new NPOI.HSSF.Util.HSSFColor.BROWN(), new NPOI.HSSF.Util.HSSFColor.OLIVE_GREEN(), new NPOI.HSSF.Util.HSSFColor.DARK_GREEN(),
                new NPOI.HSSF.Util.HSSFColor.DARK_TEAL(), new NPOI.HSSF.Util.HSSFColor.DARK_BLUE(), new NPOI.HSSF.Util.HSSFColor.INDIGO(), new NPOI.HSSF.Util.HSSFColor.GREY_80_PERCENT(),
                new NPOI.HSSF.Util.HSSFColor.ORANGE(), new NPOI.HSSF.Util.HSSFColor.DARK_YELLOW(), new NPOI.HSSF.Util.HSSFColor.GREEN(), new NPOI.HSSF.Util.HSSFColor.TEAL(), new NPOI.HSSF.Util.HSSFColor.BLUE(),
                new NPOI.HSSF.Util.HSSFColor.BLUE_GREY(), new NPOI.HSSF.Util.HSSFColor.GREY_50_PERCENT(), new NPOI.HSSF.Util.HSSFColor.RED(), new NPOI.HSSF.Util.HSSFColor.LIGHT_ORANGE(), new NPOI.HSSF.Util.HSSFColor.LIME(),
                new NPOI.HSSF.Util.HSSFColor.SEA_GREEN(), new NPOI.HSSF.Util.HSSFColor.AQUA(), new NPOI.HSSF.Util.HSSFColor.LIGHT_BLUE(), new NPOI.HSSF.Util.HSSFColor.VIOLET(), new NPOI.HSSF.Util.HSSFColor.GREY_40_PERCENT(),
                new NPOI.HSSF.Util.HSSFColor.PINK(), new NPOI.HSSF.Util.HSSFColor.GOLD(), new NPOI.HSSF.Util.HSSFColor.YELLOW(), new NPOI.HSSF.Util.HSSFColor.BRIGHT_GREEN(), new NPOI.HSSF.Util.HSSFColor.TURQUOISE(),
                new NPOI.HSSF.Util.HSSFColor.DARK_RED(), new NPOI.HSSF.Util.HSSFColor.SKY_BLUE(), new NPOI.HSSF.Util.HSSFColor.PLUM(), new NPOI.HSSF.Util.HSSFColor.GREY_25_PERCENT(), new NPOI.HSSF.Util.HSSFColor.ROSE(),
                new NPOI.HSSF.Util.HSSFColor.LIGHT_YELLOW(), new NPOI.HSSF.Util.HSSFColor.LIGHT_GREEN(), new NPOI.HSSF.Util.HSSFColor.LIGHT_TURQUOISE(), new NPOI.HSSF.Util.HSSFColor.PALE_BLUE(),
                new NPOI.HSSF.Util.HSSFColor.LAVENDER(), new NPOI.HSSF.Util.HSSFColor.WHITE(), new NPOI.HSSF.Util.HSSFColor.CORNFLOWER_BLUE(), new NPOI.HSSF.Util.HSSFColor.LEMON_CHIFFON(),
                new NPOI.HSSF.Util.HSSFColor.MAROON(), new NPOI.HSSF.Util.HSSFColor.ORCHID(), new NPOI.HSSF.Util.HSSFColor.CORAL(), new NPOI.HSSF.Util.HSSFColor.ROYAL_BLUE(),
                new NPOI.HSSF.Util.HSSFColor.LIGHT_CORNFLOWER_BLUE(), new NPOI.HSSF.Util.HSSFColor.TAN(),
            };

            return View(color);
        }
    }

    public class Student
    {
        public static List<Student> Init()
        {
            var list = new List<Student>();

            list.Add(new Student()
            {
                Name = "狼啸皓月",
                CompanyName = "Angle天使医药有限公司",
                Sno = "410822198503207033",
                HireDate = "2013-07-01".ToDateTime(),
                Salary = 10000,
                Phone = "18838187047",
                Status = "正式",
                Business = "标准五险",
                BusinessType = "新增",
                StartDate = "2016-03".ToDateTime()
            });

            list.Add(new Student()
            {
                Name = "大漠雪狼",
                CompanyName = "Angle天使医药有限公司",
                Sno = "410822198503207034",
                HireDate = "2013-07-01".ToDateTime(),
                Salary = 10000,
                Phone = "18838187047",
                Status = "正式",
                Business = "标准五险",
                BusinessType = "新增",
                StartDate = "2016-03".ToDateTime()
            });

            list.Add(new Student()
            {
                Name = "辽美式丶残月",
                CompanyName = "Angle天使医药有限公司",
                Sno = "410822198503207035",
                HireDate = "2013-07-01".ToDateTime(),
                Salary = 10000,
                Phone = "18838187047",
                Status = "正式",
                Business = "标准五险",
                BusinessType = "新增",
                StartDate = "2016-03".ToDateTime()
            });

            return list;
        }

        public string Name { get; set; }

        public string CompanyName { get; set; }

        public string Sno { get; set; }

        public DateTime HireDate { get; set; }

        public double Salary { get; set; }

        public string Phone { get; set; }

        public string Status { get; set; }

        public string Business { get; set; }

        public string BusinessType { get; set; }

        public DateTime StartDate { get; set; }
    }
}