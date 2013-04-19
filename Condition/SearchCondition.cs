using System;

namespace WebApp4.Condition
{
    public class SearchCondition
    {
        public string PropertyName { get; set; }

        public SearchOperationEnum Operation { get; set; }

        public Object PropertyValue { get; set; }
    }
}