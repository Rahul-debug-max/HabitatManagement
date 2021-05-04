using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

namespace HabitatManagement.Business
{
    public sealed class AttributeHelper
    {
        public static TAttr GetClassMemberAttribute<TAttr>(Type classType, string memberName) where TAttr : Attribute
        {
            TAttr attr = null;
            try
            {
                MemberInfo[] mi = classType.GetMember(memberName);
                if (mi != null && mi.Length > 0)
                    attr = Attribute.GetCustomAttribute(mi[0], typeof(TAttr)) as TAttr;
            }
            catch { }
            return attr;

        }

        public static int GetMaxFieldLength(Type classType, string memberName)
        {
            MaxFieldLengthAttribute a = AttributeHelper.GetClassMemberAttribute<MaxFieldLengthAttribute>(classType, memberName);
            if (a != null)
                return a.MaxLength;
            return 0;
        }

        public static bool GetUppercase(Type classType, string memberName)
        {
            UppercaseAttribute a = AttributeHelper.GetClassMemberAttribute<UppercaseAttribute>(classType, memberName);
            return a != null;                
        }
    }
}