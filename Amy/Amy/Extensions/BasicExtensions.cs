/* ==================================================================
* 类 名 称：BasicExtensions
* 版 本 号：v1.0.0
* 作 者: Administrator
* 创建时间：2015/11/2 15:05:14
* 类 说 明：
* 备 注：
*
* ==================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class BasicExtensions
    {
        #region 扩展string

        #region 类型转换
        /// <summary>
        /// String转换成Int32
        /// </summary>
        public static int ToInt32(this string original)
        {
            int value = default(int);
            int.TryParse(original, out value);
            return value;
        }

        /// <summary>
        /// String转换成Long
        /// </summary>
        public static long ToInt64(this string original)
        {
            long r = 0;
            bool b = long.TryParse(original, out r);
            return r;
        }

        /// <summary>
        /// String转换成DateTime
        /// </summary>
        public static DateTime ToDateTime(this string original)
        {
            DateTime value = default(DateTime);
            DateTime.TryParse(original, out value);
            return value;
        }

        /// <summary>
        /// String转换成double
        /// </summary>
        public static double ToDouble(this string original)
        {
            double value = default(double);
            double.TryParse(original, out value);
            return value;
        }

        /// <summary>
        /// String转换成bool
        /// </summary>
        public static bool ToBoolean(this string original)
        {
            if (original == "1")
                return true;
            bool r = false;
            bool.TryParse(original, out r);
            return r;
        }

        /// <summary>
        /// 转换成安全字符
        /// </summary>
        public static string ToSafeTrim(this string original)
        {
            if (original != null)
            {
                return original.Trim();
            }
            return string.Empty;
        }

        /// <summary>
        /// 字符串MD5加密
        /// </summary>
        public static string ToMD5(this string original)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(original, "md5").ToLower();
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] ToSplit(this string original, char separator)
        {
            return original.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] ToSplit(this string original, string separator)
        {
            return original.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }
        #endregion

        #region 验证格式
        /// <summary>
        /// 验证电话号码
        /// </summary>
        public static bool IsTelephone(this string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^(\d{3,4}-)?\d{6,8}$");
        }

        /// <summary>
        /// 验证手机号
        /// </summary>
        public static bool IsPhone(this string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^[1][3-5,8]\d{9}$");
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        public static bool IsEmail(this string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
        #endregion

        #region 其他情况
        /// <summary>
        /// 生成描述
        /// </summary>
        public static string ToDescription(this string value, int length)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            if (Encoding.UTF8.GetByteCount(value) <= length * 2)
                return value;

            value = value.ToDropHTML();

            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(value);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
                tempString += value.Substring(i, 1);
                if (tempLen >= (length - 1) * 2)
                    break;
            }
            //如果截过则加上半个省略号
            if (System.Text.Encoding.Default.GetBytes(value).Length > length)
                tempString += "...";
            return tempString;
        }

        /// <summary>
        /// 删除Html标签
        /// </summary>
        public static string ToDropHTML(this string value)
        {
            string[] aryReg ={
          @"<script[^>]*?>.*?</script>",

          @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
          @"([\r\n])[\s]+",
          @"&(quot|#34);",
          @"&(amp|#38);",
          @"&(lt|#60);",
          @"&(gt|#62);",
          @"&(nbsp|#160);",
          @"&(iexcl|#161);",
          @"&(cent|#162);",
          @"&(pound|#163);",
          @"&(copy|#169);",
          @"&#(\d+);",
          @"-->",
          @"<!--.*\n"

         };

            string[] aryRep = {
           "",
           "",
           "",
           "\"",
           "&",
           "<",
           ">",
           " ",
           "\xa1",//chr(161),
           "\xa2",//chr(162),
           "\xa3",//chr(163),
           "\xa9",//chr(169),
           "",
           "\r\n",
           ""
          };

            string newReg = aryReg[0];
            string strOutput = value;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, aryRep[i]);
            }

            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");
            strOutput.Replace("&nbsp;", " ");

            return strOutput;
        }

        #region 汉子转拼音
        private static int[] pyvalue = new int[]{-20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,-20032,-20026,
                                            -20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,-19756,-19751,-19746,-19741,-19739,-19728,
                                            -19725,-19715,-19540,-19531,-19525,-19515,-19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,
                                            -19261,-19249,-19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,-19003,-18996,
                                            -18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,-18731,-18722,-18710,-18697,-18696,-18526,
                                            -18518,-18501,-18490,-18478,-18463,-18448,-18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183,
                                            -18181,-18012,-17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,-17733,-17730,
                                            -17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,-17468,-17454,-17433,-17427,-17417,-17202,
                                            -17185,-16983,-16970,-16942,-16915,-16733,-16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,
                                            -16452,-16448,-16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,-16212,-16205,
                                            -16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,-15933,-15920,-15915,-15903,-15889,-15878,
                                            -15707,-15701,-15681,-15667,-15661,-15659,-15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,
                                            -15408,-15394,-15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,-15149,-15144,
                                            -15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,-14941,-14937,-14933,-14930,-14929,-14928,
                                            -14926,-14922,-14921,-14914,-14908,-14902,-14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,
                                            -14663,-14654,-14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,-14170,-14159,
                                            -14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,-14109,-14099,-14097,-14094,-14092,-14090,
                                            -14087,-14083,-13917,-13914,-13910,-13907,-13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,
                                            -13611,-13601,-13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,-13340,-13329,
                                            -13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,-13068,-13063,-13060,-12888,-12875,-12871,
                                            -12860,-12858,-12852,-12849,-12838,-12831,-12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,
                                            -12320,-12300,-12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,-11781,-11604,
                                            -11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,-11055,-11052,-11045,-11041,-11038,-11024,
                                            -11020,-11019,-11018,-11014,-10838,-10832,-10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,
                                            -10329,-10328,-10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254};
        private static string[] pystr = new string[]{"a","ai","an","ang","ao","ba","bai","ban","bang","bao","bei","ben","beng","bi","bian","biao",
                                            "bie","bin","bing","bo","bu","ca","cai","can","cang","cao","ce","ceng","cha","chai","chan","chang","chao","che","chen",
                                            "cheng","chi","chong","chou","chu","chuai","chuan","chuang","chui","chun","chuo","ci","cong","cou","cu","cuan","cui",
                                            "cun","cuo","da","dai","dan","dang","dao","de","deng","di","dian","diao","die","ding","diu","dong","dou","du","duan",
                                            "dui","dun","duo","e","en","er","fa","fan","fang","fei","fen","feng","fo","fou","fu","ga","gai","gan","gang","gao",
                                            "ge","gei","gen","geng","gong","gou","gu","gua","guai","guan","guang","gui","gun","guo","ha","hai","han","hang",
                                            "hao","he","hei","hen","heng","hong","hou","hu","hua","huai","huan","huang","hui","hun","huo","ji","jia","jian",
                                            "jiang","jiao","jie","jin","jing","jiong","jiu","ju","juan","jue","jun","ka","kai","kan","kang","kao","ke","ken",
                                            "keng","kong","kou","ku","kua","kuai","kuan","kuang","kui","kun","kuo","la","lai","lan","lang","lao","le","lei",
                                            "leng","li","lia","lian","liang","liao","lie","lin","ling","liu","long","lou","lu","lv","luan","lue","lun","luo",
                                            "ma","mai","man","mang","mao","me","mei","men","meng","mi","mian","miao","mie","min","ming","miu","mo","mou","mu",
                                            "na","nai","nan","nang","nao","ne","nei","nen","neng","ni","nian","niang","niao","nie","nin","ning","niu","nong",
                                            "nu","nv","nuan","nue","nuo","o","ou","pa","pai","pan","pang","pao","pei","pen","peng","pi","pian","piao","pie",
                                            "pin","ping","po","pu","qi","qia","qian","qiang","qiao","qie","qin","qing","qiong","qiu","qu","quan","que","qun",
                                            "ran","rang","rao","re","ren","reng","ri","rong","rou","ru","ruan","rui","run","ruo","sa","sai","san","sang",
                                            "sao","se","sen","seng","sha","shai","shan","shang","shao","she","shen","sheng","shi","shou","shu","shua",
                                            "shuai","shuan","shuang","shui","shun","shuo","si","song","sou","su","suan","sui","sun","suo","ta","tai",
                                            "tan","tang","tao","te","teng","ti","tian","tiao","tie","ting","tong","tou","tu","tuan","tui","tun","tuo",
                                            "wa","wai","wan","wang","wei","wen","weng","wo","wu","xi","xia","xian","xiang","xiao","xie","xin","xing",
                                            "xiong","xiu","xu","xuan","xue","xun","ya","yan","yang","yao","ye","yi","yin","ying","yo","yong","you",
                                            "yu","yuan","yue","yun","za","zai","zan","zang","zao","ze","zei","zen","zeng","zha","zhai","zhan","zhang",
                                            "zhao","zhe","zhen","zheng","zhi","zhong","zhou","zhu","zhua","zhuai","zhuan","zhuang","zhui","zhun","zhuo",
                                            "zi","zong","zou","zu","zuan","zui","zun","zuo"};

        public static string ToSpelling(this string chrstr, bool isFrist)
        {
            byte[] array = new byte[2];
            string returnstr = "";
            int chrasc = 0; int i1 = 0;
            int i2 = 0;
            char[] nowchar = chrstr.ToCharArray();
            for (int j = 0; j < nowchar.Length; j++)
            {
                array = System.Text.Encoding.Default.GetBytes(nowchar[j].ToString());
                if (array == null || array.Length > 2 || array.Length <= 0)
                {
                    break;
                }
                if (array.Length == 1)
                {
                    returnstr += nowchar[j].ToString().ToLower();
                }
                else
                {
                    i1 = (short)(array[0]);
                    i2 = (short)(array[1]);
                    chrasc = i1 * 256 + i2 - 65536;
                    if (chrasc > 0 && chrasc < 160)
                    {
                        returnstr += nowchar[j];
                    }
                    else
                    {
                        for (int i = (pyvalue.Length - 1); i >= 0; i--)
                        {
                            if (pyvalue[i] <= chrasc)
                            {
                                if (isFrist)
                                {
                                    returnstr += pystr[i].Substring(0, 1);
                                }
                                else
                                {
                                    returnstr += pystr[i];
                                }
                                break;
                            }
                        }
                    }
                }
            }
            return returnstr.Replace("/", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).ToLower();
        }

        #endregion
        #endregion

        #endregion

        #region 扩展int

        #region 类型转换
        #endregion

        #region 验证格式
        #endregion

        #region 其他情况
        public static int Add(this int value)
        {
            return value + 1;
        }

        public static int Sub(this int value)
        {
            return value > 1 ? value - 1 : 0;
        }

        public static int Add(this int value, int count)
        {
            return value + count;
        }

        public static int Sub(this int value, int count)
        {
            return value > count ? value - count : 0;
        }
        #endregion

        #endregion

        #region 扩展DateTime

        #region 类型转换
        public static string ToFormatTime(this DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays > 60)
            {
                return dt.ToFormatTime("yyyy-MM-dd HH:mm");
            }
            else if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            else if (span.TotalDays > 14)
            {
                return "2周前";
            }
            else if (span.TotalDays > 7)
            {
                return "1周前";
            }
            else if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }
            else
            {
                return dt.ToFormatTime("yyyy-MM-dd HH:mm");
            }
        }

        public static string ToFormatTime(this DateTime? dt)
        {
            if (dt == null)
                return string.Empty;

            var date = Convert.ToDateTime(dt);

            TimeSpan span = DateTime.Now - date;
            if (span.TotalDays > 60)
            {
                return date.ToFormatTime("yyyy-MM-dd HH:mm");
            }
            else if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            else if (span.TotalDays > 14)
            {
                return "2周前";
            }
            else if (span.TotalDays > 7)
            {
                return "1周前";
            }
            else if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }
            else
            {
                return date.ToFormatTime("yyyy-MM-dd HH:mm");
            }
        }

        public static string ToFormatDay(this DateTime dt)
        {
            return dt.ToFormatTime("yyyy-MM-dd");
        }

        public static string ToFormatDay(this DateTime? dt)
        {
            if (dt == null)
                return string.Empty;

            var date = Convert.ToDateTime(dt);

            return date.ToFormatTime("yyyy-MM-dd");
        }

        public static string ToFormatMinute(this DateTime dt)
        {
            return dt.ToFormatTime("yyyy-MM-dd HH:mm");
        }

        public static string ToFormatMinute(this DateTime? dt)
        {
            if (dt == null)
                return string.Empty;

            var date = Convert.ToDateTime(dt);

            return date.ToFormatTime("yyyy-MM-dd HH:mm");
        }

        public static string ToShortFormatMinute(this DateTime dt)
        {
            return dt.ToFormatTime("HH:mm");
        }

        public static string ToShortFormatMinute(this DateTime? dt)
        {
            if (dt == null)
                return string.Empty;

            var date = Convert.ToDateTime(dt);

            return date.ToFormatTime("HH:mm");
        }

        private static string ToFormatTime(this DateTime dt, string format)
        {
            return dt.ToString(format);
        }

        public static int ToTimestamp(this DateTime dt)
        {
            TimeSpan ts = dt - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt32(ts.TotalSeconds);
        }
        #endregion

        #region 验证格式
        #endregion

        #region 其他情况
        #endregion

        #endregion

        #region byte[]与string互转
        public static string ByteToString(this byte[] bt)
        {
            if (bt == null)
                return string.Empty;

            return System.Text.Encoding.UTF8.GetString(bt);
        }

        public static byte[] ToByte(this string content)
        {
            if (string.IsNullOrEmpty(content))
                return new byte[0];

            return System.Text.Encoding.Default.GetBytes(content);
        }
        #endregion

        public static string ListToString(this List<string> list, string format)
        {
            if (list == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            foreach (var v in list)
            {
                sb.AppendFormat("{0},", v);
            }

            if (sb.Length > 0)
                sb.Length = sb.Length - 1;

            return sb.ToString();
        }

        public static string ArrayToString(this string[] list, string format)
        {
            if (list == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            foreach (var v in list)
            {
                sb.AppendFormat("{0},", v);
            }

            if (sb.Length > 0)
                sb.Length = sb.Length - 1;

            return sb.ToString();
        }
    }
}
