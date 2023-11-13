

using System.ComponentModel;

namespace Forces.Application.Enums
{
    public enum MathOperation
    {
        [Description("> Greater Than")] GreaterThan,
        [Description("< Less Than")] LessThan,
        [Description("= Equals")] Equal,
        [Description(">= Greater Than Or Equal")] GreaterThanOrEqual,
        [Description("<= Less Than Or Equal")] LessThanOrEqual,
    }
}
