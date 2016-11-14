using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AspMvcLibrary.Attributes
{
    class RangeNoIncludeAttribute : ValidationAttribute
    {
        public object Maximum { get; private set; }
        public object Minimum { get; private set; }
        public Type OperandType { get; private set; }


        public RangeNoIncludeAttribute(double minimum, double maximum)
        {
            Maximum = maximum;
            Minimum = minimum;
            OperandType = typeof(double);
        }

        public RangeNoIncludeAttribute(int minimum, int maximum)
        {
            Maximum = maximum;
            Minimum = minimum;
            OperandType = typeof(int);
        }


        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            if (this.OperandType == typeof(int))
            {
                var v = Convert.ToInt32( value);
                return v>(int)this.Minimum && v<=(int)this.Maximum;
            }
            else if (this.OperandType == typeof(double))
            {
                var v =  Convert.ToDouble(value);
                return v > (double)this.Minimum && v <= (double)this.Maximum;
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("{0}ををチェックしてください。", name, this.Minimum);
        }

    }
}
