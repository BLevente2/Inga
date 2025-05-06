using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inga
{
    public class IngaCalculation
    {
        private double[,] matrix;
        private double[] vector;
        private int rows;
        private int innerMatrixSize;
        private int mainMatrixSize;
        private int lastColumn;
        private RichTextBox solutionBox;
        private TableControl matrixOnScreen;

        public IngaCalculation(
            double[,] matrix,
            double[] vector,
            int innerMatrixSize,
            int mainMatrixSize,
            RichTextBox solutionBox,
            TableControl matrixOnScreen)
        {
            this.matrix = matrix;
            this.vector = vector;
            this.rows = matrix.GetLength(0);
            this.innerMatrixSize = innerMatrixSize;
            this.mainMatrixSize = mainMatrixSize;
            this.solutionBox = solutionBox;
            this.matrixOnScreen = matrixOnScreen;
            this.lastColumn = matrixOnScreen.Data.GetLength(1) - 1;
        }

        public async Task CalculateAsync()
        {
            if (innerMatrixSize == 1)
                await SolveScalarAsync();
            else
                await SolveBlockAsync();
        }

        private async Task SolveScalarAsync()
        {
            int n = rows;
            await AddSolutionAsync("=== Skaláris Thomas-algoritmus ===");
            double[] a = new double[n], b = new double[n], c = new double[n];
            await AddSolutionAsync("Kiinduló diagonálisok:");
            for (int i = 0; i < n; i++)
            {
                c[i] = matrix[i, i];
                if (i < n - 1) b[i] = matrix[i, i + 1];
                if (i > 0) a[i] = matrix[i, i - 1];
                await AddSolutionAsync(
                    $"  c[{i + 1}] = {FormatNumber(c[i])}" +
                    (i < n - 1 ? $", b[{i + 1}] = {FormatNumber(b[i])}" : "") +
                    (i > 0 ? $", a[{i + 1}] = {FormatNumber(a[i])}" : ""));
            }

            double[] l = new double[n], k = new double[n], x = new double[n];

            await AddSolutionAsync("\n--- Előre sweep ---");
            await AddSolutionAsync("i = 1:");
            l[0] = vector[0] / c[0];
            k[0] = -b[0] / c[0];
            await AddSolutionAsync($"  l1 = f1/c1 = {FormatNumber(vector[0])}/{FormatNumber(c[0])} = {FormatNumber(l[0])}");
            await AddSolutionAsync($"  k1 = -b1/c1 = -{FormatNumber(b[0])}/{FormatNumber(c[0])} = {FormatNumber(k[0])}");

            for (int i = 1; i < n - 1; i++)
            {
                await AddSolutionAsync($"\ni = {i + 1}:");
                double denom = a[i] * k[i - 1] + c[i];
                await AddSolutionAsync(
                    $"  denom = a{i + 1}*k{i} + c{i + 1} = {FormatNumber(a[i])}*{FormatNumber(k[i - 1])} + {FormatNumber(c[i])} = {FormatNumber(denom)}");
                l[i] = (vector[i] - a[i] * l[i - 1]) / denom;
                await AddSolutionAsync(
                    $"  l{i + 1} = (f{i + 1} - a{i + 1}*l{i})/denom = ({FormatNumber(vector[i])} - {FormatNumber(a[i])}*{FormatNumber(l[i - 1])})/{FormatNumber(denom)} = {FormatNumber(l[i])}");
                k[i] = -b[i] / denom;
                await AddSolutionAsync(
                    $"  k{i + 1} = -b{i + 1}/denom = -{FormatNumber(b[i])}/{FormatNumber(denom)} = {FormatNumber(k[i])}");
            }

            await AddSolutionAsync("\n--- Utolsó komponens x_n ---");
            await AddSolutionAsync($"i = n = {n}:");
            double denomN = a[n - 1] * k[n - 2] + c[n - 1];
            await AddSolutionAsync(
                $"  denom = a{n}*k{n - 1} + c{n} = {FormatNumber(a[n - 1])}*{FormatNumber(k[n - 2])} + {FormatNumber(c[n - 1])} = {FormatNumber(denomN)}");
            x[n - 1] = (vector[n - 1] - a[n - 1] * l[n - 2]) / denomN;
            await AddSolutionAsync(
                $"  x{n} = (f{n} - a{n}*l{n - 1})/denom = ({FormatNumber(vector[n - 1])} - {FormatNumber(a[n - 1])}*{FormatNumber(l[n - 2])})/{FormatNumber(denomN)} = {FormatNumber(x[n - 1])}");
            await UpdateMatrixAsync(n - 1, lastColumn, FormatNumber(x[n - 1]));

            await AddSolutionAsync("\n--- Visszahelyettesítés ---");
            for (int i = n - 2; i >= 0; i--)
            {
                await AddSolutionAsync($"i = {i + 1}:");
                x[i] = l[i] + k[i] * x[i + 1];
                await AddSolutionAsync(
                    $"  x{i + 1} = l{i + 1} + k{i + 1}*x{i + 2} = {FormatNumber(l[i])} + {FormatNumber(k[i])}*{FormatNumber(x[i + 1])} = {FormatNumber(x[i])}");
                await UpdateMatrixAsync(i, lastColumn, FormatNumber(x[i]));
            }
        }

        private async Task SolveBlockAsync()
        {
            int N = innerMatrixSize, M = mainMatrixSize;
            await AddSolutionAsync("=== Blokk-Thomas algoritmus ===");

            // 1. szétszedés blokkokra
            double[][,] A = new double[M][,], B = new double[M][,], C = new double[M][,];
            double[][] F = new double[M][];
            await AddSolutionAsync("\n1. Kiinduló blokkok:");
            for (int i = 0; i < M; i++)
            {
                C[i] = new double[N, N];
                F[i] = new double[N];
                for (int r = 0; r < N; r++)
                {
                    for (int c = 0; c < N; c++)
                        C[i][r, c] = matrix[i * N + r, i * N + c];
                    F[i][r] = vector[i * N + r];
                }
                await AddSolutionAsync($"  C{i + 1} diag= [{string.Join(", ", Diag(C[i]))}]   F{i + 1}= [{string.Join(", ", F[i])}]");
                if (i < M - 1)
                {
                    B[i] = new double[N, N];
                    for (int r = 0; r < N; r++)
                        for (int c = 0; c < N; c++)
                            B[i][r, c] = matrix[i * N + r, (i + 1) * N + c];
                    await AddSolutionAsync($"  B{i + 1} diag= [{string.Join(", ", Diag(B[i]))}]");
                }
                if (i > 0)
                {
                    A[i] = new double[N, N];
                    for (int r = 0; r < N; r++)
                        for (int c = 0; c < N; c++)
                            A[i][r, c] = matrix[i * N + r, (i - 1) * N + c];
                    await AddSolutionAsync($"  A{i + 1} diag= [{string.Join(", ", Diag(A[i]))}]");
                }
            }

            // 2. alpha1, beta1
            await AddSolutionAsync("\n2. Számoljuk α1 és β1:");
            var invC1 = Invert(C[0]);
            var alpha1 = Negate(Multiply(invC1, B[0]));
            var beta1 = Multiply(invC1, F[0]);
            for (int r = 0; r < N; r++)
                await AddSolutionAsync($"  α1[{r + 1},*] = [{string.Join(", ", Row(alpha1, r))}]");
            for (int r = 0; r < N; r++)
                await AddSolutionAsync($"  β1[{r + 1}]   = {FormatNumber(beta1[r])}");
            await Task.Delay(500);

            // 3. előre sweep blokkonként
            for (int i = 1; i < M - 1; i++)
            {
                await AddSolutionAsync($"\n3.{i} Számoljuk S{i + 1} = A{i + 1}·α{i} + C{i + 1}:");
                var S = Add(Multiply(A[i], alpha1), C[i]);
                for (int r = 0; r < N; r++)
                    await AddSolutionAsync($"    S{i + 1}[{r + 1},*] = [{string.Join(", ", Row(S, r))}]");

                await AddSolutionAsync($"   α{i + 1} = -S⁻¹·B{i + 1},   β{i + 1} = S⁻¹·(F{i + 1} − A{i + 1}·β{i})");
                var invS = Invert(S);
                var alphai = Negate(Multiply(invS, B[i]));
                var betai = Multiply(invS, Subtract(F[i], Multiply(A[i], beta1)));
                for (int r = 0; r < N; r++)
                    await AddSolutionAsync($"    α{i + 1}[{r + 1},*] = [{string.Join(", ", Row(alphai, r))}]");
                for (int r = 0; r < N; r++)
                    await AddSolutionAsync($"    β{i + 1}[{r + 1}]   = {FormatNumber(betai[r])}");
                alpha1 = alphai; beta1 = betai;
                await Task.Delay(500);
            }

            // 4. utolsó blokk X_M
            await AddSolutionAsync($"\n4. Számoljuk X{M} = (A{M}·α{M - 1}+C{M})⁻¹·(F{M}−A{M}·β{M - 1}):");
            var Slast = Add(Multiply(A[M - 1], alpha1), C[M - 1]);
            var rhsLast = Subtract(F[M - 1], Multiply(A[M - 1], beta1));
            var Xlast = Multiply(Invert(Slast), rhsLast);
            for (int r = 0; r < N; r++)
            {
                await AddSolutionAsync($"  X{M}[{r + 1}] = {FormatNumber(Xlast[r])}");
                await UpdateMatrixAsync((M - 1) * N + r, lastColumn, FormatNumber(Xlast[r]));
            }

            // 5. visszahelyettesítés
            await AddSolutionAsync("\n5. Visszahelyettesítés blokkonként:");
            var next = Xlast;
            for (int i = M - 2; i >= 0; i--)
            {
                await AddSolutionAsync($" i = {i + 1}: X{i + 1} = α{i + 1}·X{i + 2} + β{i + 1}");
                var Xcur = AddVectors(Multiply(alpha1, next), beta1);
                for (int r = 0; r < N; r++)
                {
                    await AddSolutionAsync($"    X{i + 1}[{r + 1}] = {FormatNumber(Xcur[r])}");
                    await UpdateMatrixAsync(i * N + r, lastColumn, FormatNumber(Xcur[r]));
                }
                next = Xcur;
            }
        }

        private string FormatNumber(double v)
        {
            if (Math.Abs(v - Math.Round(v)) < 1e-10) return Math.Round(v).ToString();
            return v.ToString("F4");
        }

        private async Task AddSolutionAsync(string s)
        {
            solutionBox.AppendText(s + "\r\n");
            await Task.Delay(500);
        }

        private async Task UpdateMatrixAsync(int r, int c, string v)
        {
            matrixOnScreen.Data[r, c] = v;
            matrixOnScreen.Refresh();
            await Task.Delay(500);
        }

        private double[] Solve(double[,] A, double[] b)
        {
            int n = A.GetLength(0);
            var B = new double[n, 1];
            for (int i = 0; i < n; i++) B[i, 0] = b[i];
            var X = SolveMultiple(A, B);
            var x = new double[n];
            for (int i = 0; i < n; i++) x[i] = X[i, 0];
            return x;
        }

        private double[,] SolveMultiple(double[,] A, double[,] B)
        {
            int n = A.GetLength(0), m = B.GetLength(1);
            var aug = new double[n, n + m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) aug[i, j] = A[i, j];
                for (int j = 0; j < m; j++) aug[i, n + j] = B[i, j];
            }
            for (int i = 0; i < n; i++)
            {
                int pivot = i; double max = Math.Abs(aug[i, i]);
                for (int r = i + 1; r < n; r++)
                {
                    double val = Math.Abs(aug[r, i]);
                    if (val > max) { max = val; pivot = r; }
                }
                if (pivot != i)
                    for (int c = 0; c < n + m; c++)
                        (aug[i, c], aug[pivot, c]) = (aug[pivot, c], aug[i, c]);
                double diag = aug[i, i];
                for (int c = i; c < n + m; c++) aug[i, c] /= diag;
                for (int r = 0; r < n; r++) if (r != i)
                    {
                        double f = aug[r, i];
                        for (int c = i; c < n + m; c++)
                            aug[r, c] -= f * aug[i, c];
                    }
            }
            var X = new double[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    X[i, j] = aug[i, n + j];
            return X;
        }

        private double[,] Multiply(double[,] A, double[,] B)
        {
            int n = A.GetLength(0), m = A.GetLength(1), p = B.GetLength(1);
            var C = new double[n, p];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < p; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < m; k++) sum += A[i, k] * B[k, j];
                    C[i, j] = sum;
                }
            return C;
        }

        private double[] Multiply(double[,] A, double[] x)
        {
            int n = A.GetLength(0), m = A.GetLength(1);
            var y = new double[n];
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < m; j++) sum += A[i, j] * x[j];
                y[i] = sum;
            }
            return y;
        }

        private double[] Subtract(double[] a, double[] b)
        {
            int n = a.Length;
            var c = new double[n];
            for (int i = 0; i < n; i++) c[i] = a[i] - b[i];
            return c;
        }

        private double[,] Add(double[,] A, double[,] B)
        {
            int n = A.GetLength(0), m = A.GetLength(1);
            var C = new double[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    C[i, j] = A[i, j] + B[i, j];
            return C;
        }

        private double[] AddVectors(double[] a, double[] b)
        {
            int n = a.Length;
            var c = new double[n];
            for (int i = 0; i < n; i++) c[i] = a[i] + b[i];
            return c;
        }

        private double[,] Negate(double[,] A)
        {
            int n = A.GetLength(0), m = A.GetLength(1);
            var B = new double[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    B[i, j] = -A[i, j];
            return B;
        }

        private double[,] Invert(double[,] A)
        {
            int n = A.GetLength(0);
            var I = new double[n, n];
            for (int i = 0; i < n; i++) I[i, i] = 1;
            return SolveMultiple(A, I);
        }

        private double[] Row(double[,] A, int r)
        {
            int m = A.GetLength(1);
            var row = new double[m];
            for (int j = 0; j < m; j++) row[j] = A[r, j];
            return row;
        }

        private double[] Diag(double[,] A)
        {
            int n = Math.Min(A.GetLength(0), A.GetLength(1));
            var d = new double[n];
            for (int i = 0; i < n; i++) d[i] = A[i, i];
            return d;
        }
    }
}
