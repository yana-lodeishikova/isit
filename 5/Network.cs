using System;
using MathNet.Numerics.LinearAlgebra;

namespace NeuralNetwork
{
    public class Network
    {
        private readonly int InputNodes;
        private readonly int HiddenNodes;
        private readonly int OutputNodes;
        private readonly double LearningRate;
        private Matrix<double> WeightsInputHidden;
        private Matrix<double> WeightsHiddenOutput;

        private readonly Func<double, double> ActivationFunction = x => 1 / (1 + Math.Pow(Math.E, -x));

        public Network(int inputNodes, int hiddenNodes, int outputNodes, double learningRate)
        {
            InputNodes = inputNodes;
            HiddenNodes = hiddenNodes;
            OutputNodes = outputNodes;
            LearningRate = learningRate;

            var random = new Random();
            WeightsInputHidden = Matrix<double>.Build.Dense(HiddenNodes, InputNodes, (x, y) => random.NextDouble()) - 0.5;
            WeightsHiddenOutput = Matrix<double>.Build.Dense(OutputNodes, HiddenNodes, (x, y) => random.NextDouble()) - 0.5;
        }

        public void Train(double[] networkInputs, double[] networkOutputTargets)
        {
            var inputLayerOutputs = Matrix<double>.Build.DenseOfColumnArrays(networkInputs);

            var hiddenLayerInputs = WeightsInputHidden * inputLayerOutputs;
            var hiddenLayerOutputs = hiddenLayerInputs.Map(ActivationFunction);

            var outputLayerInputs = WeightsHiddenOutput * hiddenLayerOutputs;
            var outputLayerOutputs = outputLayerInputs.Map(ActivationFunction);
            var outputLayerOutputTargets = Matrix<double>.Build.DenseOfColumnArrays(networkOutputTargets);

            var outputLayerErrors = outputLayerOutputTargets - outputLayerOutputs;
            var hiddenLayerErrors = WeightsHiddenOutput.Transpose() * outputLayerErrors;

            WeightsHiddenOutput += LearningRate
                * outputLayerErrors.PointwiseMultiply(outputLayerOutputs).PointwiseMultiply(1 - outputLayerOutputs)
                * hiddenLayerOutputs.Transpose();
            WeightsInputHidden += LearningRate
                * hiddenLayerErrors.PointwiseMultiply(hiddenLayerOutputs).PointwiseMultiply(1 - hiddenLayerOutputs)
                * inputLayerOutputs.Transpose();
        }

        public double[] Query(double[] networkInputs)
        {
            var inputLayerOutputs = Matrix<double>.Build.DenseOfColumnArrays(networkInputs);

            var hiddenLayerInputs = WeightsInputHidden * inputLayerOutputs;
            var hiddenLayerOutputs = hiddenLayerInputs.Map(ActivationFunction);

            var outputLayerInputs = WeightsHiddenOutput * hiddenLayerOutputs;
            var outputLayerOutputs = outputLayerInputs.Map(ActivationFunction);

            return outputLayerOutputs.Column(0).ToArray();
        }
    }
}
