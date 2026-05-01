using Accord.Math;

namespace GUI_Simulation.AnomalyExploration
{
    public class InputWindow
    {
        public InputWindow(int numberOfSensors, int lenghtOfWindow)
        {
            states = new double[numberOfSensors][];
            for (int i = 0; i < numberOfSensors; i++)
            {
                states[i] = new double[lenghtOfWindow];
            }
        }

        public int order { get; set; }

        public double[][] states { get; set; }

        public double[][] convolvedStates { get; set; }

        public void ConvolveWindow()
        {
            convolvedStates = new double[states.Length][];
            for (int i = 0; i < states.Length; i++)
            {
                convolvedStates[i] = new double[states[i].Length];
                convolvedStates[i] = states[i].Convolve(new double[] { 1.0, 0.9, 0.8, 0.7, 0.6, 0.5, 0.4, 0.3, 0.2, 0.1, 0.0 }, true);
                //0.0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9 
            }

        }

        public override string ToString()
        {
            return order.ToString();
        }
    }
}
