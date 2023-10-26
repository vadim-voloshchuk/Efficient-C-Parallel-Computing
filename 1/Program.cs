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

            double x = 12345.6789;

            Console.WriteLine("\nПоследовательная обработка\n__________________________________________");
            for (int i = 1; i <= numIterations; i++)
            {
                char symbol = (char)('A' + i - 1); // Определение символа в зависимости от i

                // Выполнение последовательно
                DateTime t1 = DateTime.Now;

                for (int j = 0; j < i; j++)
                {
                    for (int l = 0; l < 10000; l++)
                            for (int s = 0; s < 10000; s++)
                            {
                                x = Math.Sqrt(x);
                                x = x + 0.000000001;
                                x = Math.Pow(x, 2);
                    }
                }

                DateTime t2 = DateTime.Now;
                double elapsedMillisecondsSequential = (t2 - t1).TotalMilliseconds;

                Console.WriteLine($"{symbol}: {elapsedMillisecondsSequential} ms");

                worksheet.Cells[i + 1, 1].Value = i;
                worksheet.Cells[i + 1, 2].Value = elapsedMillisecondsSequential;
            }

            x = 12345.6789; // Сброс переменной
            
            Console.WriteLine("\nПараллельная обработка\n__________________________________________");

            Parallel.For(1, numIterations, index =>
            {
                char symbol = (char)('A' + index - 1); // Определение символа в зависимости от i
                DateTime t3 = DateTime.Now;
                for (int j = 0; j < index; j++){
                        for (int l = 0; l < 10000; l++)
                            for (int s = 0; s < 10000; s++)
                            {
                                x = Math.Sqrt(x);
                                x = x + 0.000000001;
                                x = Math.Pow(x, 2);
                            }
                }
                DateTime t4 = DateTime.Now;
                double elapsedMillisecondsParallel = (t4 - t3).TotalMilliseconds;
                Console.WriteLine($"{symbol}: {elapsedMillisecondsParallel} ms");
                worksheet.Cells[index + 1, 3].Value = elapsedMillisecondsParallel;
            });
            x = 12345.6789; // Сброс переменной
            ExecuteCodeInParallel(numIterations, x);


                package.SaveAs(new FileInfo("Результаты.xlsx"));
            }
    }
    static void ExecuteCodeInParallel(int numIterations, double value)
    {
        Console.WriteLine("\nСтатика\n__________________________________________");
        Parallel.For(1, numIterations, index =>
        {
                char symbol = (char)('A' + index - 1);
                DateTime t3 = DateTime.Now;
                for (int j = 0; j < index; j++){

                        for (int l = 0; l < 10000; l++)
                            for (int s = 0; s < 10000; s++)
                            {
                                value = Math.Sqrt(value);
                                value = value + 0.000000001;
                                value = Math.Pow(value, 2);
                            }
                        
                }
                DateTime t4 = DateTime.Now;
                double elapsedMillisecondsParallel = (t4 - t3).TotalMilliseconds;
                Console.WriteLine($"{symbol}: {elapsedMillisecondsParallel} ms");
                // worksheet.Cells[index + 1, 3].Value = elapsedMillisecondsParallel;
            });
    }
}
