namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class Day18
    {
        private delegate long ExpressionEvaluator(ref string expression);

        private enum Operation
        {
            Add,
            Multiply
        }

        public static void Solve()
        {
            var expressions = File.ReadAllLines("Day18.data").Select(s => s.Replace(" ", "")).ToArray();
            Console.WriteLine($"(1) Sum of all expressions (no precedence): {expressions.Select(e => EvaluateExpressionWithoutPrecedence(ref e)).Sum()}");
            Console.WriteLine($"(2) Sum of all expressions (with precedence): {expressions.Select(e => EvaluateExpression(ref e)).Sum()}");
        }

        private static long EvaluateExpressionWithoutPrecedence(ref string expression)
        {
            var pos = 0;
            var result = GetNextValue(ref expression, ref pos, EvaluateExpressionWithoutPrecedence);
            while (TryGetNextOperation(ref expression, ref pos, out var operation))
            {
                result = operation switch
                {
                    Operation.Add => result + GetNextValue(ref expression, ref pos, EvaluateExpressionWithoutPrecedence),
                    _ => result * GetNextValue(ref expression, ref pos, EvaluateExpressionWithoutPrecedence)
                };
            }

            return result;
        }

        private static long EvaluateExpression(ref string expression)
        {
            var pos = 0;
            var currentValue = GetNextValue(ref expression, ref pos, EvaluateExpression);
            var multipliers = new List<long>();
            while (TryGetNextOperation(ref expression, ref pos, out var operation))
            {
                if (operation == Operation.Add)
                {
                    currentValue += GetNextValue(ref expression, ref pos, EvaluateExpression);
                }
                else
                {
                    multipliers.Add(currentValue);
                    currentValue = GetNextValue(ref expression, ref pos, EvaluateExpression);
                }
            }

            // last value/sum must also be multiplied
            return multipliers.Aggregate(1L, (a, b) => a * b) * currentValue;
        }

        // gets the next value with can be either a number or an expression in brackets
        // and moves pos behind the value
        private static long GetNextValue(ref string expression, ref int pos, ExpressionEvaluator expressionEvaluator)
        {
            if (expression[pos] == '(')
            {
                var start = pos;
                var brackets = 0;
                pos += 1;
                while (pos < expression.Length)
                {
                    if (expression[pos] == '(')
                    {
                        brackets += 1;
                    }
                    else if (expression[pos] == ')')
                    {
                        if (brackets == 0)
                        {
                            var subExpression = expression.Substring(start + 1, pos - start - 1);
                            return expressionEvaluator(ref subExpression);
                        }
                        else
                        {
                            brackets -= 1;
                        }
                    }

                    pos += 1;
                }
            }
            else
            {
                return long.Parse(expression.Substring(pos++, 1)); // no numbers with more than one digit
            }

            return 0;
        }

        private static bool TryGetNextOperation(ref string expression, ref int pos, out Operation operation)
        {
            operation = default;
            while (pos < expression.Length)
            {
                switch (expression[pos])
                {
                    case '+':
                        pos += 1;
                        operation = Operation.Add;
                        return true;
                    case '*':
                        pos += 1;
                        operation = Operation.Multiply;
                        return true;
                    default:
                        pos += 1;
                        break;
                }
            }

            return false;
        }
    }
}
