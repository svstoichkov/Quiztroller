namespace TriviaGoldMine.Client.ViewModels
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Excel;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Models;

    public class QuestionsViewModel : ViewModelBase
    {
        private readonly List<Question> questions = new List<Question>(); 
        private Question currentQuestion;
        private int currentQuestionIndex;

        public QuestionsViewModel()
        {
            this.Next = new RelayCommand(this.HandleNext, this.CanNext);
            this.Previous = new RelayCommand(this.HandlePrevious, this.CanPrevious);
        }

        public ICommand Next { get; set; }

        public ICommand Previous { get; set; }

        public Question CurrentQuestion
        {
            get
            {
                return this.currentQuestion;
            }
            set
            {
                this.Set(() => this.CurrentQuestion, ref this.currentQuestion, value);
            }
        }

        private bool CanPrevious()
        {
            return this.currentQuestionIndex > 0;
        }

        private void HandlePrevious()
        {
            this.currentQuestionIndex--;
            this.CurrentQuestion = this.questions[this.currentQuestionIndex];
        }

        private void HandleNext()
        {
            this.currentQuestionIndex++;
            this.CurrentQuestion = this.questions[this.currentQuestionIndex];
        }

        private bool CanNext()
        {
            return this.currentQuestionIndex < this.questions.Count - 1;
        }

        public void ParseSpreadsheet(string path)
        {
            try
            {
                var stream = File.Open(path, FileMode.Open, FileAccess.Read);
                var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                var dataSet = excelReader.AsDataSet();

                var table = dataSet.Tables[0];
                this.questions.Clear();
                for (int i = 1; i <= 20; i++)
                {
                    var row = table.Rows[i];
                    var number = int.Parse(row[0].ToString());
                    var points = int.Parse(row[1].ToString());
                    var mainQuestion = row[2].ToString();
                    var answer = row[3].ToString();
                    var alternateQuestion = row[6].ToString();
                    var question = new Question(number, points, mainQuestion, answer, alternateQuestion);
                    this.questions.Add(question);
                }

                excelReader.Close();

                this.CurrentQuestion = this.questions.First();
            }
            catch
            {
                MessageBox.Show("Error when parsing the spreadsheet.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
