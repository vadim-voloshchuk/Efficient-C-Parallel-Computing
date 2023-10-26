    using System;
    using System.Windows.Forms;

    namespace MyLifeGameApp
    {
        public partial class Form1 : Form
        {
            private DataGridView dataGridView;
            private Button startButton; 
            private Button stopButton; 
            private Button parallButton;
            private Button parallColumnsButton; 
            private LifeTable lifeTable;
            private System.Windows.Forms.Timer gameTimer; // Переместил сюда

            public Form1()
            {
                InitializeComponent();

                startButton = new Button();
                startButton.Text = "Start Game";
                startButton.Click += StartButton_Click;

                stopButton = new Button();
                stopButton.Text = "Stop Game";
                stopButton.Click += StopButton_Click;

                parallButton = new Button();
                parallButton.Text = "Parallel Game";
                parallButton.Click += ParallelButton_Click;

                parallColumnsButton = new Button();
                parallColumnsButton.Text = "Parallel Columns Game";
                parallColumnsButton.Click += ParallelColumnsButton_Click;

                stopButton = new Button();
                stopButton.Text = "Stop Game";
                stopButton.Click += StopButton_Click;

                Controls.Add(startButton); 
                Controls.Add(stopButton);
                Controls.Add(parallButton);
                Controls.Add(parallColumnsButton);

                startButton.Location = new Point(10, 10); 
                startButton.Size = new Size(100, 30); 

                stopButton.Location = new Point(130, 10); 
                stopButton.Size = new Size(100, 30); 

                parallButton.Location = new Point(250, 10); 
                parallButton.Size = new Size(100, 30); 

                parallColumnsButton.Location = new Point(370, 10); 
                parallColumnsButton.Size = new Size(100, 30); 

                lifeTable = new LifeTable(25, 25);

dataGridView = new DataGridView();
dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
dataGridView.Dock = DockStyle.Fill;
dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

Controls.Add(dataGridView);

int desiredWidth = (int)(this.Width * 0.8);
int desiredHeight = (int)(this.Height * 0.8);

dataGridView.Width = desiredWidth;
dataGridView.Height = desiredHeight;

// Центрирование таблицы
dataGridView.Location = new Point((this.ClientRectangle.Width - desiredWidth) / 2, (this.ClientRectangle.Height - desiredHeight) / 2);

dataGridView.ColumnHeadersVisible = false;
dataGridView.RowHeadersVisible = false;



                dataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(dataGridView_CellFormatting);


                lifeTable.InitializeRandom();

                UpdateDataGridView();
            }

private void UpdateDataGridView()
{
    dataGridView.Rows.Clear();
    dataGridView.Columns.Clear();

    bool[,] table = lifeTable.GetTableState();

    dataGridView.RowCount = table.GetLength(0);
    dataGridView.ColumnCount = table.GetLength(1);

    for (int i = 0; i < table.GetLength(0); i++)
    {
        for (int j = 0; j < table.GetLength(1); j++)
        {
            dataGridView.Rows[i].Cells[j].Value = table[i, j];

            dataGridView.Rows[i].Cells[j].Style.ForeColor = Color.Transparent;

        }
    }
}

private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
{
    if (e.Value != null && e.Value is bool)
    {
        bool cellValue = (bool)e.Value;
        if (cellValue)
        {
            e.CellStyle.BackColor = Color.Green; // Если значение true, установите зеленый цвет фона
        }
        else
        {
            e.CellStyle.BackColor = Color.Red; // Если значение false, установите красный цвет фона
        }
    }
}





            private void StartButton_Click(object sender, EventArgs e)
            {
                StartGame();
            }

            private void StartGame()
            {
                gameTimer = new System.Windows.Forms.Timer();
                gameTimer.Interval = 10; // 1 секунда
                gameTimer.Tick += (s, ev) =>
                {
                    lifeTable.CalculateNextGeneration(); // Рассчет следующего поколения
                    UpdateDataGridView(); // Обновление отображения
                };
                gameTimer.Start();
            }

            private void StartGameParall()
            {
                gameTimer = new System.Windows.Forms.Timer();
                gameTimer.Interval = 1; // 1 секунда
                gameTimer.Tick += (s, ev) =>
                {
                    lifeTable.CalculateNextGenerationParallel(); // Рассчет следующего поколения
                    UpdateDataGridView(); // Обновление отображения
                };
                gameTimer.Start();
            }

            private void StartGameParallColumns()
            {
                gameTimer = new System.Windows.Forms.Timer();
                gameTimer.Interval = 10; // 1 секунда
                gameTimer.Tick += (s, ev) =>
                {
                    lifeTable.CalculateNextGenerationParallelColumns(); // Рассчет следующего поколения
                    UpdateDataGridView(); // Обновление отображения
                };
                gameTimer.Start();
            }


            private void StopGame()
            {
                if (gameTimer != null && gameTimer.Enabled)
                {
                    gameTimer.Stop();
                    gameTimer.Dispose();
                }
            }


            private void StopButton_Click(object sender, EventArgs e)
            {
                StopGame();
            }

            private void ParallelButton_Click(object sender, EventArgs e)
            {
                StartGameParall();
            }

            private void ParallelColumnsButton_Click(object sender, EventArgs e)
            {
                StartGameParallColumns();
            }
        }
    }
