using System;
using MathNet.Numerics.LinearAlgebra;

namespace NeuralNetwork
{
    // Нейронная сеть
    public class Network
    {
        // Количество нейронов во входном, скрытом и выходном слоях сети
        private readonly int InputNodes;
        private readonly int HiddenNodes;
        private readonly int OutputNodes;
        // Коэффициент обучения
        private readonly double LearningRate;
        // Весовые коэффициенты связей между нейронами во входном и скрытом слое, скрытом и выходном
        private Matrix<double> WeightsInputHidden;
        private Matrix<double> WeightsHiddenOutput;

        // Функция активации нейрона - сигмоида
        private readonly Func<double, double> ActivationFunction = x => 1 / (1 + Math.Pow(Math.E, -x));

        public Network(int inputNodes, int hiddenNodes, int outputNodes, double learningRate)
        {
            InputNodes = inputNodes;
            HiddenNodes = hiddenNodes;
            OutputNodes = outputNodes;
            LearningRate = learningRate;

            // Инициализация весов случайными значениями в диапазоне [-0,5; 0,5)
            var random = new Random();
            WeightsInputHidden = Matrix<double>.Build.Dense(HiddenNodes, InputNodes, (x, y) => random.NextDouble()) - 0.5;
            WeightsHiddenOutput = Matrix<double>.Build.Dense(OutputNodes, HiddenNodes, (x, y) => random.NextDouble()) - 0.5;
        }

        // Обучение нейронной сети на одном примере эталонных выходных значений
        public void Train(double[] networkInputs, double[] networkOutputTargets)
        {
            // Расчет фактических выходных значений сети, такой же как в методе Query
            var inputLayerOutputs = Matrix<double>.Build.DenseOfColumnArrays(networkInputs);

            var hiddenLayerInputs = WeightsInputHidden * inputLayerOutputs;
            var hiddenLayerOutputs = hiddenLayerInputs.Map(ActivationFunction);

            var outputLayerInputs = WeightsHiddenOutput * hiddenLayerOutputs;
            var outputLayerOutputs = outputLayerInputs.Map(ActivationFunction);
            var outputLayerOutputTargets = Matrix<double>.Build.DenseOfColumnArrays(networkOutputTargets);

            // Ошибки выходного слоя - разница между целевым значением и действительным
            var outputLayerErrors = outputLayerOutputTargets - outputLayerOutputs;
            // Обратное распространение ошибки
            // Ошибки скрытого слоя - ошибки выходного слоя, распределенные пропорционально весам связей между соответствующими нейронами
            var hiddenLayerErrors = WeightsHiddenOutput.Transpose() * outputLayerErrors;

            // Обновление весов связей между нейронами скрытого и выходного слоя - минимизация ошибки методом градиентного спуска
            WeightsHiddenOutput += LearningRate
                * outputLayerErrors.PointwiseMultiply(outputLayerOutputs).PointwiseMultiply(1 - outputLayerOutputs)
                * hiddenLayerOutputs.Transpose();
            WeightsInputHidden += LearningRate
                * hiddenLayerErrors.PointwiseMultiply(hiddenLayerOutputs).PointwiseMultiply(1 - hiddenLayerOutputs)
                * inputLayerOutputs.Transpose();
        }

        // Опрос нейронной сети по заданным входным значениям
        public double[] Query(double[] networkInputs)
        {
            // Преобразование массива входных значений в матрицу из одного столбца
            var inputLayerOutputs = Matrix<double>.Build.DenseOfColumnArrays(networkInputs);

            // Входные сигналы скрытых нейронов определяются выходными сигналами входных нейронов, домноженными на веса связей
            var hiddenLayerInputs = WeightsInputHidden * inputLayerOutputs;
            // Выходные сигналы нейронов - это входные сигналы, преобразованные через функцию активации
            var hiddenLayerOutputs = hiddenLayerInputs.Map(ActivationFunction);

            // То же самое для выходного слоя
            var outputLayerInputs = WeightsHiddenOutput * hiddenLayerOutputs;
            var outputLayerOutputs = outputLayerInputs.Map(ActivationFunction);

            // Транспонирование столбца исходящих сигналов нейронов в массив выходных значений сети
            return outputLayerOutputs.Column(0).ToArray();
        }
    }
}
