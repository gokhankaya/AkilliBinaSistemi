using AdleGraph.Interfaces;
using GUI_Simulation.AnomalyExploration;
using SequentialPattern;
using System;

namespace GUI_Simulation.SequencePattern
{
    public class SequenceBarDTO
    {
        public int Order { get; set; }
        public string Person { get; set; }
        public Scenario Scenario { get; set; }
        public Sequence<INode> Sequence { get; set; }
        public string SimilarityRatio { get; set; }

        public double normalizedValue { get; set; }
        public int LCS { get; set; }

        public Guid ID { get; set; }

        public bool TrainingData { get; set; }
    }
}