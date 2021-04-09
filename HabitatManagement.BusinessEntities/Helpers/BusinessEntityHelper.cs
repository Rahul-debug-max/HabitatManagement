using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace HabitatManagement.BusinessEntities
{
    public sealed class BusinessEntityHelper
    {
        /// <summary>
        /// Copy all properties from one business entity to another. CustomData from the source entity will be copied to new properties on the target entity (if they exist)
        /// </summary>
        /// <param name="fromObject">Entity to copy properties from</param>
        /// <param name="toObject">Entity to copy properties to</param>
        public static void ConvertBEToBEForUI<FromType, ToType>(FromType fromObject, ToType toObject)
            where FromType : BusinessEntity
            where ToType : BusinessEntity
        {
            // Translate all properties from the data-access type BE (with attached custom data) into an entity that has explicit fields for the custom data
            foreach (PropertyInfo toProperty in toObject.GetType().GetProperties())
            {
                PropertyInfo fromProperty = fromObject.GetType().GetProperty(toProperty.Name);

                // If the property didn't exist, search the custom data
                if (fromProperty == null)
                {
                    if (fromObject.HasCustomData(toProperty.Name))
                        toProperty.SetValue(toObject, fromObject.GetCustomData(toProperty.Name), null);
                }
                else
                {
                    if (toProperty.CanWrite)
                        toProperty.SetValue(toObject, fromProperty.GetValue(fromObject, null), null);
                }
            }
        }

        /// <summary>
        /// Converts UI style entities to the base business entity they are derived from. Note that no custom data will be added to the destination business entity
        /// </summary>
        /// <typeparam name="FromType">The UI business entity type to convert from</typeparam>
        /// <typeparam name="ToType">The base business entity type to convert to</typeparam>
        /// <param name="fromObject">The UI business entity to convert from</param>
        /// <returns></returns>
        public static ToType ConvertBEForUIToBE<FromType, ToType>(FromType fromObject)
            where FromType : BusinessEntity
            where ToType : BusinessEntity, new()
        {
            ToType toObject = new ToType();
            // Translate all properties from the UI-style BE into the corresponding base BE (note, no custom data can be added)
            foreach (PropertyInfo toProperty in toObject.GetType().GetProperties())
            {
                PropertyInfo fromProperty = fromObject.GetType().GetProperty(toProperty.Name);
                if (fromProperty != null)
                    toProperty.SetValue(toObject, fromProperty.GetValue(fromObject, null), null);
            }

            return toObject;
        }

        /// <summary>
        /// Converts a list of dataaccess-type business objects to those more suitable for biding to UI representations such as grids
        /// </summary>
        /// <typeparam name="FromType">Type to convert from</typeparam>
        /// <typeparam name="ToType">Type to convert to</typeparam>
        /// <param name="fromList">List of dataaccess type business entity objects</param>
        /// <returns>List of UI type business objects</returns>
        public static IList<ToType> ConvertBEListToBEForUIList<FromType, ToType>(IList<FromType> fromList)
            where FromType : BusinessEntity
            where ToType : BusinessEntity, new()
        {

            List<ToType> toList = null;

            // What we are doing here is converting the standard business entity into entities that define all the Properties required for the grid
            if (fromList != null)
            {
                toList = new List<ToType>(fromList.Count);

                foreach (FromType fromEnt in fromList)
                {
                    ToType toEnt = new ToType();
                    ConvertBEToBEForUI(fromEnt, toEnt);
                    toList.Add(toEnt);
                }
            }

            return toList;
        }


        /// <summary>
        /// Compares two generic property values according to the filter operator field.
        /// Is used to search business entity collections by reflection
        /// </summary>
        /// <typeparam name="T">Type of property (must implement IComparable)</typeparam>
        /// <param name="filterValue">Filtering value</param>
        /// <param name="propertyValue">Value of a property (of a given business entity)</param>
        /// <param name="filterOperator">Filter criterion</param>
        /// <returns></returns>
        public static bool PropertyValuesMatch<T>(T filterValue, T propertyValue, FilterOperatorField filterOperator) where T : IComparable
        {
            bool result = false;

            // If either arg is null then the only available options will be equal or not equal
            if (propertyValue == null || filterValue == null)
            {
                switch (filterOperator)
                {
                    case FilterOperatorField.NotEqual:
                        result = (propertyValue == null && filterValue != null) || (propertyValue != null && filterValue == null);
                        break;
                    case FilterOperatorField.Equal:
                        result = propertyValue == null && filterValue == null;
                        break;
                }
                return result;
            }

            // Normal, non-null arguments
            switch (filterOperator)
            {
                case FilterOperatorField.NotEqual:
                    if (typeof(T).IsAssignableFrom(typeof(string)))
                    {
                        string filterString = filterValue as string;
                        string propertyString = propertyValue as string;
                        result = propertyString.ToLower() != filterString.ToLower();
                    }
                    else
                        result = !propertyValue.Equals(filterValue);
                    break;
                case FilterOperatorField.Equal:
                    if (typeof(T).IsAssignableFrom(typeof(string)))
                    {
                        string filterString = filterValue as string;
                        string propertyString = propertyValue as string;
                        result = propertyString.ToLower() == filterString.ToLower();
                    }
                    else
                        result = propertyValue.Equals(filterValue);
                    break;
                case FilterOperatorField.GreaterThan:
                    result = propertyValue.CompareTo(filterValue) < 0;
                    break;
                case FilterOperatorField.GreaterThanOrEqual:
                    result = propertyValue.CompareTo(filterValue) <= 0;
                    break;
                case FilterOperatorField.LessThan:
                    result = propertyValue.CompareTo(filterValue) > 0;
                    break;
                case FilterOperatorField.LessThanOrEqual:
                    result = propertyValue.CompareTo(filterValue) > 0;
                    break;
                case FilterOperatorField.Contains:
                    if (typeof(T).IsAssignableFrom(typeof(string)))
                    {
                        string filterString = filterValue as string;
                        string propertyString = propertyValue as string;
                        result = propertyString.ToLower().Contains(filterString.ToLower());
                    }
                    break;
                case FilterOperatorField.StartsWith:
                    if (typeof(T).IsAssignableFrom(typeof(string)))
                    {
                        string filterString = filterValue as string;
                        string propertyString = propertyValue as string;
                        result = propertyString.ToLower().StartsWith(filterString.ToLower());
                    }
                    break;
            }

            return result;
        }

        public static ToType XMLDeserializeFromString<ToType>(string xml)
            where ToType : BusinessEntity
        {
            XmlSerializer ser = new XmlSerializer(typeof(ToType));
            return (ToType)ser.Deserialize(new StringReader(xml));
        }

        /// <summary>
        /// Copies only the selected properties from one object to another of the same type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fromObject"></param>
        /// <param name="toObject"></param>
        /// <param name="selectedProperties"></param>
        public static void CopySelectedProperties<T>(T fromObject, T toObject, IList<string> selectedProperties)
        {
            // Translate all properties from the data-access type BE (with attached custom data) into an entity that has explicit fields for the custom data            
            foreach (PropertyInfo toProperty in toObject.GetType().GetProperties())
            {
                // If no selected properties, assume all
                if (selectedProperties == null || selectedProperties.Count == 0 || selectedProperties.Contains(toProperty.Name))
                {
                    PropertyInfo fromProperty = fromObject.GetType().GetProperty(toProperty.Name);

                    // Copy to target object
                    if (fromProperty != null)                        
                        toProperty.SetValue(toObject, fromProperty.GetValue(fromObject, null), null);
                }
            }
        }

        public static void ReplaceNullProperties<T>(T o)
        {
            // Find any properties that contain null and replace them
            // Primarily used to ensure Plex interface calls succeed initially
            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                if (p.GetValue(o, null) == null && p.CanWrite)
                {
                    if (p.PropertyType == typeof(string))
                        p.SetValue(o, "", null);
                    else if (p.PropertyType == typeof(long?))
                        p.SetValue(o, default(long), null);
                    else if (p.PropertyType == typeof(int?))
                        p.SetValue(o, 0, null);
                    else if (p.PropertyType == typeof(decimal?))
                        p.SetValue(o, default(decimal), null);
                    else if (p.PropertyType == typeof(double?))
                        p.SetValue(o, default(double), null);
                    else if (p.PropertyType == typeof(char?))
                        p.SetValue(o, default(char), null);
                }
            }
        }

        public static void ConvertPropertiesToUppercase<T>(T o)
        {
            // Find any properties that are marked with the uppercase attribute
            // and ensure that they are uppercase
            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                if (p.PropertyType == typeof(string) && p.GetSetMethod() != null && AttributeHelper.GetUppercase(typeof(T), p.Name))
                {
                    string s = p.GetValue(o, null) as string; 
                    if (s != null)
                        p.SetValue(o, s.ToUpper(), null);
                }
            }
        }

        public static void HTMLDecodeAllStrings<T>(T o)
        {
            // Find any string properties and HTML decode them
            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                if (p.PropertyType == typeof(string) && p.GetSetMethod() != null)
                {
                    string s = p.GetValue(o, null) as string;
                    if (s != null )
                        p.SetValue(o, System.Web.HttpUtility.HtmlDecode(s), null);
                }
            }
        }
    }
}
