using System;
using System.Collections.Generic;
using System.Text;

namespace LinnworksSales.Data.Models
{
    public class Filter
    {
        public Filter(Type type, string selectedValue)
        {
            Type = type;
            SelectedValue = selectedValue;
        }

        public Type Type { get; set; }
        public string SelectedValue { get; set; }
        public bool HasSelectedValue => !string.IsNullOrEmpty(SelectedValue);
    }
}
