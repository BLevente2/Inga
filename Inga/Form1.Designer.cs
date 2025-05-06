namespace Inga
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SolutionBox = new RichTextBox();
            ControlPanel = new Panel();
            InnerMatrixSizeSelector = new NumericUpDown();
            InnerMatrixSizeLabel = new Label();
            MatrixSizeSelector = new NumericUpDown();
            MatrixSizeLabel = new Label();
            CalculateButton = new Button();
            TablePanel = new Panel();
            ControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)InnerMatrixSizeSelector).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MatrixSizeSelector).BeginInit();
            SuspendLayout();
            // 
            // SolutionBox
            // 
            SolutionBox.BorderStyle = BorderStyle.FixedSingle;
            SolutionBox.Dock = DockStyle.Right;
            SolutionBox.Location = new Point(528, 0);
            SolutionBox.Name = "SolutionBox";
            SolutionBox.Size = new Size(691, 587);
            SolutionBox.TabIndex = 0;
            SolutionBox.Text = "";
            // 
            // ControlPanel
            // 
            ControlPanel.BorderStyle = BorderStyle.FixedSingle;
            ControlPanel.Controls.Add(InnerMatrixSizeSelector);
            ControlPanel.Controls.Add(InnerMatrixSizeLabel);
            ControlPanel.Controls.Add(MatrixSizeSelector);
            ControlPanel.Controls.Add(MatrixSizeLabel);
            ControlPanel.Dock = DockStyle.Top;
            ControlPanel.Location = new Point(0, 0);
            ControlPanel.Name = "ControlPanel";
            ControlPanel.Size = new Size(528, 47);
            ControlPanel.TabIndex = 1;
            // 
            // InnerMatrixSizeSelector
            // 
            InnerMatrixSizeSelector.BorderStyle = BorderStyle.FixedSingle;
            InnerMatrixSizeSelector.Dock = DockStyle.Left;
            InnerMatrixSizeSelector.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 238);
            InnerMatrixSizeSelector.Location = new Point(752, 0);
            InnerMatrixSizeSelector.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            InnerMatrixSizeSelector.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            InnerMatrixSizeSelector.Name = "InnerMatrixSizeSelector";
            InnerMatrixSizeSelector.Size = new Size(78, 45);
            InnerMatrixSizeSelector.TabIndex = 3;
            InnerMatrixSizeSelector.TextAlign = HorizontalAlignment.Center;
            InnerMatrixSizeSelector.Value = new decimal(new int[] { 1, 0, 0, 0 });
            InnerMatrixSizeSelector.ValueChanged += InnerMatrixSizeSelector_ValueChanged;
            // 
            // InnerMatrixSizeLabel
            // 
            InnerMatrixSizeLabel.AutoSize = true;
            InnerMatrixSizeLabel.Dock = DockStyle.Left;
            InnerMatrixSizeLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 238);
            InnerMatrixSizeLabel.Location = new Point(343, 0);
            InnerMatrixSizeLabel.Name = "InnerMatrixSizeLabel";
            InnerMatrixSizeLabel.Padding = new Padding(100, 0, 0, 0);
            InnerMatrixSizeLabel.Size = new Size(409, 38);
            InnerMatrixSizeLabel.TabIndex = 2;
            InnerMatrixSizeLabel.Text = "BelsőMátrixokMérete:";
            // 
            // MatrixSizeSelector
            // 
            MatrixSizeSelector.BorderStyle = BorderStyle.FixedSingle;
            MatrixSizeSelector.Dock = DockStyle.Left;
            MatrixSizeSelector.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 238);
            MatrixSizeSelector.Location = new Point(265, 0);
            MatrixSizeSelector.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            MatrixSizeSelector.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            MatrixSizeSelector.Name = "MatrixSizeSelector";
            MatrixSizeSelector.Size = new Size(78, 45);
            MatrixSizeSelector.TabIndex = 1;
            MatrixSizeSelector.TextAlign = HorizontalAlignment.Center;
            MatrixSizeSelector.Value = new decimal(new int[] { 3, 0, 0, 0 });
            MatrixSizeSelector.ValueChanged += MatrixSizeSelector_ValueChanged;
            // 
            // MatrixSizeLabel
            // 
            MatrixSizeLabel.AutoSize = true;
            MatrixSizeLabel.Dock = DockStyle.Left;
            MatrixSizeLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 238);
            MatrixSizeLabel.Location = new Point(0, 0);
            MatrixSizeLabel.Name = "MatrixSizeLabel";
            MatrixSizeLabel.Padding = new Padding(25, 0, 0, 0);
            MatrixSizeLabel.Size = new Size(265, 38);
            MatrixSizeLabel.TabIndex = 0;
            MatrixSizeLabel.Text = "MátrixNagysága:";
            // 
            // CalculateButton
            // 
            CalculateButton.Dock = DockStyle.Bottom;
            CalculateButton.FlatStyle = FlatStyle.Flat;
            CalculateButton.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 238);
            CalculateButton.ForeColor = SystemColors.ControlText;
            CalculateButton.Location = new Point(0, 526);
            CalculateButton.Name = "CalculateButton";
            CalculateButton.Size = new Size(528, 61);
            CalculateButton.TabIndex = 2;
            CalculateButton.Text = "Kiszámít";
            CalculateButton.UseVisualStyleBackColor = true;
            CalculateButton.Click += CalculateButton_Click;
            CalculateButton.MouseEnter += CalculateButton_MouseEnter;
            CalculateButton.MouseLeave += CalculateButton_MouseLeave;
            // 
            // TablePanel
            // 
            TablePanel.Dock = DockStyle.Fill;
            TablePanel.Location = new Point(0, 47);
            TablePanel.Margin = new Padding(35);
            TablePanel.Name = "TablePanel";
            TablePanel.Padding = new Padding(35);
            TablePanel.Size = new Size(528, 479);
            TablePanel.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1219, 587);
            Controls.Add(TablePanel);
            Controls.Add(CalculateButton);
            Controls.Add(ControlPanel);
            Controls.Add(SolutionBox);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Inga";
            WindowState = FormWindowState.Maximized;
            ControlPanel.ResumeLayout(false);
            ControlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)InnerMatrixSizeSelector).EndInit();
            ((System.ComponentModel.ISupportInitialize)MatrixSizeSelector).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox SolutionBox;
        private Panel ControlPanel;
        private Label MatrixSizeLabel;
        private NumericUpDown MatrixSizeSelector;
        private Label InnerMatrixSizeLabel;
        private NumericUpDown InnerMatrixSizeSelector;
        private Button CalculateButton;
        private Panel TablePanel;
    }
}
