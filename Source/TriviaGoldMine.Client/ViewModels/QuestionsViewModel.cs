namespace TriviaGoldMine.Client.ViewModels
{
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;

    using Excel;

    using GalaSoft.MvvmLight;

    using Models;

    public class QuestionsViewModel : ViewModelBase
    {
        public void ParseSpreadsheet(string path)
        {
            try
            {
                var stream = File.Open(path, FileMode.Open, FileAccess.Read);
                var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                var dataSet = excelReader.AsDataSet();

                var table = dataSet.Tables[0];
                var result = new List<Question>();
                for (int i = 1; i <= 20; i++)
                {
                    var row = table.Rows[i];
                    var number = int.Parse(row[0].ToString());
                    var points = int.Parse(row[1].ToString());
                    var mainQuestion = row[2].ToString();
                    var answer = row[3].ToString();
                    var alternateQuestion = row[6].ToString();
                    var question = new Question(number, points, mainQuestion, answer, alternateQuestion);
                    result.Add(question);
                }

                excelReader.Close();
            }
            catch
            {
                MessageBox.Show("Error when parsing the spreadsheet.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
