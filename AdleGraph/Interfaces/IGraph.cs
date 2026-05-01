using System;
using System.Collections.Generic;
using AdleGraph.Interfaces;

namespace AdleGraph.Interfaces
{
    public interface IGraph
    {
        List<IEdge> EdgeList { get; set; }
        List<INode> NodeList { get; set; }

        string Name { get; set; }
        bool ShowEdgesWeights { get; set; }
        IEdge AddEdge(string NodeNameFrom, string NodeNameTo, Func<double> function = null, bool isDirected = false);
        IEdge AddEdge(INode nodeFrom, INode nodeTo = null, Func<double> function = null, bool isdirected = false);
        IEdge AddEdge(string NodeNameFrom, string NodeNameTo, double weight = default(double), bool isDirected = false);
        IEdge AddEdge(INode nodeFrom, INode nodeTo = null, double weight = default(double), bool isdirected = false);
        INode AddNode(string name);
        void AddNode(INode node);
        double[,] GetMatrixOfGraph();
        string ToMatrisString();
        INode GetNodeWithName(string name, bool addNodeIfNotExist = false);
        INode MoveNext(INode node, bool useEgdeWeights = false);
        bool NodeExits(string nodeName);
        bool NodeExits(INode node);
        List<List<INode>> Run(INode StartNode, INode EndNode, bool useEgdeWeights = false, int maxIteration = -1, bool allowLoops = false, int maxTryCountForLoops = 10);
        bool UpdateEdge(string edgeName, IEdge newEdge);
    }
}