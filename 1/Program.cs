using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using OfficeOpenXml;

class Program
{
    static void Main()
    {
        int numIterations = Environment.ProcessorCount; // Количество фрагментов кода для выполнения

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Результаты");

            worksheet.Cells[1, 1].Value = "Количество фрагментов";
            worksheet.Cells[1, 2].Value = "Время выполнения (мс) - Последовательное";
            worksheet.Cells[1, 3].Value = "Время выполнения (мс) - Многопоточное";

            for (int i = 1; i <= numIterations; i++)
            {
                double x = 12345.6789;

                // Выполнение последовательно
                DateTime t1 = DateTime.Now;

                for (int j = 0; j < i; j++)
                {
                    for (int k = 0; k < 10000; k++)
                    {
                        x = Math.Sqrt(x);
                        x = x + 0.000000001;
                        x = Math.Pow(x, 2);
                    }
                }

                DateTime t2 = DateTime.Now;
                double elapsedMillisecondsSequential = (t2 - t1).TotalMilliseconds;

                // Выполнение многопоточно
                x = 12345.6789; // Сброс переменной
                DateTime t3 = DateTime.Now;
                Parallel.For(0, i, index =>
                {
                    for (int j = 0; j < 10000; j++)
                    {
                        for (int k = 0; k < 10000; k++)
                        {
                            x = Math.Sqrt(x);
                            x = x + 0.000000001;
                            x = Math.Pow(x, 2);
                        }
                    }
                });

                DateTime t4 = DateTime.Now;
                double elapsedMillisecondsParallel = (t4 - t3).TotalMilliseconds;

                worksheet.Cells[i + 1, 1].Value = i;
                worksheet.Cells[i + 1, 2].Value = elapsedMillisecondsSequential;
                worksheet.Cells[i + 1, 3].Value = elapsedMillisecondsParallel;
            }

            package.SaveAs(new FileInfo("Результаты.xlsx"));
        }
    }
}
