namespace Quiztroller.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Windows.Input;

    using Autofac;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Newtonsoft.Json;

    using TriviaGoldMine.Helpers.Models;

    public class QuestionsViewModel : ViewModelBase
    {
        private readonly PowerPointControllerViewModel controller;
        private Question currentQuestion;
        private int currentQuestionIndex;

        public QuestionsViewModel()
        {
            this.Next = new RelayCommand(this.HandleNext, this.CanNext);
            this.Previous = new RelayCommand(this.HandlePrevious, this.CanPrevious);

            this.controller = ViewModelLocator.Container.Resolve<PowerPointControllerViewModel>();
        }

        public ICommand Next { get; set; }

        public ICommand Previous { get; set; }

        public List<Question> Questions { get; set; } = new List<Question>();

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

        public void OpenQuizPackage(string path)
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), "Quiztroller");
            if (Directory.Exists(tempFolder))
            {
                foreach (var file in Directory.GetFiles(tempFolder))
                {
                    File.Delete(file);
                }
            }
            else
            {
                Directory.CreateDirectory(tempFolder);
            }

            using (var archive = ZipFile.Open(path, ZipArchiveMode.Read))
            {
                archive.ExtractToDirectory(tempFolder);
            }

            var pptx = Directory.GetFiles(tempFolder).First(x => x.Contains("pptx"));
            Process.Start(pptx);
            var questionsTxt = File.ReadAllText(Path.Combine(tempFolder, "questions.txt"));
            this.Questions = JsonConvert.DeserializeObject<List<Question>>(questionsTxt);
            this.CurrentQuestion = this.Questions.First();
        }

        private bool CanPrevious()
        {
            return this.currentQuestionIndex > 0;
        }

        private void HandlePrevious()
        {
            this.currentQuestionIndex--;
            this.CurrentQuestion = Questions[this.currentQuestionIndex];

            //this.controller.PreviousSlide();
        }

        private void HandleNext()
        {
            this.currentQuestionIndex++;
            this.CurrentQuestion = Questions[this.currentQuestionIndex];

            this.controller.NextSlide();
        }

        private bool CanNext()
        {
            return this.currentQuestionIndex < Questions.Count - 1;
        }
    }
}