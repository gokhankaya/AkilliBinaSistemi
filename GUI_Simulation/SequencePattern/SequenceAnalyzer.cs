using AdleGraph.Interfaces;
using SequentialPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Simulation.SequencePattern
{
    public class SequenceAnalyzer
    {
        public SequenceAnalyzer()
        {
            Rules = new List<IAnalysisRule>();
            currentSequenceAnalysisResult = new List<SequenceBarDTO>();
            probabilityDistributionOfNodesInTheNextStep = new List<AnalizeResult>();
        }

        public List<IAnalysisRule> Rules { get; set; }

        public List<SequenceBarDTO> Data { get; set; }

        public List<SequenceBarDTO> currentSequenceAnalysisResult { get; private set; }

        public List<AnalizeResult> probabilityDistributionOfNodesInTheNextStep { get; private set; }
        public List<AnalizeResult> lastProbabilityDistributionOfNodes { get; private set; }

        public string ResultsFromWhichRule { get; private set; }


        public bool Analyze(Sequence<INode> param)
        {
            if (Data == null || Data.Count <= 0)
                return false;

            var orderedRules = Rules.OrderBy(x => x.order).ToList();
            currentSequenceAnalysisResult = new List<SequenceBarDTO>();

            foreach (var rule in orderedRules)
            {
                ResultsFromWhichRule = rule.name;

                currentSequenceAnalysisResult = rule.Analize(param, Data);
                probabilityDistributionOfNodesInTheNextStep = rule.probabilityDistributionOfNodesInTheNextStep;
                lastProbabilityDistributionOfNodes = rule.lastProbabilityDistributionOfNodes;

                if (currentSequenceAnalysisResult.Count > 0)
                    break;
            }

            return true;
        }

        public double GetSupportRatio()
        {
            var result = (double)((100 * currentSequenceAnalysisResult.Count) / Data.Count);
            return result;
        }
    }
}


