using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CollectiveDecision
{
    public partial class Form1 : Form
    {
        private static readonly int ControlsMarginPx = 8;
        private static readonly (int Width, int Height) ControlsSizePx = (220, 24);
        private static readonly string[] Candidates =
        {
            "JBL Wave 200TWS",
            "Samsung Buds 2",
            "SteelSeries Arctis 1",
            "A4Tech HS-200",
        };

        private Stack<(Label Label, ComboBox[] ComboBoxes)> VoteOrderControls;

        public Form1()
        {
            InitializeComponent();
            VoteOrderControls = new Stack<(Label Label, ComboBox[] ComboBoxes)>();
        }
        
        // При нажатии на кнопку "Добавить" появляются элементы управления, позволяющие
        // очередному выборщику заполнить список предпочтений
        private void buttonAddVotes_Click(object sender, EventArgs e)
        {
            var verticalLocation = (ControlsSizePx.Height + ControlsMarginPx) * VoteOrderControls.Count;

            var label = CreateLabel(
                new Point(0, verticalLocation),
                $"Список предпочтений №{VoteOrderControls.Count + 1}"
            );

            var comboBoxes = Candidates
                .Select(
                    (candidate, i) => CreateComboBox(
                        new Point((ControlsMarginPx + ControlsSizePx.Width) * (i + 1), verticalLocation),
                        candidate
                    )
                )
                .ToArray();

            VoteOrderControls.Push((label, comboBoxes));
            panel.SuspendLayout();
            panel.Controls.Add(label);
            panel.Controls.AddRange(comboBoxes);
            panel.ResumeLayout();
        }

        private Label CreateLabel(Point location, string text)
        {
            var label = new Label();

            label.Location = location;
            label.Size = new Size(ControlsSizePx.Width, ControlsSizePx.Height);
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleLeft;

            return label;
        }

        private ComboBox CreateComboBox(Point location, string candidate)
        {
            var comboBox = new ComboBox();
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.Location = location;
            comboBox.Size = new Size(ControlsSizePx.Width, ControlsSizePx.Height);
            comboBox.Items.AddRange(Candidates);
            comboBox.SelectedItem = candidate;
            return comboBox;
        }

        // При нажатии кнопку "Удалить" уничтожаются элементы управления для последнего выборщика
        private void buttonRemoveVotes_Click(object sender, EventArgs e)
        {
            if (VoteOrderControls.Count > 0)
            {
                var lastVoteOrderControls = VoteOrderControls.Pop();
                panel.Controls.Remove(lastVoteOrderControls.Label);
                foreach (var comboBox in lastVoteOrderControls.ComboBoxes)
                {
                    panel.Controls.Remove(comboBox);
                }
            }
        }
    }
}
