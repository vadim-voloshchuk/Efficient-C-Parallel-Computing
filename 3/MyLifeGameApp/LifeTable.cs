namespace MyLifeGameApp
{
public class LifeTable
{
    private bool[,] table;
    public int Width { get; private set; }
    public int Height { get; private set; }


    public LifeTable(int height, int width)
    {
        Height = height;
        Width = width;
        table = new bool[Height, Width];
    }

    public void InitializeRandom()
    {
        // Инициализация случайными значениями
        Random random = new Random();

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                table[i, j] = random.Next(2) == 0; // 50% вероятность быть живой
            }
        }
    }

    public bool[,] GetTableState()
    {
        return table;
    }

    public void CalculateNextGeneration()
    {
        var tableNew = new bool[Height, Width];

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                int neighbors = CalcNeighbors(i, j);
                tableNew[i, j] = LifeRules(neighbors, table[i, j]);
            }
        }

        table = tableNew;
    }

    public void CalculateNextGenerationParallel()
{
    var tableNew = new bool[Height, Width];

    Parallel.For(0, Height, i => {
        for (int j = 0; j < Width; j++) {
            int neighbors = CalcNeighbors(i, j);
            tableNew[i, j] = LifeRules(neighbors, table[i, j]);
        }
    });

    table = tableNew;
}

public void CalculateNextGenerationParallelColumns()
{
    var tableNew = new bool[Height, Width];

    Parallel.For(0, Width, j => {
        for (int i = 0; i < Height; i++) {
            int neighbors = CalcNeighbors(i, j);
            tableNew[i, j] = LifeRules(neighbors, table[i, j]);
        }
    });

    table = tableNew;
}


    private int CalcNeighbors(int i, int j)
    {
        int neighbors = 0;

        for (int x = i - 1; x <= i + 1; x++)
        {
            for (int y = j - 1; y <= j + 1; y++)
            {
                if (x < 0 || y < 0 || x >= Height || y >= Width || (x == i && y == j))
                {
                    continue;
                }

                if (table[x, y])
                {
                    neighbors++;
                }
            }
        }

        return neighbors;
    }

    private bool LifeRules(int neighbors, bool state)
    {
        if (state) // Живая клетка
        {
            if (neighbors < 2 || neighbors > 3)
            {
                return false; // Умирает
            }
            return true; // Остается живой
        }
        else // Неживая клетка
        {
            if (neighbors == 3)
            {
                return true; // Оживает
            }
            return false; // Остается неживой
        }
    }
}
}
