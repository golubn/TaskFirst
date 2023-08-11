using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1_TestTask_1
{
    internal class GeneratorFiles
    {
        public static void GenerateFiles(string folder_path)
        {
            const int file_count = 100;
            const int strings_in_file = 100000;

            Random random = new Random();

            if(!Directory.Exists(folder_path)) // создание директории если ее нет
            {
                Directory.CreateDirectory(folder_path);
            }

            for (int i = 1; i <= file_count; i++) // генерация файлов и заполнение их рандомными данными
            {
                string file_path = Path.Combine(folder_path, $"file{i}.txt");

                using (StreamWriter writer = new StreamWriter(file_path))
                {
                    for (int x = 0; x < strings_in_file; x++)
                    {
                        string date_random = GenerateRandomDate(random);
                        string latin = GenerateRandomString(random, 10);
                        string russian_random = GenerateRandomRussianString(random, 10);
                        int even_int_random = GenerateRandomEvenInteger(random, 1, 100000000);
                        double positive_double_random = GenerateRandomPositiveDouble(random, 1, 20);

                        string string_random = $"{date_random}||{latin}||{russian_random}||{even_int_random}||{positive_double_random:F8}||";
                        writer.WriteLine(string_random);
                    }
                }
            }

            Console.WriteLine("Files generated successfully.");
        }

        private static string GenerateRandomDate(Random random)
        {
            DateTime startDate = DateTime.Now.AddYears(-5);
            DateTime endDate = DateTime.Now;
            TimeSpan range = endDate - startDate;
            int randomDays = random.Next(0, (int)range.TotalDays);
            DateTime randomDate = startDate.AddDays(randomDays);
            return randomDate.ToString("dd.MM.yyyy");
        }

        private static string GenerateRandomString(Random random, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string GenerateRandomRussianString(Random random, int length)
        {
            const string chars = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static int GenerateRandomEvenInteger(Random random, int minValue, int maxValue)
        {
            int even_int_random = random.Next(minValue, maxValue + 1);
            return even_int_random % 2 == 0 ? even_int_random : even_int_random + 1;
        }

        private static double GenerateRandomPositiveDouble(Random random, double minValue, double maxValue)
        {
            return minValue + random.NextDouble() * (maxValue - minValue);
        }
    }
}
