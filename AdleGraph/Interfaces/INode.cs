using System;

namespace AdleGraph.Interfaces
{
    public interface INode
    {
        int EdgeCount { get; set; }
        string Name { get; set; }
        object Tag { get; set; }
    }
}