using OfficeOpenXml;
using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        int[] NValues = { 10, 100, 1000, 100000 }; // Размеры векторов
        int[] MValues = { 2, 3, 4, 5, 10 }; // Число потоков
        int[] KValues = { 1, 10, 100, 1000 }; // Параметр "сложности"

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Results");
            worksheet.Cells[1, 1].Value = "N";
            worksheet.Cells[1, 2].Value = "M";
            worksheet.Cells[1, 3].Value = "K";
            worksheet.Cells[1, 4].Value = "Sequential (ms)";
            worksheet.Cells[1, 5].Value = "Parallel (ms)";
            worksheet.Cells[1, 6].Value = "Circular Parallel (ms)";

            int row = 2;

            foreach (int N in NValues)
            {
                foreach (int M in MValues)
                {
                    foreach (int K in KValues)
                    {
                        double[] a = new double[N];
                        double[] b = new double[N];

                        // Заполнение вектора a (просто случайными числами)
                        Random random = new Random();
                        for (int i = 0; i < N; i++)
                        {
                            a[i] = random.NextDouble();
                        }

                        // Измеряем время последовательного выполнения
                        Stopwatch sw = new Stopwatch();
                        sw.Start();

                        for (int i = 0; i < N; i++)
                        {
                            for (int j = 0; j < K; j++)
                            {
                                b[i] += Math.Pow(a[i], 1.789);
                            }
                        }

                        sw.Stop();
                        TimeSpan sequentialTime = sw.Elapsed;

                        // Измеряем время параллельного выполнения
                        sw.Restart();

                        Parallel.For(0, N, i =>
                        {
                            for (int j = 0; j < K; j++)
                            {
                                b[i] += Math.Pow(a[i], 1.789);
                            }
                        });

                        sw.Stop();
                        TimeSpan parallelTime = sw.Elapsed;

                        // Измеряем время кругового параллельного выполнения
                        sw.Restart();

                        Parallel.For(0, N, new ParallelOptions { MaxDegreeOfParallelism = M }, i =>
                        {
                            if (i % 2 == 0)
                            {
                                for (int j = 0; j < K; j++)
                                {
                                    b[i] += Math.Pow(a[i], 1.789);
                                }
                            }
                            else
                            {
                                for (int j = 0; j < K; j++)
                                {
                                    b[i] += Math.Pow(a[i], 1.789);
                                }
                            }
                        });

                        sw.Stop();
                        TimeSpan circularParallelTime = sw.Elapsed;

                        worksheet.Cells[row, 1].Value = N;
                        worksheet.Cells[row, 2].Value = M;
                        worksheet.Cells[row, 3].Value = K;
                        worksheet.Cells[row, 4].Value = sequentialTime.TotalMilliseconds;
                        worksheet.Cells[row, 5].Value = parallelTime.TotalMilliseconds;
                        worksheet.Cells[row, 6].Value = circularParallelTime.TotalMilliseconds;

                        row++;
                    }
                }
            }

            package.SaveAs(new FileInfo("Results.xlsx"));
        }

        Console.WriteLine("Результаты сохранены в Results.xlsx");
    }
}
