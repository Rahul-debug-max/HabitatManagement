using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HabitatManagement.BusinessEntities
{
    /// <summary>
    /// An attribute to give properties the ability to define their default length
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxFieldLengthAttribute : Attribute
    {
        private int _maxLength = 0;

        public MaxFieldLengthAttribute(int maxLength)
        {
            _maxLength = maxLength;
        }

        public int MaxLength
        {
            get { return _maxLength; }
        }

        public override string ToString()
        {
            return _maxLength.ToString();
        }
    }
}
