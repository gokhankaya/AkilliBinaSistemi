using System;
using AdleGraph.Interfaces;

namespace AdleGraph
{

    /// <summary>
    /// Garf için her bir node temsil eder. Nodun kenarlarını bulmak için nodeun adından edgeliste arama yapılabilir.
    /// </summary>
    public class Node : INode
    {
        #region Ctors
        public Node()
        {

        }

        public Node(string name) : this()
        {
            Name = name;
        }

        #endregion Ctors

        #region Properties
        public string Name { get; set; } = null;

        public int EdgeCount { get; set; }

        public object Tag { get; set; }

        #endregion Properties

        #region Methods
        public override string ToString()
        {
            return Name;
        }

        #endregion  Methods
    }
}
