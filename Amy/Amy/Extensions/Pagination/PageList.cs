using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace System
{
    [CollectionDataContract]
    public class PageList<T> : List<T>, IPagination
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        [DataMember]
        public int DataCount { get; set; }
        /// <summary>
        /// 页总数
        /// </summary>
        [DataMember]
        public int PageCount { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        [DataMember]
        public int CurrentPage { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }
        /// <summary>
        /// 分页连接
        /// </summary>
        public string Link { get; set; }

        public PageList()
        {
        }

        public PageList(IQueryable<T> source, int currentPage, int pageSize)
        {
            Initialize(source, currentPage, pageSize, source.Count());
        }

        public PageList(IList<T> source, int currentPage, int pageSize, int dataCount)
        {
            Initialize(source, currentPage, pageSize, dataCount);
        }

        public PageList(IEnumerable<T> source, int currentPage, int pageSize, int dataCount)
        {
            Initialize(source, currentPage, pageSize, dataCount);
        }

        protected void Initialize(IEnumerable<T> source, int currentPage, int pageSize, int dataCount)
        {
            this.DataCount = dataCount;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            if (this.DataCount > 0)
                this.PageCount = (int)Math.Ceiling(this.DataCount / (double)pageSize);
            else
                this.PageCount = 1;

            if (source.Count() > pageSize)
                source = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            AddRange(source);
        }

        protected void Initialize(IList<T> source, int currentPage, int pageSize, int dataCount)
        {
            this.DataCount = dataCount;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            if (this.DataCount > 0)
                this.PageCount = (int)Math.Ceiling(this.DataCount / (double)pageSize);
            else
                this.PageCount = 1;
            if(source.Count>pageSize)
                source = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            AddRange(source);
        }

        protected void Initialize(IQueryable<T> source, int currentPage, int pageSize, int dataCount)
        {
            this.DataCount = source.Count();
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            if (this.DataCount > 0)
                this.PageCount = (int)Math.Ceiling(this.DataCount / (double)pageSize);
            else
                this.PageCount = 1;

            AddRange(source.Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToList());
        }
    }
}
