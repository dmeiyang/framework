/* ==================================================================
* 类 名 称：IDHelper
* 版 本 号：v1.0.0
* 作 者: Administrator
* 创建时间：2015/2/27 18:11:01
* 类 说 明：
* 备 注：
*
* ==================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public class IDHelper
    {
        public static string Id16
        {
            get
            {
                long i = 1;

                foreach (byte b in Guid.NewGuid().ToByteArray())
                {
                    i *= ((int)b + 1);
                }

                return string.Format("{0:x}", i - DateTime.Now.Ticks); 
            }
        }

        public static string Id19
        {
            get
            {
                byte[] buffer = Guid.NewGuid().ToByteArray();

                return BitConverter.ToInt64(buffer, 0).ToString();  
            }
        }

        public static string Id32 { get { return Guid.NewGuid().ToString("N"); } }

        public static string Id36 { get { return Guid.NewGuid().ToString(); } }
    }
}
