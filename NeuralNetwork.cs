using System.Numerics;

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
            OutputCount = outputCount;  //skręt & prędkość
            Weights = new double[InputCount * OutputCount];    // - skręca w lewo, + w prawo, 0 prosto
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

            for (int k = 0; k < OutputCount; k++) //OutputCount=2
            {
                double sum = 0;
                for (int i = 0; i < InputCount; i++) //InputCount =4
                {
                    int weightIndex = i + (k * InputCount);
                    sum += inputs[i] * Weights[weightIndex]; // obliczmy obecne położenie przez wagi, aby ustalić skręt lub prędkość
                }
                outputs[k] = Math.Tanh(sum); //przekształcamy na wartość  od -1 do 1
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

        public static NeuralNetwork Crossover(NeuralNetwork parent1, NeuralNetwork parent2)
        {
            NeuralNetwork child = new NeuralNetwork(parent1.InputCount, parent2.OutputCount);
            Random rand = new Random();

            for (int i = 0; i < child.Weights.Length; i++)
            {
                // 50% szans na gen od rodzica 1, 50% od rodzica 2
                child.Weights[i] = rand.NextDouble() < 0.5 ? parent1.Weights[i] : parent2.Weights[i];
            }
            return child;
        }
    }
}