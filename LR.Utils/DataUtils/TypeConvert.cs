using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections;

namespace LR.Utils.DataUtils
{
    /// <summary>
    /// 数据类型解析类 
    /// </summary>
    public static class TypeConvert
    {
        /// <summary>
        /// Convert data table to IList object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="dataTable">Data table object</param>
        /// <returns>IList object</returns>
        public static IList<T> DataTableToIList<T>(DataTable dataTable)
        {
            // 0. Check parameter
            if (dataTable == null)
                return null;


            // 1. Declare object
            IList<T> result = new List<T>();
            Type type = typeof(T);    // Get type of object
            T obj = default(T);          // Declare temp value


            // 2. Set value to object
            object valueToAssign = null;
            object valObj = null;
            foreach (DataRow dataRow in dataTable.Rows)    // Loop by row
            {
                obj = (T)Activator.CreateInstance(type);           // Create instance

                foreach (DataColumn dataColumn in dataTable.Columns)          // Loop by column
                {
                    valObj = dataRow[dataColumn.ColumnName];
                    if (valObj == null || valObj == DBNull.Value || string.IsNullOrEmpty(valObj.ToString()))
                        continue;

                    //update by brian.chen 20110914
                    PropertyInfo property = type.GetProperty(dataColumn.ColumnName);
                    if (property != null)
                    {
                        valueToAssign = null;

                        // 对可空类型的判断
                        if (property.PropertyType.IsGenericType &&
                            property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            valueToAssign = Convert.ChangeType(valObj, property.PropertyType.GetGenericArguments()[0]);
                        }
                        else
                        {
                            valueToAssign = Convert.ChangeType(valObj, property.PropertyType);
                        }

                        if (valueToAssign != null)
                        {
                            property.SetValue(obj, valueToAssign, null);
                        }
                    }

                    //foreach (PropertyInfo viewProperty in type.GetProperties())   // Property list of T
                    //{
                    //    if (viewProperty.Name == dataColumn.ColumnName)
                    //    {
                    //        valueToAssign = null;

                    //        // 对可空类型的判断
                    //        if (viewProperty.PropertyType.IsGenericType &&
                    //            viewProperty.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    //        {
                    //            valueToAssign = Convert.ChangeType(valObj, viewProperty.PropertyType.GetGenericArguments()[0]);
                    //        }
                    //        else
                    //        {
                    //            valueToAssign = Convert.ChangeType(valObj, viewProperty.PropertyType);
                    //        }

                    //        if (valueToAssign != null)
                    //        {
                    //            viewProperty.SetValue(obj, valueToAssign, null);
                    //        }

                    //        break;
                    //    }
                    //}
                }

                result.Add(obj);
            }


            // End. Return result
            return result;
        }

