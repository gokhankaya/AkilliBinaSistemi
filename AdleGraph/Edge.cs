using AdleGraph.Interfaces;
using System;

namespace AdleGraph
{
    public class Edge : IEdge
    {
        public INode Node1 { get; set; }
        public INode Node2 { get; set; }

        public bool IsDirected { get; set; } = false;
        public double Weight { get; set; } = -1;
        public bool ShowWeight { get; set; } = false;

        public Func<double> FunctionOfEdge { get; set; } = null;

        public string Name
        {
            get
            {
                string direction = IsDirected ? ">" : string.Empty;
                string weight = Weight == -1.0 ? "" : Weight.ToString();  //ShowWeight ? (FunctionOfEdge == null ? (Weight.ToString()) : FunctionOfEdge.ToString()) : string.Empty;

                return $"({Node1.Name}) --{weight}--{direction} ({Node2.Name})";
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public TimeSpan getTimeSpan()
        {
            return new TimeSpan();
            //if (Node1 == null || Node2 == null)
            //{
            //    return new TimeSpan(-1);
            //}

            //return Node2.Time - Node1.Time;
        }
    }
}
