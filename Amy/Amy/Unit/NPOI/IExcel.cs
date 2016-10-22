using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

namespace Amy.Unit.NPOI
{
    public interface IExcel
    {
        /// <summary>
        /// 初始化摘要信息
        /// </summary>
        void InitializeWorkbook();

        /// <summary>
        /// 初始化摘要信息
        /// </summary>
        /// <param name="company">公司信息</param>
        /// <param name="subject">主题信息</param>
        void InitializeWorkbook(string company, string subject);

        /// <summary>
        /// 创建Sheet
        /// </summary>
        Sheet CreateSheet(string name);

        /// <summary>
        /// 创建cellstyle
        /// </summary>
        CellStyle CreateCellStyle(string fontname = "宋体", short size = 10, short color = 8, byte underline = 0, bool isItalic = false, bool isStrikeout = false, byte verticalAlignment = 1, byte alignment = 1);

        /// <summary>
        /// 创建Excel文件
        /// </summary>
        /// <param name="path">文件物理路径</param>
        /// <returns>true，创建成功；false，创建失败</returns>
        bool WriteToFile(string path);

        /// <summary>
        /// 创建Excel文件流
        /// </summary>
        /// <param name="path">文件物理路径</param>
        /// <returns>Excel文件流</returns>
        System.IO.MemoryStream WriteToStream();
    }
}
