using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace LR.Utils.DataUtils
{
    public static class ListToJson
    {
        /// <summary>
        /// List转成json 数据共用方法，根据传属性名得到相关转换 
        /// </summary>
        /// <typeparam name="list">转换的实体集合</typeparam>
        /// <param name="propertyName">实体类对应的属性</param>
        /// <param name="jsonShowName">转换后的属性显示名</param>
        /// <returns></returns>
        public static string GetJsonData<T>(IEnumerable<T> list, string propertyName, string jsonShowName)
        {
            string[] propertyNameArr = propertyName.Split(',');//实体类属性
            string[] jsonShowNameArr = jsonShowName.Split(',');//转化为UI实用属性名
            int recordCount = 0;
            int propertyCount = 0;

            StringBuilder Json = new StringBuilder();
            Json.Append("[");

            foreach (T enti in list)
            {
                recordCount += 1;
                if (recordCount > 1)
                {
                    Json.Append(",");
                }

                propertyCount = 0;
                Type type = enti.GetType();

                Json.Append("{");
                for (int j = 0; j < propertyNameArr.Length; j++)
                {
                    PropertyInfo property = type.GetProperty(propertyNameArr[j]);
                    if (property != null)
                    {
                        propertyCount += 1;
                        if (propertyCount > 1)
                        {
                            Json.Append(",");
                        }

                        Json.Append(jsonShowNameArr[j]);
                        Json.Append(":'");
                        Json.Append(StringFormat(property.GetValue(enti, null) == null ? "" : property.GetValue(enti, null).ToString(), type));
                        Json.Append("'");
                    }
                }
                Json.Append("}");
            }

            Json.Append("]");
            return Json.ToString();
        }

        /// <summary>
        /// 将一个实体类转换成Json，haley
        /// </summary>
        /// <typeparam name="T">实体类的类型</typeparam>
        /// <param name="entity">要转换的实体类</param>
        /// <param name="propertyName">实体类对应的属性</param>
        /// <param name="jsonShowName">转换后的显示名称</param>
        /// <returns></returns>
        public static string GetEntityToJson<T>(T entity, string propertyName, string jsonShowName)
        {
            string[] propertyNameArr = propertyName.Split(',');//实体类属性
            string[] jsonShowNameArr = jsonShowName.Split(',');//转化为UI实用属性名
            int propertyCount = 0;

            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            Json.Append("{");
            Type type = entity.GetType();
            for (int j = 0; j < propertyNameArr.Length; j++)
            {
                PropertyInfo property = type.GetProperty(propertyNameArr[j]);
                if (property != null)
                {
                    propertyCount += 1;
                    if (propertyCount > 1)
                    {
                        Json.Append(",");
                    }

                    Json.Append(jsonShowNameArr[j]);
                    Json.Append(":'");
                    Json.Append(StringFormat(property.GetValue(entity, null) == null ? "" : property.GetValue(entity, null).ToString(), type));
                    Json.Append("'");
                }
            }
            Json.Append("}");
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
            int propertyCount = 0;

            StringBuilder Json = new StringBuilder();
            Json.Append("[");

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    propertyCount = 0;
                    Type type = list[i].GetType();
                    Json.Append("{");
                    for (int j = 0; j < propertyNameArr.Length; j++)
                    {
                        PropertyInfo property = type.GetProperty(propertyNameArr[j]);
                        if (property != null)
                        {
                            propertyCount += 1;
                            if (propertyCount > 1)
                            {
                                Json.Append(",");
                            }

                            Json.Append(jsonShowNameArr[j]);
                            Json.Append(":'");
                            Json.Append(ListToJson.StringFormat(property.GetValue(list[i], null) == null ? "" : property.GetValue(list[i], null).ToString(), type));
                            Json.Append("'");
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




    }
}
