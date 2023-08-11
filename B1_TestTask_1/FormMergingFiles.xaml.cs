using Accessibility;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для FormMergingFiles.xaml
    /// </summary>
    public partial class FormMergingFiles : Window
    {
        public FormMergingFiles()
        {
            InitializeComponent();
        }

        private void MergingDeleteButtonClick(object sender, EventArgs e)
        {
            int totalDeletedStrings = 0;

            string folderPath = folderPathTextBox.Text; // ввод путя к папке
            string filterText = removeTextBox.Text; // ввод фильра для удаления


            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("Don't found the folder path.");
                return;
            }

            string[] fileNames = Directory.GetFiles(folderPath, "file*.txt");
            string mergedFile = System.IO.Path.Combine(folderPath, "merged_file.txt");
            

            using (StreamWriter mergedWriter = new StreamWriter(mergedFile)) // открываем поток 
            {
                foreach (string fileName in fileNames)
                {
                    int deletedStringsInFile = DeleteLinesFromFile(fileName, filterText);
                    totalDeletedStrings += deletedStringsInFile; // удаление и подсчет удаленных строк
                    using (StreamReader reader = new StreamReader(fileName)) // чтение содержимого файла и запись
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (!line.Contains(filterText))
                            {
                                mergedWriter.WriteLine(line);
                            }
                        }
                    }
                   
                }
               
            }
            deletedStringsLabel.Content = $"Deleted {totalDeletedStrings} strings";
            MessageBox.Show("Files merged and deleted successfully.");
        }  

        private int DeleteLinesFromFile(string filePath, string filterText)
        {
            int delete_strings = 0;

            List<string> new_strings = new List<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!line.Contains(filterText))
                    {
                        new_strings.Add(line);
                    }
                    else
                    {
                        delete_strings++;
                        MessageBox.Show($"Deleted [{filterText}] string from {filePath}");
                    }
                }

               
            }
           
            using (StreamWriter writer = new StreamWriter(filePath, false)) // запись нужных строк в файл
            {
                foreach (string new_string in new_strings)
                {
                    writer.WriteLine(new_string);
                }
            }

            return delete_strings;
        }
    }
}
