namespace SoundProcessing
{
    partial class SoundProcessing
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectOptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.t4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.f2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exercise4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rectanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vonHaanWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hammingWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.chart3 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart4 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart5 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.MValue = new System.Windows.Forms.TextBox();
            this.RValue = new System.Windows.Forms.TextBox();
            this.Raaa = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart5)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1862, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(120, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectOptionToolStripMenuItem,
            this.exercise4ToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // selectOptionToolStripMenuItem
            // 
            this.selectOptionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.t4ToolStripMenuItem,
            this.f2ToolStripMenuItem});
            this.selectOptionToolStripMenuItem.Name = "selectOptionToolStripMenuItem";
            this.selectOptionToolStripMenuItem.Size = new System.Drawing.Size(149, 26);
            this.selectOptionToolStripMenuItem.Text = "Exercise 3";
            this.selectOptionToolStripMenuItem.Click += new System.EventHandler(this.selectOptionToolStripMenuItem_Click);
            // 
            // t4ToolStripMenuItem
            // 
            this.t4ToolStripMenuItem.Name = "t4ToolStripMenuItem";
            this.t4ToolStripMenuItem.Size = new System.Drawing.Size(374, 26);
            this.t4ToolStripMenuItem.Text = "Average magnitude difference function (T4)";
            this.t4ToolStripMenuItem.Click += new System.EventHandler(this.t4ToolStripMenuItem_Click);
            // 
            // f2ToolStripMenuItem
            // 
            this.f2ToolStripMenuItem.Name = "f2ToolStripMenuItem";
            this.f2ToolStripMenuItem.Size = new System.Drawing.Size(374, 26);
            this.f2ToolStripMenuItem.Text = "Cepstrum (F2)";
            this.f2ToolStripMenuItem.Click += new System.EventHandler(this.f2ToolStripMenuItem_Click);
            // 
            // exercise4ToolStripMenuItem
            // 
            this.exercise4ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rectanToolStripMenuItem,
            this.vonHaanWindowToolStripMenuItem,
            this.hammingWindowToolStripMenuItem});
            this.exercise4ToolStripMenuItem.Name = "exercise4ToolStripMenuItem";
            this.exercise4ToolStripMenuItem.Size = new System.Drawing.Size(149, 26);
            this.exercise4ToolStripMenuItem.Text = "Exercise 4";
            // 
            // rectanToolStripMenuItem
            // 
            this.rectanToolStripMenuItem.Name = "rectanToolStripMenuItem";
            this.rectanToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.rectanToolStripMenuItem.Text = "Rectangular window";
            this.rectanToolStripMenuItem.Click += new System.EventHandler(this.rectanToolStripMenuItem_Click);
            // 
            // vonHaanWindowToolStripMenuItem
            // 
            this.vonHaanWindowToolStripMenuItem.Name = "vonHaanWindowToolStripMenuItem";
            this.vonHaanWindowToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.vonHaanWindowToolStripMenuItem.Text = "Von Haan window";
            this.vonHaanWindowToolStripMenuItem.Click += new System.EventHandler(this.vonHaanWindowToolStripMenuItem_Click);
            // 
            // hammingWindowToolStripMenuItem
            // 
            this.hammingWindowToolStripMenuItem.Name = "hammingWindowToolStripMenuItem";
            this.hammingWindowToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.hammingWindowToolStripMenuItem.Text = "Hamming window";
            this.hammingWindowToolStripMenuItem.Click += new System.EventHandler(this.hammingWindowToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 78);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 42);
            this.button1.TabIndex = 4;
            this.button1.Text = "Play sound";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(161, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "File name: ";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 141);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(159, 40);
            this.button2.TabIndex = 6;
            this.button2.Text = "Play changed sound";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 50);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(108, 38);
            this.button3.TabIndex = 7;
            this.button3.Text = "Calculate";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // chart3
            // 
            chartArea4.Name = "ChartArea1";
            this.chart3.ChartAreas.Add(chartArea4);
            this.chart3.Location = new System.Drawing.Point(12, 50);
            this.chart3.Name = "chart3";
            this.chart3.Size = new System.Drawing.Size(1838, 286);
            this.chart3.TabIndex = 8;
            this.chart3.Text = "chart3";
            // 
            // chart4
            // 
            chartArea5.Name = "ChartArea1";
            this.chart4.ChartAreas.Add(chartArea5);
            this.chart4.Location = new System.Drawing.Point(12, 342);
            this.chart4.Name = "chart4";
            this.chart4.Size = new System.Drawing.Size(1838, 328);
            this.chart4.TabIndex = 9;
            this.chart4.Text = "chart4";
            // 
            // chart5
            // 
            chartArea6.Name = "ChartArea1";
            this.chart5.ChartAreas.Add(chartArea6);
            this.chart5.Location = new System.Drawing.Point(12, 676);
            this.chart5.Name = "chart5";
            this.chart5.Size = new System.Drawing.Size(1838, 309);
            this.chart5.TabIndex = 10;
            this.chart5.Text = "chart5";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 50);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(58, 22);
            this.textBox1.TabIndex = 11;
            this.textBox1.Visible = false;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(552, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 17);
            this.label2.TabIndex = 12;
            this.label2.Text = "M:";
            // 
            // MValue
            // 
            this.MValue.Location = new System.Drawing.Point(581, 9);
            this.MValue.Name = "MValue";
            this.MValue.Size = new System.Drawing.Size(43, 22);
            this.MValue.TabIndex = 13;
            this.MValue.Text = "2049";
            this.MValue.TextChanged += new System.EventHandler(this.MValue_TextChanged);
            // 
            // RValue
            // 
            this.RValue.Location = new System.Drawing.Point(671, 9);
            this.RValue.Name = "RValue";
            this.RValue.Size = new System.Drawing.Size(46, 22);
            this.RValue.TabIndex = 14;
            this.RValue.Text = "1024";
            this.RValue.TextChanged += new System.EventHandler(this.RValue_TextChanged);
            // 
            // Raaa
            // 
            this.Raaa.AutoSize = true;
            this.Raaa.Location = new System.Drawing.Point(643, 12);
            this.Raaa.Name = "Raaa";
            this.Raaa.Size = new System.Drawing.Size(22, 17);
            this.Raaa.TabIndex = 15;
            this.Raaa.Text = "R:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(966, 9);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(95, 23);
            this.button4.TabIndex = 16;
            this.button4.Text = "Calculate";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(733, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 17);
            this.label3.TabIndex = 20;
            this.label3.Text = "L:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(826, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 17);
            this.label4.TabIndex = 21;
            this.label4.Text = "fc:";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(759, 10);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(48, 22);
            this.textBox4.TabIndex = 22;
            this.textBox4.Text = "1023";
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(855, 10);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(53, 22);
            this.textBox5.TabIndex = 23;
            this.textBox5.Text = "300";
            this.textBox5.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1166, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 17);
            this.label5.TabIndex = 24;
            this.label5.Text = "Time domain duration: ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1423, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(189, 17);
            this.label6.TabIndex = 25;
            this.label6.Text = "Frequency domain duration: ";
            // 
            // SoundProcessing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1862, 1055);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.Raaa);
            this.Controls.Add(this.RValue);
            this.Controls.Add(this.MValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.chart5);
            this.Controls.Add(this.chart4);
            this.Controls.Add(this.chart3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SoundProcessing";
            this.Text = "Sound Processing";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectOptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem t4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem f2ToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart4;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem exercise4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rectanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vonHaanWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hammingWindowToolStripMenuItem;
        private System.Windows.Forms.TextBox MValue;
        private System.Windows.Forms.TextBox RValue;
        private System.Windows.Forms.Label Raaa;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}

