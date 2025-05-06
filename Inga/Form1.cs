using System.Threading.Tasks;

namespace Inga
{
    public partial class Form1 : Form
    {
        public TableControl Table { get; set; }
        public Form1()
        {
            InitializeComponent();

            Table = new TableControl();
            Table.Dock = DockStyle.Fill;
            Table.BackColor = SystemColors.Control;
            Table.GridColor = SystemColors.ControlText;
            Table.TextColor = SystemColors.WindowText;
            Table.Font = new Font("Arial", 30);
            TablePanel.Controls.Add(Table);
            UpdateTable();
        }

        public void UpdateTable()
        {
            Table.ReadOnly = false;

            for (int i = 0; i < Table.Rows; i++)
            {
                for (int j = 0; j < Table.Columns; j++)
                {
                    Table.SetCellReadOnly(i, j, false);
                }
            }

            int x = (int)MatrixSizeSelector.Value;
            int y = (int)InnerMatrixSizeSelector.Value;
            int size = x * y;
            Table.Rows = size;
            Table.Columns = size + 1;

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    if (!(i >= j - 1 && i <= j + 1))
                    {

                        for (int k = 0; k < y; k++)
                        {
                            for (int l = 0; l < y; l++)
                            {
                                int rownum = i * y + k;
                                int colnum = j * y + l;
                                Table.Data[rownum, colnum] = "0";
                                Table.SetCellReadOnly(rownum, colnum, true);
                            }
                        }
                    }
                }
            }
        }

        private void MatrixSizeSelector_ValueChanged(object sender, EventArgs e)
        {
            UpdateTable();
        }

        private void InnerMatrixSizeSelector_ValueChanged(object sender, EventArgs e)
        {
            UpdateTable();
        }

        private async void CalculateButton_Click(object sender, EventArgs e)
        {
            string savedButtonText = CalculateButton.Text;
            try
            {
                CalculateButton.Text = "Számítás....";
                CalculateButton.Enabled = false;
                MatrixSizeSelector.Enabled = false;
                InnerMatrixSizeSelector.Enabled = false;
                SolutionBox.Clear();
                SolutionBox.ReadOnly = true;
                Table.ReadOnly = true;
                await Task.Delay(1); // Simulate some delay for UI update

                double[,] matrix = new double[Table.Rows, Table.Columns - 1];
                double[] vector = new double[Table.Rows];

                for (int i = 0; i < Table.Rows; i++)
                {
                    for (int j = 0; j < Table.Columns - 1; j++)
                    {
                        if (double.TryParse(Table.Data[i, j], out double value))
                        {
                            matrix[i, j] = value;
                        }
                        else
                        {
                            MessageBox.Show($"Invalid input at row {i + 1}, column {j + 1}");
                            return;
                        }
                    }
                    if (double.TryParse(Table.Data[i, Table.Columns - 1], out double vectorValue))
                    {
                        vector[i] = vectorValue;
                    }
                    else
                    {
                        MessageBox.Show($"Invalid input at row {i + 1}, last column");
                        return;
                    }
                }
                int matrixsize = (int)MatrixSizeSelector.Value;
                int innermatrixsize = (int)InnerMatrixSizeSelector.Value;

                IngaCalculation inga = new IngaCalculation(matrix, vector, innermatrixsize, matrixsize, SolutionBox, Table);

                await inga.CalculateAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                CalculateButton.Text = savedButtonText;
                CalculateButton.Enabled = true;
                MatrixSizeSelector.Enabled = true;
                InnerMatrixSizeSelector.Enabled = true;
                SolutionBox.ReadOnly = false;
                Table.ReadOnly = false;
            }
        }

        private void CalculateButton_MouseEnter(object sender, EventArgs e)
        {
            CalculateButton.BackColor = SystemColors.Highlight;
        }

        private void CalculateButton_MouseLeave(object sender, EventArgs e)
        {
            CalculateButton.BackColor = SystemColors.Control;
        }
    }
}
