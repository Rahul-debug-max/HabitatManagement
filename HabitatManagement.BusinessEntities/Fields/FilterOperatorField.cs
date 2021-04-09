using System;
using System.Collections.Generic;
using System.Text;

namespace HabitatManagement.BusinessEntities
{
    [Flags]
    public enum FilterOperatorField : byte
    {
        None = 0,
        Equal = 1,
        NotEqual = 2,
        GreaterThanOrEqual = 4,
        LessThanOrEqual = 8,  
        GreaterThan = 16,
        LessThan = 32,
        Contains = 64,
        StartsWith = 128,
        AllForNumeric = Equal | NotEqual | GreaterThanOrEqual | LessThanOrEqual | LessThan | GreaterThan,
        AllForText = Equal | NotEqual | Contains | StartsWith
    }

    public static class FilterOperatorHelper
    {
        public static bool HasOperator(FilterOperatorField allOperators, FilterOperatorField singleOperator)
        {
            return ((allOperators & singleOperator) == singleOperator);
        }
    }
}