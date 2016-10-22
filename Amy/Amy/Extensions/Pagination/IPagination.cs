using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public interface IPagination
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        int DataCount { get; set; }
        /// <summary>
        /// 页总数
        /// </summary>
        int PageCount { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        int CurrentPage { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        int PageSize { get; set; }
        /// <summary>
        /// 分页连接
        /// </summary>
        string Link { get; set; }
    }
}