        #region 基础判别函数
        /// <summary>
        /// 对象是否非空
        /// 为空返回 false
        /// 不为空返回 true
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <returns>bool值</returns>
        public static bool NotNull(object Object) { return !IsNull(Object, false); }
        /// <summary>
        /// 对象是否非空
        /// 为空返回 false
        /// 不为空返回 true
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsRemoveSpace">是否去除空格</param>
        /// <returns>bool值</returns>
        public static bool NotNull(object Object, bool IsRemoveSpace) { return !IsNull(Object, IsRemoveSpace); }
        /// <summary>
        /// 对象是否为空
        /// 为空返回 false
        /// 不为空返回 true
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <returns>bool值</returns>
        public static bool IsNull(object Object) { return IsNull(Object, false); }
        /// <summary>
        /// 对象是否为空
        /// 为空返回 false
        /// 不为空返回 true
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsRemoveSpace">是否去除空格</param>
        /// <returns>bool值</returns>
        public static bool IsNull(object Object, bool IsRemoveSpace)
        {
            if (Object == null) return true;
            string Objects = Object.ToString();
            if (Objects == "") return true;
            if (IsRemoveSpace)
            {
                if (Objects.Replace(" ", "") == "") return true;
                if (Objects.Replace("　", "") == "") return true;
            }
            return false;
        }
        /// <summary>
        /// 对象是否为bool值
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <returns>bool值</returns>
        public static bool IsBool(object Object) { return IsBool(Object, false); }
        /// <summary>
        /// 判断是否为bool值
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="Default">默认bool值</param>
        /// <returns>bool值</returns>
        public static bool IsBool(object Object, bool Default)
        {
            if (IsNull(Object)) return Default;
            try { return bool.Parse(Object.ToString()); }
            catch { return Default; }
        }
        /// <summary>
        /// 是否邮件地址
        /// </summary>
        /// <param name="Mail">等待验证的邮件地址</param>
        /// <returns>bool</returns>
        public static bool IsMail(string Mail) { return Regex.IsMatch(Mail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"); }
        /// <summary>
        /// 是否URL地址
        /// </summary>
        /// <param name="HttpUrl">等待验证的Url地址</param>
        /// <returns>bool</returns>
        public static bool IsHttp(string HttpUrl) { return Regex.IsMatch(HttpUrl, @"^(http|https):\/\/[A-Za-z0-9%\-_@]+\.[A-Za-z0-9%\-_@]{2,}[A-Za-z0-9\.\/=\?%\-&_~`@[\]:+!;]*$"); }
        /// <summary>
        /// 取字符左函数
        /// </summary>
        /// <param name="Object">要操作的 string  数据</param>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>string</returns>
        public static string Left(object Object, int MaxLength)
        {
            if (IsNull(Object)) return "";
            return Object.ToString().Substring(0, Math.Min(Object.ToString().Length, MaxLength));
        }
        /// <summary>
        /// 取字符中间函数
        /// </summary>
        /// <param name="Object">要操作的 string  数据</param>
        /// <param name="StarIndex">开始的位置索引</param>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>string</returns>
        public static string Mid(string Object, int StarIndex, int MaxLength)
        {
            if (IsNull(Object)) return "";
            if (StarIndex >= Object.Length) return "";
            return Object.Substring(StarIndex, MaxLength);
        }
        /// <summary>
        /// 取字符右函数
        /// </summary>
        /// <param name="Object">要操作的 string  数据</param>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>string</returns>
        public static string Right(object Object, int MaxLength)
        {
            if (IsNull(Object)) return "";
            int i = Object.ToString().Length;
            if (i < MaxLength) { MaxLength = i; i = 0; } else { i = i - MaxLength; }
            return Object.ToString().Substring(i, MaxLength);
        }
        /// <summary>
        /// 判断是否包含Http头的地址
        /// </summary>
        /// <param name="Object">要操作的对象</param>
        /// <returns>bool</returns>
        public static bool IsHttpUrl(string Object)
        {
            if (IsNull(Object)) return false;
            if (Left(Object, 7).ToLower() == "http://") return true;
            return false;
        }
        /// <summary>
        /// 数字前导加0
        /// </summary>
        /// <param name="Int">要操作的 int  数据</param>
        /// <param name="MaxLength">最大长度</param>
        /// <returns>string</returns>
        public static string AddZeros(int Int, int MaxLength) { return AddZeros(Int.ToString(), MaxLength); }
        /// <summary>
        /// 数字前导加0
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="MaxLength">默认长度不加0</param>
        /// <returns>字符</returns>
        public static string AddZeros(string Object, int MaxLength)
        {
            int iLength = Object.Length;
            if (iLength < MaxLength)
            {
                for (int i = 1; i <= (MaxLength - iLength); i++) Object = "0" + Object;
            }
            return Object;
        }
        #endregion
        #region 字符转换函数
        /// <summary>
        /// 字符 int  转换为 char
        /// </summary>
        /// <param name="Int">字符[int]</param>
        /// <returns>char</returns>
        public static char IntToChar(int Int) { return (char)Int; }
        /// <summary>
        /// 字符 int  转换为字符 string
        /// </summary>
        /// <param name="Int">字符 int</param>
        /// <returns>字符 string</returns>
        public static string IntToString(int Int) { return IntToChar(Int).ToString(); }
        /// <summary>
        /// 字符 string  转换为字符 int
        /// </summary>
        /// <param name="Strings">字符 string</param>
        /// <returns>字符 int</returns>
        public static int StringToInt(string Strings)
        {
            if (IsNull(Strings)) return -100; char[] chars = Strings.ToCharArray(); return (int)chars[0];
        }
        /// <summary>
        /// 字符 string  转换为 char
        /// </summary>
        /// <param name="Strings">字符 string</param>
        /// <returns>char</returns>
        public static char StringToChar(string Strings) { return IntToChar(StringToInt(Strings)); }
        #endregion
        #region 操作 int  数据
        /// <summary>
        /// 对象是否为 int  类型数据
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsTrue">返回是否转换成功</param>
        /// <returns>int值</returns>
        private static int IsInt(object Object, out bool IsTrue)
        {
            try { IsTrue = true; return int.Parse(Object.ToString()); }
            catch { IsTrue = false; return 0; }
        }
        /// <summary>
        /// 转换成为 int  数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>int 数据</returns>
        public static int ToInt(object Object) { return ToInt(Object, 0); }
        /// <summary>
        /// 转换成为 int  数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <returns>int 数据</returns>
        public static int ToInt(object Object, int Default) { return ToInt(Object, Default, 0, 999999999); }
        /// <summary>
        /// 转换成为 int  数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinInt"> 下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>int 数据</returns>
        public static int ToInt(object Object, int Default, int MinInt) { return ToInt(Object, Default, MinInt, 999999999); }
        /// <summary>
        /// 转换成为 int  数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinInt"> 下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <param name="MaxInt">上界限定的最大值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>int 数据</returns>
        public static int ToInt(object Object, int Default, int MinInt, int MaxInt)
        {
            bool IsTrue = false;
            int Int = IsInt(Object, out IsTrue);
            if (!IsTrue) return Default;
            if (Int < MinInt || Int > MaxInt) return Default;
            return Int;
        }
        #endregion
        #region 操作 long  数据
        /// <summary>
        /// 对象是否为 long  类型数据
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsTrue">返回是否转换成功</param>
        /// <returns>long值</returns>
        private static long IsLong(object Object, out bool IsTrue)
        {
            try { IsTrue = true; return long.Parse(Object.ToString()); }
            catch { IsTrue = false; return 0; }
        }
        /// <summary>
        /// 转换成为 Long 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>Long 数据</returns>
        public static long ToLong(object Object) { return ToLong(Object, 0); }
        /// <summary>
        /// 转换成为 Long 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <returns>Long 数据</returns>
        public static long ToLong(object Object, long Default) { return ToLong(Object, Default, -9223372036854775808, 9223372036854775807); }
        /// <summary>
        /// 转换成为 long 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">转换不成功返回的默认值</param>
        /// <param name="MinLong">下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>long 数据</returns>
        public static long ToLong(object Object, long Default, long MinLong) { return ToLong(Object, Default, MinLong, 9223372036854775807); }
        /// <summary>
        /// 转换成为 long 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinLong">下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <param name="MaxLong">上界限定的最大值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>long 数据</returns>
        public static long ToLong(object Object, long Default, long MinLong, long MaxLong)
        {
            bool IsTrue = false;
            long Long = IsLong(Object, out IsTrue);
            if (!IsTrue) return Default;
            if (Long < MinLong || Long > MaxLong) return Default;
            return Long;
        }
        #endregion
        #region 操作 float  数据
        /// <summary>
        /// 对象是否为 float  类型数据
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsTrue">返回是否转换成功</param>
        /// <returns>float值</returns>
        private static float IsFloat(object Object, out bool IsTrue)
        {
            try { IsTrue = true; return float.Parse(Object.ToString()); }
            catch { IsTrue = false; return 0; }
        }
        /// <summary>
        /// 转换成为 float 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>float 数据</returns>
        public static float ToFloat(object Object) { return ToFloat(Object, 0); }
        /// <summary>
        /// 转换成为 float 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <returns>float 数据</returns>
        public static float ToFloat(object Object, float Default) { return ToFloat(Object, Default, 0, 999999999); }
        /// <summary>
        /// 转换成为 float 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinFloat"> 小于等于 转换成功后,下界限定的最小值,若超过范围 则返回 默认值</param>
        /// <returns>float 数据</returns>
        public static float ToFloat(object Object, float Default, float MinFloat) { return ToFloat(Object, Default, MinFloat, 999999999); }
        /// <summary>
        /// 转换成为 float 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinFloat"> 下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <param name="MaxFloat"> 上界限定的最大值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>float 数据</returns>
        public static float ToFloat(object Object, float Default, float MinFloat, float MaxFloat)
        {
            bool IsTrue = false;
            float Float = IsFloat(Object, out IsTrue);
            if (!IsTrue) return Default;
            if (Float < MinFloat || Float > MaxFloat) return Default;
            return Float;
        }
        #endregion
        #region 操作 decimal  数据
        /// <summary>
        /// 对象是否为 decimal  类型数据
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsTrue">返回是否转换成功</param>
        /// <returns>decimal值</returns>
        private static decimal IsDecimal(object Object, out bool IsTrue)
        {
            try { IsTrue = true; return decimal.Parse(Object.ToString()); }
            catch { IsTrue = false; return 0; }
        }
        /// <summary>
        /// 转换成为 decimal 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <returns>decimal 数据</returns>
        public static decimal ToDecimal(object Object) { return ToDecimal(Object, 0); }
        /// <summary>
        /// 转换成为 decimal 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <returns>decimal 数据</returns>
        public static decimal ToDecimal(object Object, decimal Default) { return ToDecimal(Object, Default, 0, 999999999); }

        /// <summary>
        /// 转换成为 decimal 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinFloat"> 小于等于 转换成功后,下界限定的最小值,若超过范围 则返回 默认值</param>
        /// <returns>decimal 数据</returns>
        public static decimal ToDecimal(object Object, decimal Default, decimal MinFloat) { return ToDecimal(Object, Default, MinFloat, 999999999); }
        /// <summary>
        /// 转换成为 decimal 数据
        /// </summary>
        /// <param name="Object">要转换的对象</param>
        /// <param name="Default">默认值</param>
        /// <param name="MinDecimal"> 下界限定的最小值 , 若超过范围 , 则返回 默认值</param>
        /// <param name="MaxDecimal"> 上界限定的最大值 , 若超过范围 , 则返回 默认值</param>
        /// <returns>decimal 数据</returns>
        public static decimal ToDecimal(object Object, decimal Default, decimal MinDecimal, decimal MaxDecimal)
        {
            bool IsTrue = false;
            decimal Decimal = IsDecimal(Object, out IsTrue);
            if (!IsTrue) return Default;
            if (Decimal < MinDecimal || Decimal > MaxDecimal) return Default;
            return Decimal;
        }
        #endregion
        #region 操作 dateTime 数据
        /// <summary>
        /// 是否为时间格式
        /// </summary>
        /// <param name="Object">要判断的对象</param>
        /// <param name="IsTrue">返回是否转换成功</param>
        /// <returns>DateTime</returns>
        public static DateTime IsTime(object Object, out bool IsTrue)
        {
            IsTrue = false;
            if (IsNull(Object)) return DateTime.Now;
            try { IsTrue = true; return DateTime.Parse(Object.ToString()); }
            catch { IsTrue = false; return DateTime.Now; }
        }
        /// <summary>
        /// 获得当前时间
        /// </summary>
        /// <param name="Style">时间样式</param>
        /// <returns>string</returns>
        public static string ToNow(string Style) { return DateTime.Now.ToString(Style); }
        /// <summary>
        /// 转换字符串为格式化时间字符串
        /// </summary>
        /// <param name="Object">要操作的字符</param>
        /// <returns>string</returns>
        public static string ToTime(string Object) { return ToTime(Object, "yyyy-MM-dd"); }
        /// <summary>
        /// 转换字符串为格式化时间字符串
        /// </summary>
        /// <param name="Object">要操作的字符</param>
        /// <param name="Style">格式化样式</param>
        /// <returns>string</returns>
        public static string ToTime(string Object, string Style) { return ToTime(Object, DateTime.Now, Style); }
        /// <summary>
        /// 转换字符串为格式化时间字符串
        /// </summary>
        /// <param name="Object">要操作的字符</param>
        /// <param name="Default">默认时间</param>
        /// <returns>string</returns>
        public static string ToTime(string Object, DateTime Default) { return ToTime(Object, Default, "yyyy-MM-dd"); }
        /// <summary>
        /// 转换字符串为格式化时间字符串
        /// </summary>
        /// <param name="Object">要操作的字符</param>
        /// <param name="Default">默认时间</param>
        /// <param name="Style">格式化样式</param>
        /// <returns>string</returns>
        public static string ToTime(string Object, DateTime Default, string Style)
        {
            if (IsNull(Object)) return Default.ToString(Style);
            bool IsTrue = false;
            DateTime Time = IsTime(Object, out IsTrue);
            if (!IsTrue) return Default.ToString(Style);
            return Time.ToString(Style);
        }
        #endregion

        #region Json To Entity, Merge from LR.Common.JsToEntity
        /// <summary>
        /// 从字符串中选择符合的实体。其中需指定实体不为空的字段名称
        /// </summary>
        /// <param name="js">Json 字符串</param>
        /// <param name="check">实体属性不为空参数，如：{"A","B"}</param>
        /// <returns></returns>
        public static List<T> GetEntityList<T>(string js, string[] check)
        {
            List<T> entityList = JsonConvert.DeserializeObject<List<T>>(js);
            List<T> getList = new List<T>();
            foreach (var c in entityList)
            {
                for (var i = 0; i < check.Length; i++)
                {
                    var pra = check[i];
                    var value = c.GetType().GetProperty(pra).GetValue(c, null);
                    if (value != null)
                    {
                        getList.Insert(0, c);
                        break;
                    }
                }

            }
            return getList;
        }
        #endregion

        #region List To DataTable , Merge from ListToDataTable
        /// <summary>
        /// 把IList数据类型转换成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="propertyName"></param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(IList<T> list, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
                propertyNameList.AddRange(propertyName);

            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
        #endregion

        #region List To Json, Merge from ListToJson
        /// <summary>
        /// List转成json 数据共用方法，根据传属性名得到相关转换 
        /// </summary>
        /// <typeparam name="list">转换的实体集合</typeparam>
        /// <param name="propertyName">实体类对应的属性</param>
        /// <param name="jsonShowName">转换后的属性显示名</param>
        /// <returns></returns>
        public static string GetJsonData<T>(IList<T> list, string propertyName, string jsonShowName)
        {
            string[] propertyNameArr = propertyName.Split(',');//实体类属性
            string[] jsonShowNameArr = jsonShowName.Split(',');//转化为UI实用属性名

            StringBuilder Json = new StringBuilder();
            Json.Append("[");

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Type type = list[i].GetType();
                    Json.Append("{");
                    for (int j = 0; j < propertyNameArr.Length; j++)
                    {
                        PropertyInfo property = type.GetProperty(propertyNameArr[j]);
                        if (property != null)
                        {
                            Json.Append(jsonShowNameArr[j]);
                            Json.Append(":'");
                            Json.Append(StringFormat(property.GetValue(list[i], null) == null ? "" : property.GetValue(list[i], null).ToString(), type));
                            Json.Append("'");
                        }
                        if (j < propertyNameArr.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]");
            return Json.ToString();
        }


        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append(@"<A>\\\</A>"); break;
                    case '\\':
                        sb.Append("<A>\\\\</A>"); break;
                    case '/':
                        sb.Append("<A>\\/</A>"); break;
                    case '\b':
                        sb.Append("<A>\\b</A>"); break;
                    case '\f':
                        sb.Append("<A>\\f</A>"); break;
                    case '\n':
                        sb.Append("<A>\\n</A>"); break;
                    case '\r':
                        sb.Append("<A>\\r</A>"); break;
                    case '\t':
                        sb.Append("<A>\\t</A>"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 格式化字符型、日期型、布尔型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            return str;
        }

        /// <summary>
        /// 获取已保存的值，绑定下拉框
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="list">转换的实体集合</typeparam>
        /// <param name="propertyName">实体类对应的属性</param>
        /// <param name="jsonShowName">转换后的属性显示名</param>
        /// <param name="Ids">保存需显示为选中状态的下拉框</param>
        /// <param name="name">需进行比较的实体类属性</param>
        /// <returns></returns>
        public static string ListToJsonSelected<T>(IList<T> list, string propertyName, string jsonShowName, string Ids, string name)
        {
            string[] propertyNameArr = propertyName.Split(',');//实体类属性
            string[] jsonShowNameArr = jsonShowName.Split(',');//转化为UI实用属性名
            string[] IdsList = Ids.Split(',');

            StringBuilder Json = new StringBuilder();
            Json.Append("[");

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Type type = list[i].GetType();
                    Json.Append("{");
                    for (int j = 0; j < propertyNameArr.Length; j++)
                    {
                        PropertyInfo property = type.GetProperty(propertyNameArr[j]);
                        if (property != null)
                        {
                            Json.Append(jsonShowNameArr[j]);
                            Json.Append(":'");
                            Json.Append(ListToJson.StringFormat(property.GetValue(list[i], null) == null ? "" : property.GetValue(list[i], null).ToString(), type));
                            Json.Append("'");
                        }
                        if (j < propertyNameArr.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    PropertyInfo propertys = type.GetProperty(name);
                    if (propertys.GetValue(list[i], null) != null)
                    {
                        for (int ii = 0; ii < IdsList.Length; ii++)
                        {
                            if (IdsList[ii] == propertys.GetValue(list[i], null).ToString())
                            {
                                Json.Append(",selected:true");
                            }
                        }
                    }

                    Json.Append("}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]");
            return Json.ToString();
        }
        #endregion
    }
}
