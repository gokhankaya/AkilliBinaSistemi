using System;

namespace AdleGraph.Interfaces
{
    public interface IEdge
    {
        Func<double> FunctionOfEdge { get; set; }
        bool IsDirected { get; set; }
        string Name { get; }
        INode Node1 { get; set; }
        INode Node2 { get; set; }
        bool ShowWeight { get; set; }
        double Weight { get; set; }
        TimeSpan getTimeSpan();
    }
}