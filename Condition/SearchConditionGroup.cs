using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp4.Condition
{
    public class SearchConditionGroup
    {
        public List<SearchCondition> ConditionList { get; set; }

        public ConstraintType ConstraintType { get; set; }
    }
}
