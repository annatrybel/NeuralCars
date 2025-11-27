using System;

namespace NeuralCars
{
    public class NeuralNetwork
    {
        public double[] Weights;
        private int InputCount;
        private int OutputCount;
        private Random _random = new Random();

        public NeuralNetwork(int inputCount, int outputCount)
        {
            InputCount = inputCount;
            OutputCount = outputCount;
            Weights = new double[InputCount * OutputCount];
            InitializeWeights();
        }

        public NeuralNetwork(NeuralNetwork other)
        {
            InputCount = other.InputCount;
            OutputCount = other.OutputCount;
            Weights = new double[other.Weights.Length];
            Array.Copy(other.Weights, Weights, other.Weights.Length);
        }

        private void InitializeWeights()
        {
            for (int i = 0; i < Weights.Length; i++)
                Weights[i] = _random.NextDouble() * 2 - 1; 
        }

        public double[] FeedForward(double[] inputs)
        {
            double[] outputs = new double[OutputCount];

            for (int o = 0; o < OutputCount; o++)
            {
                double sum = 0;
                for (int i = 0; i < InputCount; i++)
                {
                    int weightIndex = i + (o * InputCount);
                    sum += inputs[i] * Weights[weightIndex];
                }
                outputs[o] = Math.Tanh(sum);
            }

            return outputs;
        }

        public void Mutate(double rate)
        {
            for (int i = 0; i < Weights.Length; i++)
            {
                if (_random.NextDouble() < rate)
                {
                    double mutation = (_random.NextDouble() * 2 - 1) * 0.5;
                    Weights[i] += mutation;

                    if (Weights[i] > 1) Weights[i] = 1;
                    if (Weights[i] < -1) Weights[i] = -1;
                }
            }
        }
    }
}