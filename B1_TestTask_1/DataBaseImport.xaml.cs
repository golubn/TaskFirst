using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace B1_TestTask_1
{
    /// <summary>
    /// Логика взаимодействия для DataBaseImport.xaml
    /// </summary>
    public partial class DataBaseImport : Window
    {
        public DataBaseImport()
        {
            InitializeComponent();
        }


        private async void ImportToDb(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=G:\\TestB1\\TestB1db.db";
            string folderPath = folderPathTextBox.Text;
            

            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("Invalid folder path.");
                return;
            }

            string[] fileNames = Directory.GetFiles(folderPath, "file*.txt"); // получаем массив файлов

            await Task.Run(() => ImportFilesToDatabaseAsync(connectionString, fileNames)); // асинхронно импортируем файлы в бд
        }


        private async Task ImportFilesToDatabaseAsync(string connectionString, string[] fileNames)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                await connection.OpenAsync();

                foreach (string fileName in fileNames)
                {
                    using (StreamReader reader = new StreamReader(fileName))
                    {
                        string line;
                        int totalRowCount = File.ReadLines(fileName).Count();
                        int importedRowCount = 0;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] values = line.Split("||");
                            
                            DateTime date = DateTime.ParseExact(values[0], "dd.MM.yyyy", null);
                            string latin = values[1];
                            string russian = values[2];
                            int evenInt = int.Parse(values[3]);
                            double positiveDouble = double.Parse(values[4]);

                            string request = "INSERT INTO random_records (date, latin, russian, intNumber, doubleNumber) VALUES (@date, @latin, @russian, @intNumber, @doubleNumber)";

                            using (SqliteCommand command = new SqliteCommand(request, connection))
                            {
                                command.Parameters.AddWithValue("@date", date);
                                command.Parameters.AddWithValue("@latin", latin);
                                command.Parameters.AddWithValue("@russian", russian);
                                command.Parameters.AddWithValue("@intNumber", evenInt);
                                command.Parameters.AddWithValue("@doubleNumber", positiveDouble);

                                await command.ExecuteNonQueryAsync();
                            }

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                importProgressTextBlock.Text = $"Importing {importedRowCount} of {totalRowCount} rows...";
                            });

                            importedRowCount++;
                            

                        }
                    }
                }
            }
        }

        public void GenerateFile(object sender, EventArgs e)
        {
            string path = @"G:\Files";
            GeneratorFiles.GenerateFiles(path);
        }

        private void SumOfInt_MedianaOfDouble(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=G:\\TestB1\\TestB1db.db";
            string request = @"
            SELECT
                (SELECT SUM(intNumber) FROM random_records) AS SumInt,
                AVG(middle_value) AS MedianDouble
            FROM (
                SELECT doubleNumber AS middle_value
                FROM random_records
                ORDER BY middle_value
                LIMIT 1 OFFSET (SELECT COUNT(*) FROM random_records) / 2
            ) UNION ALL
            SELECT
                (SELECT SUM(intNumber) FROM random_records) AS SumInt,
                AVG(middle_value) AS MedianDouble
            FROM (
                SELECT doubleNumber AS middle_value
                FROM random_records
                ORDER BY middle_value
                LIMIT 2 OFFSET (SELECT COUNT(*) FROM random_records) / 2
            );";

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(request, connection))
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        long sumInt = Convert.ToInt64(reader["SumInt"]);
                        double medianDouble = Convert.ToDouble(reader["MedianDouble"]);

                        resultLabel.Content = $"Sum of Integers: {sumInt}\nMedian of Doubles: {medianDouble}";
                    }
                }
            }
        }
    }
}
