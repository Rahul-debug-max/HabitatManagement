using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace HabitatManagement.Business
{
    public class BusinessEntity : ICloneable
    {
        public BusinessEntity()
        {
            _customData = new Dictionary<string, object>();
        }

        /// <summary>
        /// Custom data is a tag that can be used to attach additional data to the business entity
        /// The XML serializer can't handle this IDictionary so by default it is tagged as XmlIgnoreAttribute 
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute]
        private IDictionary<string, object> _customData;
        [System.Xml.Serialization.XmlIgnoreAttribute]
        private IDictionary<string, object> CustomData
        {
            get { return this._customData; }
            set { this._customData = value; }
        }

        /// <summary>
        /// Gets the data associated with a custom key. Returns null if data not found.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetCustomData(string key)
        {
            return HasCustomData(key) ? CustomData[key] : null;
        }

        public bool HasCustomData(string key)
        {
            return (!string.IsNullOrEmpty(key) && CustomData.ContainsKey(key));
        }

        /// <summary>
        /// Sets the custom data if it exists, otherwise adds the custom data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void SetCustomData(string key, object data)
        {
            if (HasCustomData(key))
                CustomData[key] = data;
            else
                CustomData.Add(key, data);
        }

        public void RemoveAllCustomData()
        {
            CustomData.Clear();
        }

        public void RemoveCustomData(string key)
        {
            if (HasCustomData(key))
                CustomData.Remove(key);
        }

        /// <summary>
        /// The clone method does not do a full deep-copy operation. It ensures however that the custom data emptied.
        /// </summary>
        /// <returns></returns>
        public BusinessEntity CloneWithoutCustomData()
        {
            BusinessEntity clone = this.Clone() as BusinessEntity;

            if (clone != null)
                clone.RemoveAllCustomData();

            return clone;
        }

        /// <summary>
        /// The clone method does not do a full deep-copy operation; however it does ensure that the custom data is a new instance
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            BusinessEntity clone = this.MemberwiseClone() as BusinessEntity;

            clone.CustomData = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> pair in CustomData)
                clone.CustomData.Add(pair);

            return clone;
        }

        /// <summary>
        /// A method that can be overriden to check if two business entity objects are equal by key
        /// Often used to compare two business entities for equality when comboboxes are loaded with business entities
        /// By default this function will work for any entities that have a single key named 'ID'
        /// </summary>
        /// <returns></returns>
        public virtual bool AreEqualByKey(BusinessEntity b)
        {
            bool equal = false;
            try
            {
                // For convenience this function attempts to match on key of 'ID'
                // This is true for many business entities so they will not have to explicitly override this method.
                PropertyInfo pinfo = this.GetType().GetProperty("ID");
                PropertyInfo pinfo2 = b.GetType().GetProperty("ID");
                if (pinfo != null && pinfo2 != null)
                {
                    object o1 = pinfo.GetValue(this, null);
                    object o2 = pinfo2.GetValue(b, null);

                    if (o1 != null && o2 != null)
                    {
                        int i1 = Functions.ToInt(o1);
                        int i2 = Functions.ToInt(o2);
                        //if (i1 != 0 && i1 == i2)
                        if (i1 == i2)
                            equal = true;
                    }
                }
            }
            catch
            {
                equal = false;
            }
            return equal;

        }

        public string XMLSerializeToString()
        {
            StringWriter sw = new StringWriter();
            XmlSerializer ser = new XmlSerializer(this.GetType());
            ser.Serialize(sw, this);
            return sw.ToString();
        }

        public XmlDocument XMLSerializeToDocument()
        {
            XmlDocument doc = new XmlDocument();
            MemoryStream stream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(this.GetType());
            ser.Serialize(stream, this);
            stream.Seek(0, SeekOrigin.Begin);
            doc.Load(stream);
            stream.Close();
            return doc;
        }

        /// <summary>
        /// Gets the data associated with a custom key. Returns default if data not found.
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <param name="key">Custom Filed Name</param>
        /// <returns></returns>
        public T GetCustomDataValue<T>(string key)
        {
            T returnValue = default(T);
            if (this.HasCustomData(key))
            {
                var data = this.GetCustomData(key);
                if(data != null)
                {
                    returnValue = (T)data;
                    if (returnValue == null)
                        returnValue = default(T);
                }
            }
            return returnValue;
        }

        // Return all Custom data for this instance
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public IDictionary<string, object> GetAllCustomData
        {
            get { return this.CustomData; }
            set { }
        }
    }
}