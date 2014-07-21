using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace LR.Utils.DataUtils
{
    /// <summary>
    /// XML帮助类
    /// </summary>
    public class XmlHelper
    {
        protected string strXml;
        protected XmlDocument objXmlDoc = new XmlDocument();
        public XmlDocument Document
        {
            get { return objXmlDoc; }
        }

        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="XmlFile">The XML file.</param>
        public XmlHelper(string xmlString)
        {
            if (!string.IsNullOrEmpty(xmlString))
            {
                try
                {
                    objXmlDoc.LoadXml(xmlString);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                strXml = xmlString;
            }
        }

        /// <summary>
        /// 地址参数构造函数
        /// </summary>
        /// <param name="path"></param>
        public XmlHelper(Uri path)
        {
            try
            {
                objXmlDoc.Load(path.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        ///  获取多个节点
        /// </summary>
        /// <param name="xpath">The xpath.</param>
        /// <returns></returns>
        public XmlNodeList GetNodeList(string xpath)
        {
            XmlNodeList nodes = objXmlDoc.SelectNodes(xpath);
            return nodes;
        }


        /// <summary>
        ///  获取单个节点
        /// </summary>
        /// <param name="xpath">The xpath.</param>
        /// <returns></returns>
        public XmlNode GetNode(string xpath)
        {
            return objXmlDoc.SelectSingleNode(xpath);
        }


        /// <summary>
        /// 获取节点值
        /// </summary>
        /// <param name="xmlPathNode">节点Path，例如：/aaaa/bbbb/cccc</param>
        /// <returns></returns>
        public string GetNodeValue(string xmlPathNode)
        {
            XmlNode node = objXmlDoc.SelectSingleNode(xmlPathNode);
            if (node != null)
                return node.InnerText;
            else
                return string.Empty;
        }

        /// <summary>
        /// 获取节点属性值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public string GetAttrValue(XmlNode node, string attrName)
        {
            if (node == null) return string.Empty;

            XmlAttribute attr = node.Attributes[attrName];

            if (attr == null)
                return string.Empty;
            else
                return attr.Value;
        }


        /// <summary>
        /// 获取节点值
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childNodeName"></param>
        /// <returns></returns>
        public string GetChildNodeValue(XmlNode parentNode, string childNodeName)
        {
            if (parentNode == null || string.IsNullOrEmpty(childNodeName))
                return string.Empty;

            XmlNode node = parentNode[childNodeName];

            if (node != null)
                return node.InnerText;
            else
                return string.Empty;
        }

        /// <summary>
        /// 判断父节点有没有指定名字的孩子
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childNodeName"></param>
        /// <returns></returns>
        public static bool HasChildNode(XmlNode parentNode, string childNodeName)
        {
            childNodeName = childNodeName.ToUpper();
            XmlNodeList xnl = parentNode.ChildNodes;
            foreach (XmlNode node in xnl)
            {
                if (node.Name.ToUpper() == childNodeName)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 根据父节点获取子节点值
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childNodeName"></param>
        /// <returns></returns>
        public static string GetXmlChildNodeValue(XmlNode parentNode, string childNodeName)
        {
            if (parentNode == null || string.IsNullOrEmpty(childNodeName))
                return string.Empty;

            XmlNode node = parentNode[childNodeName];

            if (node != null)
                return node.InnerText;
            else
                return string.Empty;
        }



        /// <summary>
        /// 获取节点属性值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static string GetXmlAttrValue(XmlNode node, string attrName)
        {
            if (node == null) return string.Empty;

            XmlAttribute attr = node.Attributes[attrName];

            if (attr == null)
                return string.Empty;
            else
                return attr.Value;
        }
    }
}
