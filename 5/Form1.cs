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
    }
}
