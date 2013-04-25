namespace WebApp4.Condition
{
    public enum SearchOperationEnum
    {
        //包含
        Contains,
        //不等于
        NotEqual,
        //等于
        Equal,
        //大于
        GreaterThan,
        //小于
        LesserThan,
        //大于等于
        GreaterThanOrEqual,
        //小于等于
        LesserThanOrEqual
    }
    public enum ConstraintType
    {
        And,
        Or
    }
}