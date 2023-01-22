using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace NeuralNetwork
{
    public partial class Form1 : Form
    {
        // Количество входных нейронов в одной стороне квадратной сетки входного слоя
        private const int BlocksOneSide = 10;

        private Network Network;
        private readonly Dictionary<string, IEnumerable<Image>> SymbolExampleImages = new Dictionary<string, IEnumerable<Image>>();

        public Form1()
        {
            InitializeComponent();
            ResetImage();
        }

        // Нажатие на кнопку добавления тренировочных изображений для символа - открывает окно выбора файлов-изображений
        private void button1_Click(object sender, EventArgs e) => AddExamples("1");
        private void button2_Click(object sender, EventArgs e) => AddExamples("2");
        private void button3_Click(object sender, EventArgs e) => AddExamples("3");
        private void button4_Click(object sender, EventArgs e) => AddExamples("4");
        private void button5_Click(object sender, EventArgs e) => AddExamples("5");
        private void button6_Click(object sender, EventArgs e) => AddExamples("6");
        private void button7_Click(object sender, EventArgs e) => AddExamples("7");
        private void button8_Click(object sender, EventArgs e) => AddExamples("8");
        private void button9_Click(object sender, EventArgs e) => AddExamples("9");
        private void button10_Click(object sender, EventArgs e) => AddExamples("10");
        private void AddExamples(string symbol)
        {
            var dialog = new OpenFileDialog {Multiselect = true, Filter = "Bitmap Image|*.bmp"};
            if (dialog.ShowDialog() != DialogResult.OK) return;

            var bitmapFilePaths = dialog.FileNames;
            var bitmaps = bitmapFilePaths.Select(Image.FromFile).ToArray();

            if (SymbolExampleImages.ContainsKey(symbol)) SymbolExampleImages.Remove(symbol);
            SymbolExampleImages.Add(symbol, bitmaps);

            labelImages.Text = $"Изображений: {SymbolExampleImages.SelectMany(example => example.Value).Count()}";
        }

        // Рисование символа мышкой
        private void pictureBox_Draw(object sender, MouseEventArgs mouseEvent)
        {
            if (mouseEvent.Button != MouseButtons.Left) return;
            using (var image = Graphics.FromImage(pictureBox.Image))
            {
                image.DrawEllipse(new Pen(Color.Black, 3), mouseEvent.Location.X - 2, mouseEvent.Location.Y - 2, 3, 3);
                pictureBox.Invalidate();
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            ResetImage();
            labelResult.Text = null;
        }

        // Очистка поля рисования символа
        private void ResetImage()
        {
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (var image = Graphics.FromImage(pictureBox.Image))
            {
                image.Clear(Color.White);
                pictureBox.Invalidate();
            }
        }

        // Сохранение нарисованного символа в файл
        private void buttonSave_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog {OverwritePrompt = true, Filter = "Bitmap Image|*.bmp"};
            if (dialog.ShowDialog() != DialogResult.OK) return;

            var file = dialog.OpenFile();
            pictureBox.Image.Save(file, ImageFormat.Bmp);
            file.Close();
        }
        
        // Распознавание нарисованного символа нейросетью
        private void buttonTest_Click(object sender, EventArgs e)
        {
            var symbols = SymbolExampleImages.Keys.ToArray();
            var resultSymbolPercentages = Network.Query(ConvertImageToNetworkInputs(pictureBox.Image));
            var resultSymbolIndex = Array.IndexOf(resultSymbolPercentages, resultSymbolPercentages.Max());
            labelResult.Text = $"{symbols[resultSymbolIndex]} ({resultSymbolPercentages[resultSymbolIndex]:P})";
        }

        // Запуск обучения нейросети
        private void buttonTrain_Click(object sender, EventArgs e)
        {
            Network = new Network(BlocksOneSide * BlocksOneSide, 50, SymbolExampleImages.Count, 0.3);
            // 10 "эпох" повторения циклов обучения сети
            for (int i = 0; i < 10; i++)
            {
                // Перебор всех изображений-примеров для всех распознаваемых символов
                for (var j = 0; j < SymbolExampleImages.Count; j++)
                {
                    var symbol = SymbolExampleImages.Keys.ElementAt(j);
                    foreach (var image in SymbolExampleImages[symbol])
                    {
                        var inputs = ConvertImageToNetworkInputs(image);

                        // Заполнение целевых значений: желаемое значение корректного символа - 0.99, остальные - 0.01
                        var targetOutputs = new double[SymbolExampleImages.Count]
                            .Select(_ => 0.01)
                            .ToArray();
                        targetOutputs[j] = 0.99;

                        Network.Train(inputs, targetOutputs);
                    }
                }
            }
        }

        // Преобразование изображения (с большим разрешением в пикселях) в массив входных значений сети 10x10
        private static double[] ConvertImageToNetworkInputs(Image image)
        {
            var bitmap = new Bitmap(image);
            if (bitmap.Width % BlocksOneSide != 0 || bitmap.Height % BlocksOneSide != 0) throw new InvalidOperationException($"Изображение не раскладывается на сетку {BlocksOneSide}x{BlocksOneSide}");

            // Изображение делится на равные блоки, каждый из которых соответствует одному входному нейрону
            var inputBlocks = new double[BlocksOneSide, BlocksOneSide];
            for (var blockX = 0; blockX < BlocksOneSide; blockX++)
            {
                for (var blockY = 0; blockY < BlocksOneSide; blockY++)
                {
                    // Определение координат границ блока на изображении
                    int blockWidth = bitmap.Width / BlocksOneSide;
                    int blockHeight = bitmap.Height / BlocksOneSide;
                    int leftMargin = blockWidth * blockX;
                    int topMargin = blockHeight * blockY;

                    // Заполнение блока пикселями изображения
                    var blockPixels = new Color[blockWidth * blockHeight];
                    for (var x = 0; x < blockWidth; x++)
                    {
                        for (var y = 0; y < blockHeight; y++)
                        {
                            blockPixels[y * blockHeight + x] = bitmap.GetPixel(leftMargin + x, topMargin + y);
                        }
                    }

                    // Подсчет количества черных пикселей в блоке
                    var percentBlackPixelsInBlock =
                        (double)blockPixels.Count(pixel => pixel.GetBrightness() == 0)
                        / blockPixels.Length;

                    var value = percentBlackPixelsInBlock * 8;
                    if (value > 1) value = 1;

                    // Преобразование количества черных пикселей в результирующее входное значение нейрона
                    inputBlocks[blockX, blockY] = value * 0.99 + 0.01;
                }
            }

            // Преобразование двумерного массива блоков изображения в одномерный массив входных значений нейрона
            var inputs = new double[BlocksOneSide * BlocksOneSide];
            for (var blockX = 0; blockX < BlocksOneSide; blockX++)
            {
                for (var blockY = 0; blockY < BlocksOneSide; blockY++)
                {
                    inputs[blockY * BlocksOneSide + blockX] = inputBlocks[blockX, blockY];
                }
            }

            return inputs;
        }
    }
}
