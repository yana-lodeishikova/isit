namespace CollectiveDecision
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox = new System.Windows.Forms.TextBox();
            this.buttonAddVotes = new System.Windows.Forms.Button();
            this.buttonRemoveVotes = new System.Windows.Forms.Button();
            this.buttonBorda = new System.Windows.Forms.Button();
            this.buttonSimpson = new System.Windows.Forms.Button();
            this.buttonCopeland = new System.Windows.Forms.Button();
            this.buttonCondorcet = new System.Windows.Forms.Button();
            this.buttonPlurality = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox.Location = new System.Drawing.Point(18, 440);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.Size = new System.Drawing.Size(1086, 158);
            this.textBox.TabIndex = 9;
            // 
            // buttonAddVotes
            // 
            this.buttonAddVotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAddVotes.Location = new System.Drawing.Point(18, 405);
            this.buttonAddVotes.Name = "buttonAddVotes";
            this.buttonAddVotes.Size = new System.Drawing.Size(89, 29);
            this.buttonAddVotes.TabIndex = 1;
            this.buttonAddVotes.Text = "Добавить";
            this.buttonAddVotes.UseVisualStyleBackColor = true;
            this.buttonAddVotes.Click += new System.EventHandler(this.buttonAddVotes_Click);
            // 
            // buttonRemoveVotes
            // 
            this.buttonRemoveVotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRemoveVotes.Location = new System.Drawing.Point(112, 405);
            this.buttonRemoveVotes.Name = "buttonRemoveVotes";
            this.buttonRemoveVotes.Size = new System.Drawing.Size(89, 29);
            this.buttonRemoveVotes.TabIndex = 2;
            this.buttonRemoveVotes.Text = "Удалить";
            this.buttonRemoveVotes.UseVisualStyleBackColor = true;
            this.buttonRemoveVotes.Click += new System.EventHandler(this.buttonRemoveVotes_Click);
            // 
            // buttonBorda
            // 
            this.buttonBorda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBorda.Location = new System.Drawing.Point(1001, 405);
            this.buttonBorda.Name = "buttonBorda";
            this.buttonBorda.Size = new System.Drawing.Size(101, 29);
            this.buttonBorda.TabIndex = 3;
            this.buttonBorda.Text = "Борда";
            this.buttonBorda.UseVisualStyleBackColor = true;
            // 
            // buttonSimpson
            // 
            this.buttonSimpson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSimpson.Location = new System.Drawing.Point(894, 405);
            this.buttonSimpson.Name = "buttonSimpson";
            this.buttonSimpson.Size = new System.Drawing.Size(101, 29);
            this.buttonSimpson.TabIndex = 3;
            this.buttonSimpson.Text = "Симпсон";
            this.buttonSimpson.UseVisualStyleBackColor = true;
            // 
            // buttonCopeland
            // 
            this.buttonCopeland.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopeland.Location = new System.Drawing.Point(787, 405);
            this.buttonCopeland.Name = "buttonCopeland";
            this.buttonCopeland.Size = new System.Drawing.Size(101, 29);
            this.buttonCopeland.TabIndex = 3;
            this.buttonCopeland.Text = "Копленд";
            this.buttonCopeland.UseVisualStyleBackColor = true;
            // 
            // buttonCondorcet
            // 
            this.buttonCondorcet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCondorcet.Location = new System.Drawing.Point(680, 405);
            this.buttonCondorcet.Name = "buttonCondorcet";
            this.buttonCondorcet.Size = new System.Drawing.Size(101, 29);
            this.buttonCondorcet.TabIndex = 3;
            this.buttonCondorcet.Text = "Кондорсе";
            this.buttonCondorcet.UseVisualStyleBackColor = true;
            this.buttonCondorcet.Click += new System.EventHandler(this.buttonCondorcet_Click);
            // 
            // buttonPlurality
            // 
            this.buttonPlurality.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPlurality.Location = new System.Drawing.Point(573, 405);
            this.buttonPlurality.Name = "buttonPlurality";
            this.buttonPlurality.Size = new System.Drawing.Size(101, 29);
            this.buttonPlurality.TabIndex = 3;
            this.buttonPlurality.Text = "Большинство";
            this.buttonPlurality.UseVisualStyleBackColor = true;
            this.buttonPlurality.Click += new System.EventHandler(this.buttonPlurality_Click);
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.AutoScroll = true;
            this.panel.Location = new System.Drawing.Point(18, 12);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(1086, 387);
            this.panel.TabIndex = 10;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1120, 608);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.buttonPlurality);
            this.Controls.Add(this.buttonCondorcet);
            this.Controls.Add(this.buttonCopeland);
            this.Controls.Add(this.buttonSimpson);
            this.Controls.Add(this.buttonBorda);
            this.Controls.Add(this.buttonRemoveVotes);
            this.Controls.Add(this.buttonAddVotes);
            this.Controls.Add(this.textBox);
            this.Location = new System.Drawing.Point(15, 15);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Коллективное решение";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Button buttonAddVotes;
        private System.Windows.Forms.Button buttonRemoveVotes;

        private System.Windows.Forms.Button buttonBorda;
        private System.Windows.Forms.Button buttonSimpson;
        private System.Windows.Forms.Button buttonCopeland;
        private System.Windows.Forms.Button buttonCondorcet;
        private System.Windows.Forms.Button buttonPlurality;

        private System.Windows.Forms.Panel panel;
    }
}
