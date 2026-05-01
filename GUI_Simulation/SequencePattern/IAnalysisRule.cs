using AdleGraph.Interfaces;
using SequentialPattern;
using System.Collections.Generic;

namespace GUI_Simulation.SequencePattern
{
    public interface IAnalysisRule
    {
        string name { get; }

        void setParams(params object[] param);

        List<SequenceBarDTO> Analize(Sequence<INode> param, List<SequenceBarDTO> data);

        List<AnalizeResult> probabilityDistributionOfNodesInTheNextStep { get; }

        List<AnalizeResult> lastProbabilityDistributionOfNodes { get; }

        List<SequenceBarDTO> currentSequenceAnalysisResultOfSequenceBars { get; }

        int order { get; }
    }
}
