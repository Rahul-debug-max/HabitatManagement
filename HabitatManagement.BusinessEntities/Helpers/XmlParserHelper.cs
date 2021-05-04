using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HabitatManagement.Business
{
    public class XmlParserHelper
    {
        /// <summary>
        /// Is Valid Xml
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsValidXml(string message)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(message.Trim());
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (XmlException)
            {
                return false;
            }
        }

        /// <summary>
        /// ConvertXMLMessageToObjectType
        /// </summary>
        /// <param name="xmlMessage"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public Object ConvertXMLMessageToObjectType(string xmlMessage, Type objectType)
        {
            StringReader stringReader = null;
            XmlSerializer xmlSerializer = null;
            XmlTextReader xmlTextReader = null;
            Object obj = null;
            try
            {
                stringReader = new StringReader(xmlMessage);
                xmlSerializer = new XmlSerializer(objectType);
                xmlTextReader = new XmlTextReader(stringReader);
                obj = xmlSerializer.Deserialize(xmlTextReader);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (xmlTextReader != null)
                {
                    xmlTextReader.Close();
                }
                if (stringReader != null)
                {
                    stringReader.Close();
                }
            }
            return obj;
        }
    }
}
