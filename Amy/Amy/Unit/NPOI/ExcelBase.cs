using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NPOI.HPSF;
using NPOI.HSSF.Util;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Amy.Unit.NPOI
{
    public class ExcelBase : IExcel
    {
        HSSFWorkbook hssfworkbook;

        public void InitializeWorkbook()
        {
            InitializeWorkbook("荒古禁地", "在线Excel解决方案");
        }

        public void InitializeWorkbook(string company, string subject)
        {
            hssfworkbook = new HSSFWorkbook();

            //设置摘要的公司信息
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = company;
            hssfworkbook.DocumentSummaryInformation = dsi;

            //设置摘要的主题信息
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = subject;
            hssfworkbook.SummaryInformation = si;
        }

        public Sheet CreateSheet(string name)
        {
            return hssfworkbook.CreateSheet(name);
        }

        public CellStyle CreateCellStyle(string fontname = "宋体", short size = 10, short color = 8, byte underline = 0, bool isItalic = false, bool isStrikeout = false, byte verticalAlignment = 1, byte alignment = 1)
        {
            Font font = hssfworkbook.CreateFont();

            font.FontName = fontname;
            font.FontHeightInPoints = size;
            font.Color = color;
            font.Underline = underline;//(byte)FontUnderlineType.DOUBLE;
            font.IsItalic = isItalic;
            font.IsStrikeout = isStrikeout;

            CellStyle style = hssfworkbook.CreateCellStyle();
            style.SetFont(font);

            style.VerticalAlignment = (VerticalAlignment)verticalAlignment;//垂直对齐
            style.Alignment = (HorizontalAlignment)alignment;//水平对齐

            return style;
        }

        public bool WriteToFile(string path)
        {
            System.IO.FileStream file = null;

            try
            {
                file = new System.IO.FileStream(path, System.IO.FileMode.Create);

                hssfworkbook.Write(file);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                file.Close();
            }
        }

        public System.IO.MemoryStream WriteToStream()
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            hssfworkbook.Write(ms);

            return ms;
        }
    }
}
