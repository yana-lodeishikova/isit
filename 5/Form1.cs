using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace NeuralNetwork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ResetImage();
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
