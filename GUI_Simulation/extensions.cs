using Accord.MachineLearning.DecisionTrees;
using System;

namespace GUI_Simulation
{
    public static class extensions
    {
        public static void PrintPretty(this DecisionNode ob, string indent, bool last)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }

            string outPut = ob.Output != null ? $" output => {ob.Output.ToString()}" : "";
            string value = ob.IsRoot ? "Root" : $"{ob.Comparison} {ob.Value}";

            Console.WriteLine($"{value} {outPut}");

            for (int i = 0; i < ob.Branches.Count; i++)
                ob.Branches[i].PrintPretty(indent, i == ob.Branches.Count - 1);
        }

        public static string PrintPretty2(this DecisionNode ob, string indent, bool last)
        {
            string details = "";

            details += indent;
            if (last)
            {
                details += "\\-";
                indent += "  ";
            }
            else
            {
                details += "|-";
                indent += "|  ";
            }

            string outPut = ob.Output != null ? $" output => {ob.Output.ToString()}" : "";
            string value = ob.IsRoot ? "Root" : $"{ob.Comparison} {ob.Value}";

            details += $"{value} {outPut}\n";

            for (int i = 0; i < ob.Branches.Count; i++)
                details +=  ob.Branches[i].PrintPretty2(indent, i == ob.Branches.Count - 1);

            return details;
        }

        public static string ToStringValue(this double[] ob)
        {
            string value = "";
            for (int i = 0; i < ob.Length; i++)
            {
                value += $" {ob[i]},".TrimEnd(new char[] { ',' });
            }

            return value.TrimStart(new char[] { ' ' });
        }
    }
}
