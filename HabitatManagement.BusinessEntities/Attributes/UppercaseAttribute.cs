using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HabitatManagement.Business
{
    /// <summary>
    /// An attribute to specify that the property must recieve uppercase input
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UppercaseAttribute : Attribute
    {                
    }
}
