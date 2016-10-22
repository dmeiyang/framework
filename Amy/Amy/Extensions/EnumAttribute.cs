/* ==================================================================
* 类 名 称：EnumAttribute
* 版 本 号：v1.0.0
* 作 者: Administrator
* 创建时间：2015/11/2 15:23:25
* 类 说 明：
* 备 注：
*
* ==================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System
{
    /// <summary>
    ///     把枚举值按照指定的文本显示
    ///     <remarks>
    ///         一般通过枚举值的ToString()可以得到变量的文本，
    ///         但是有时候需要的到与之对应的更充分的文本，
    ///         这个类帮助达到此目的
    ///     </remarks>
    /// </summary>
    /// <example>
    ///     [EnumDescription("中文数字")]
    ///     enum MyEnum
    ///     {
    ///     [EnumDescription("数字一")]
    ///     One = 1,
    ///     [EnumDescription("数字二")]
    ///     Two,
    ///     [EnumDescription("数字三")]
    ///     Three
    ///     }
    ///     EnumDescription.GetEnumText(typeof(MyEnum));
    ///     EnumDescription.GetFieldText(MyEnum.Two);
    ///     EnumDescription.GetFieldTexts(typeof(MyEnum));
    /// </example>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class EnumDescription : Attribute
    {
        private readonly string _enumDisplayText;
        private readonly int _enumRank;
        private FieldInfo _fieldIno;

        /// <summary>
        ///     描述枚举值
        /// </summary>
        /// <param name="enumDisplayText">描述内容</param>
        /// <param name="enumRank">排列顺序</param>
        public EnumDescription(string enumDisplayText, int enumRank)
        {
            _enumDisplayText = enumDisplayText;
            _enumRank = enumRank;
        }

        /// <summary>
        ///     描述枚举值，默认排序为5
        /// </summary>
        /// <param name="enumDisplayText">描述内容</param>
        public EnumDescription(string enumDisplayText)
            : this(enumDisplayText, 5)
        {
        }

        public string EnumDisplayText
        {
            get { return _enumDisplayText; }
        }

        public int EnumRank
        {
            get { return _enumRank; }
        }

        public int EnumValue
        {
            get { return (int)_fieldIno.GetValue(null); }
        }

        public string FieldName
        {
            get { return _fieldIno.Name; }
        }

        #region  对枚举描述属性的解释相关函数
        /// <summary>
        /// 得到对枚举的描述文本
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static string GetOptions(Type enumType)
        {
            var eds = GetFieldTexts(enumType);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var v in eds)
            {
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", v.FieldName, v.EnumDisplayText);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 得到对枚举的描述文本
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static string GetEnumText(Type enumType)
        {
            var eds = (EnumDescription[])enumType.GetCustomAttributes(typeof(EnumDescription), false);
            if (eds.Length != 1)
                return string.Empty;
            return eds[0].EnumDisplayText;
        }

        /// <summary>
        /// 获得指定枚举类型中，指定值的描述文本。
        /// </summary>
        /// <param name="enumValue">枚举值，不要作任何类型转换</param>
        /// <returns>描述字符串</returns>
        public static string GetFieldText(object enumValue)
        {
            EnumDescription[] descriptions = GetFieldTexts(enumValue.GetType(), SortType.Default);
            foreach (EnumDescription ed in descriptions)
            {
                if (ed._fieldIno.Name == enumValue.ToString()) return ed.EnumDisplayText;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获得指定枚举类型中，指定值的描述文本。
        /// </summary>
        /// <param name="Type">枚举类型</param>
        /// <param name="enumValue">枚举值名称，不要作任何类型转换</param>
        /// <returns>描述字符串</returns>
        public static string GetFieldText(Type enumType, string enumName)
        {
            EnumDescription[] descriptions = GetFieldTexts(enumType, SortType.Default);
            foreach (EnumDescription ed in descriptions)
            {
                if (ed._fieldIno.Name == enumName) return ed.EnumDisplayText;
            }
            return string.Empty;
        }

        /// <summary>
        /// 得到枚举类型定义的所有文本，按定义的顺序返回
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>所有定义的文本</returns>
        public static EnumDescription[] GetFieldTexts(Type enumType)
        {
            return GetFieldTexts(enumType, SortType.Default);
        }

        /// <summary>
        /// 得到枚举类型定义的所有文本
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="sortType">指定排序类型</param>
        /// <returns>所有定义的文本</returns>
        public static EnumDescription[] GetFieldTexts(Type enumType, SortType sortType)
        {
            Hashtable cachedEnum = new Hashtable();

            //缓存中没有找到，通过反射获得字段的描述信息
            if (cachedEnum.Contains(enumType.FullName) == false)
            {
                FieldInfo[] fields = enumType.GetFields();
                var edAL = new ArrayList();
                foreach (FieldInfo fi in fields)
                {
                    object[] eds = fi.GetCustomAttributes(typeof(EnumDescription), false);
                    if (eds.Length != 1) continue;
                    ((EnumDescription)eds[0])._fieldIno = fi;
                    edAL.Add(eds[0]);
                }

                cachedEnum.Add(enumType.FullName, edAL.ToArray(typeof(EnumDescription)));
            }
            var descriptions = (EnumDescription[])cachedEnum[enumType.FullName];
            if (descriptions.Length <= 0)
                //Log.Error("枚举类型[" + enumType.Name + "]未定义属性EnumValueDescription",null);

                //按指定的属性冒泡排序
                for (int m = 0; m < descriptions.Length; m++)
                {
                    //默认就不排序了
                    if (sortType == SortType.Default) break;

                    for (int n = m; n < descriptions.Length; n++)
                    {
                        bool swap = false;

                        switch (sortType)
                        {
                            case SortType.Default:
                                break;
                            case SortType.DisplayText:
                                if (
                                    String.CompareOrdinal(descriptions[m].EnumDisplayText,
                                        descriptions[n].EnumDisplayText) > 0)
                                    swap = true;
                                break;
                            case SortType.Rank:
                                if (descriptions[m].EnumRank > descriptions[n].EnumRank) swap = true;
                                break;
                        }

                        if (swap)
                        {
                            EnumDescription temp = descriptions[m];
                            descriptions[m] = descriptions[n];
                            descriptions[n] = temp;
                        }
                    }
                }

            return descriptions;
        }

        #endregion
    }

    /// <summary>
    ///     排序类型
    /// </summary>
    public enum SortType
    {
        /// <summary>
        ///     按枚举顺序默认排序
        /// </summary>
        Default,

        /// <summary>
        ///     按描述值排序
        /// </summary>
        DisplayText,

        /// <summary>
        ///     按排序熵
        /// </summary>
        Rank
    }
}
