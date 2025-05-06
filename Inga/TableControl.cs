using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

public class TableControl : Control
{
    int rows = 1;
    int columns = 1;
    string[,] data = new string[1, 1];
    bool[,] cellReadOnly = new bool[1, 1];
    bool tableReadOnly;
    bool showGrid = true;
    Color gridColor = Color.Black;
    float gridThickness = 1f;
    Color textColor = Color.Black;
    TextBox editingTextBox;
    int editRow;
    int editColumn;

    public TableControl()
    {
        DoubleBuffered = true;
        SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
    }

    public override Font Font
    {
        get => base.Font;
        set { base.Font = value; Invalidate(); }
    }

    public int Rows
    {
        get => rows;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException();
            if (rows != value)
            {
                rows = value;
                data = new string[rows, columns];
                cellReadOnly = new bool[rows, columns];
                Invalidate();
            }
        }
    }

    public int Columns
    {
        get => columns;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException();
            if (columns != value)
            {
                columns = value;
                data = new string[rows, columns];
                cellReadOnly = new bool[rows, columns];
                Invalidate();
            }
        }
    }

    public string[,] Data
    {
        get => data;
        set
        {
            if (value.GetLength(0) != rows || value.GetLength(1) != columns) throw new ArgumentException();
            data = value;
            Invalidate();
        }
    }

    public bool[,] CellReadOnly
    {
        get => cellReadOnly;
        set
        {
            if (value.GetLength(0) != rows || value.GetLength(1) != columns) throw new ArgumentException();
            cellReadOnly = value;
        }
    }

    public bool ReadOnly
    {
        get => tableReadOnly;
        set => tableReadOnly = value;
    }

    public bool ShowGrid
    {
        get => showGrid;
        set { showGrid = value; Invalidate(); }
    }

    public Color GridColor
    {
        get => gridColor;
        set { gridColor = value; Invalidate(); }
    }

    public float GridThickness
    {
        get => gridThickness;
        set { gridThickness = value; Invalidate(); }
    }

    public Color TextColor
    {
        get => textColor;
        set { textColor = value; Invalidate(); }
    }

    public void SetCellReadOnly(int row, int column, bool readOnly)
    {
        if (row < 0 || row >= rows || column < 0 || column >= columns) throw new ArgumentOutOfRangeException();
        cellReadOnly[row, column] = readOnly;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (rows < 1 || columns < 1) return;

        var g = e.Graphics;
        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        float cw = (float)ClientSize.Width / columns;
        float ch = (float)ClientSize.Height / rows;
        float offset = gridThickness / 2f;

        if (showGrid)
        {
            using var pen = new Pen(gridColor, gridThickness)
            { Alignment = System.Drawing.Drawing2D.PenAlignment.Center };

            for (int c = 1; c < columns; c++)
                g.DrawLine(pen, c * cw, offset, c * cw, ClientSize.Height - offset);

            for (int r = 1; r < rows; r++)
                g.DrawLine(pen, offset, r * ch, ClientSize.Width - offset, r * ch);

            g.DrawRectangle(pen, offset, offset, ClientSize.Width - gridThickness, ClientSize.Height - gridThickness);
        }

        using var sf = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter,
            FormatFlags = StringFormatFlags.NoClip
        };

        using var brush = new SolidBrush(textColor);

        for (int r = 0; r < rows; r++)
            for (int c = 0; c < columns; c++)
            {
                var rect = new RectangleF(c * cw, r * ch, cw, ch);
                g.DrawString(data[r, c] ?? string.Empty, Font, brush, rect, sf);
            }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);

        if (editingTextBox != null) CommitEdit();
        if (tableReadOnly) return;

        float cw = (float)ClientSize.Width / columns;
        float ch = (float)ClientSize.Height / rows;
        int c = (int)(e.X / cw);
        int r = (int)(e.Y / ch);
        if (r < 0 || r >= rows || c < 0 || c >= columns) return;
        if (cellReadOnly[r, c]) return;

        BeginEdit(r, c, new Rectangle((int)(c * cw), (int)(r * ch), (int)cw, (int)ch));
    }

    void BeginEdit(int r, int c, Rectangle cellRect)
    {
        EndEdit();

        editingTextBox = new TextBox
        {
            BorderStyle = BorderStyle.None,
            Multiline = false,
            TextAlign = HorizontalAlignment.Center,
            Font = Font,
            ForeColor = textColor,
            BackColor = BackColor,
            Text = data[r, c] ?? string.Empty
        };

        int th = editingTextBox.PreferredHeight;
        int y = cellRect.Y + (cellRect.Height - th) / 2;

        editingTextBox.Bounds = new Rectangle(cellRect.X + 1, y, cellRect.Width - 2, th);

        editRow = r;
        editColumn = c;
        editingTextBox.LostFocus += EditingTextBox_LostFocus;
        editingTextBox.KeyDown += EditingTextBox_KeyDown;
        Controls.Add(editingTextBox);
        editingTextBox.Focus();
        editingTextBox.SelectAll();
    }

    void CommitEdit()
    {
        if (editingTextBox == null) return;
        data[editRow, editColumn] = editingTextBox.Text;
        EndEdit();
        Invalidate();
    }

    void EndEdit()
    {
        if (editingTextBox == null) return;
        editingTextBox.LostFocus -= EditingTextBox_LostFocus;
        editingTextBox.KeyDown -= EditingTextBox_KeyDown;
        Controls.Remove(editingTextBox);
        editingTextBox.Dispose();
        editingTextBox = null;
    }

    void EditingTextBox_LostFocus(object sender, EventArgs e) => CommitEdit();

    void EditingTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
        {
            e.SuppressKeyPress = true;
            CommitEdit();
        }
    }
}
