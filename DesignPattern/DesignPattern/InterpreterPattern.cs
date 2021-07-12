using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 解释器模式
    /// Given a language, define a representation for its grammar along with an interpreter that use the representation to interpret sentences in the language.
    /// 给定一种语言，定义它的语法的一种表示，并定义一个解释器，并且使用解释器来解释语言中的句子。
    ///
    /// 有四个角色：
    /// - 抽象解释器 AbstractExpression
    ///   - 具体解释任务由各个实现类完成，具体解释器分别由 TerminalExpression 和 NonterminalExpression 完成。
    /// - 终结表达式 TerminalExpression
    ///   - 实现与文法中的元素关联的解释操作，通常一个解释器模式中，只有一个终结符表达式，但是又多个实例，对应不同的终结符。
    ///   - VarExpression 就是终结符表达式，表达式中的每个终结符都在栈中产生了一个 VarExpression 对象。
    /// - 非终结表达式 NonterminalExpression
    ///   - 文法中的每条规则，对应一个非终结表达式。
    ///   - AddExpression 对应加法规则，SubExpression 对应减法规则。
    ///   - 非终结表达式根据逻辑的复杂程度而增加，原则上每个文法规则都对应一个非终结表达式。
    /// - 表达式上下文角色 Context
    ///   - 就是给解释器传值，然后用解释器解释。
    /// 
    /// 优点：
    /// - 易扩展。添加规则只需要添加非终结表达式就好了。
    /// 缺点：
    /// - 语法规则复杂，就会由类膨胀。
    /// - 递归调用。调试、效率都很麻烦。
    ///
    /// 使用场景：
    /// - 重复发生的问题可以使用解释器模式。
    ///   - 如多个服务器，每天产生大量不同格式的日志，虽然格式不同，但是数据要素相同，也就是终结符表达式相同，但是非终极表达式需要定制。
    ///
    /// 解释器模式很少使用，尽量不要再重要的模块中使用解释器模式，否则维护很麻烦。可使用 shell、Groovy等脚本语言来代替解释器模式。
    /// 如果实在需要用，也没有必要从头开始写解释器，而是找一下开源解析工具包。
    /// 
    /// </summary>
    class InterpreterPattern
    {
        /// <summary>
        /// 公式运算解释器
        /// </summary>
        public void Interpreter()
        {
            var calculator = new Calculator("a+b-c");
            var result = calculator.Run(new Dictionary<string, int>
            {
                {"a", 10},
                {"b", 5},
                {"c", 8}
            });
            Console.WriteLine(result);
        }

        /// <summary>
        /// 调用解释器模式的通用客户端代码
        /// </summary>
        public void Interpreter2()
        {
            var ctx = new ExpressionContext();
            // 一个语法容器，用来容纳一个具体的表达式
            var stack = new Stack<AbstractExpression>();
            for (;;)
            {
                // 进行语法判断，并且产生递归调用
            }
            // 产生一个完成的语法树，由各个具体的语法分析进行解析
            var exp = stack.Pop();
            // 具体元素进入场景类
            exp.Interpret(ctx);
        }

    }

    #region 解释器模式实现四则运算

    abstract class Expression
    {
        /// <summary>
        /// 解析公式和数字，其中的 key 是公式中的参数，value 是具体数字
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract int Interpreter(Dictionary<string, int> dic);
    }

    /// <summary>
    /// 负责解析变量，如 a + b 中的 a 和 b
    /// </summary>
    class VarExpression : Expression
    {
        private readonly string _key;

        public VarExpression(string key)
        {
            _key = key;
        }

        public override int Interpreter(Dictionary<string, int> dic)
        {
            return dic[_key];
        }
    }

    /// <summary>
    /// 负责解释符号，如 +、-、*、/
    /// </summary>
    abstract class SymbolExpression : Expression
    {
        public Expression Left { get; }
        public Expression Right { get; }

        protected SymbolExpression(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }
    }

    /// <summary>
    /// 负责解析 +
    /// </summary>
    class AddExpression : SymbolExpression
    {
        public AddExpression(Expression left, Expression right) : base(left, right)
        {
        }

        public override int Interpreter(Dictionary<string, int> dic)
        {
            return Left.Interpreter(dic) + Right.Interpreter(dic);
        }
    }

    /// <summary>
    /// 负责解析 -
    /// </summary>
    class SubExpression : SymbolExpression
    {
        public SubExpression(Expression left, Expression right) : base(left, right)
        {
        }

        public override int Interpreter(Dictionary<string, int> dic)
        {
            return Left.Interpreter(dic) - Right.Interpreter(dic);
        }
    }

    class Calculator
    {
        private Expression _expression;

        public Calculator(string expStr)
        {
            var stack = new Stack<Expression>();
            var charArray = expStr.ToCharArray();
            
            for (int i = 0; i < charArray.Length; i++)
            {
                Expression left, right;
                switch (charArray[i])
                {
                    case '+': // 公式中的 +
                        left = stack.Pop();
                        right = new VarExpression(charArray[++i].ToString());
                        stack.Push(new AddExpression(left, right));
                        break;
                    case '-': // 公式中的 - 
                        left = stack.Pop();
                        right = new VarExpression(charArray[++i].ToString());
                        stack.Push(new SubExpression(left, right));
                        break;
                    default: // 公式中的变量
                        stack.Push(new VarExpression(charArray[i].ToString()));
                        break;
                }
            }

            _expression = stack.Pop();
        }

        /// <summary>
        /// 开始运算
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public int Run(Dictionary<string, int> dic)
        {
            return _expression.Interpreter(dic);
        }
    }

    #endregion

    #region 解释器模式通用源码

    /// <summary>
    /// 抽象解释器角色
    /// </summary>
    abstract class AbstractExpression
    {
        public abstract object Interpret(ExpressionContext ctx);
    }

    /// <summary>
    /// 终结符表达式角色
    /// </summary>
    class TerminalExpression : AbstractExpression
    {
        /// <summary>
        /// 终结符通常只有一个，但是有多个对象
        /// 终结符表达式主要是处理场景元素和数据的转换。
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override object Interpret(ExpressionContext ctx)
        {
            return null;
        }
    }

    /// <summary>
    /// 非终结符表达式角色
    /// 每个非终结符表达式都代表一个文法规则，并且每个文法规则都只关心自己周边文法规则的结果（注意是结果），
    /// 所有就产生了每个非终结符表达式调用自己周边的非终结表达式，然后最终、最小的规则就是终结符表达式。
    /// </summary>
    class NonterminalExpression : AbstractExpression
    {
        /// <summary>
        /// 每个非终结符表达式都会对其他表达式产生依赖
        /// </summary>
        /// <param name="expressions"></param>
        public NonterminalExpression(params AbstractExpression[] expressions)
        {
            
        }
        /// <summary>
        /// 进行文法处理
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override object Interpret(ExpressionContext ctx)
        {
            return null;
        }
    }

    /// <summary>
    /// 上下文角色
    /// </summary>
    class ExpressionContext
    {
        
    }

    #endregion
}
