namespace Quiztroller.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Microsoft.Azure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    using Newtonsoft.Json;

    using TriviaGoldMine.Helpers.Models;

    public class QuestionsViewModel : ViewModelBase
    {
        private readonly PowerPointControllerViewModel controller;
        private readonly CloudBlobContainer container;

        private Question currentQuestion;
        private int currentQuestionIndex;
        private Visibility loadQuestionsVisibility = Visibility.Visible;
        private Visibility questionsLoadingVisibility = Visibility.Collapsed;

        public QuestionsViewModel(PowerPointControllerViewModel controller)
        {
            this.controller = controller;

            var blobClient = new CloudBlobClient(new Uri(@"https://quiztroller.blob.core.windows.net/"));
            this.container = blobClient.GetContainerReference("quizbuilder");
            this.container.CreateIfNotExists();

            this.GetPackages();

            this.Next = new RelayCommand(this.HandleNext, this.CanNext);
            this.Previous = new RelayCommand(this.HandlePrevious, this.CanPrevious);
            this.LoadPackage = new RelayCommand<CloudBlockBlob>(this.HandleLoadPackage);
            this.Reload = new RelayCommand(() => this.LoadQuestionsVisibility = Visibility.Visible, ()=> this.LoadQuestionsVisibility != Visibility.Visible);
        }

        public ICommand Next { get; }

        public ICommand Previous { get; }

        public ICommand LoadPackage { get; }

        public ICommand Reload { get; }

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

        public Visibility LoadQuestionsVisibility
        {
            get
            {
                return this.loadQuestionsVisibility;
            }
            set
            {
                this.Set(() => this.LoadQuestionsVisibility, ref this.loadQuestionsVisibility, value);
            }
        }

        public Visibility QuestionsLoadingVisibility
        {
            get
            {
                return this.questionsLoadingVisibility;
            }
            set
            {
                this.Set(() => this.QuestionsLoadingVisibility, ref this.questionsLoadingVisibility, value);
            }
        }

        public ObservableCollection<CloudBlockBlob> Packages { get; } = new ObservableCollection<CloudBlockBlob>();

        private void GetPackages()
        {
            this.Packages.Clear();
            var blobs = this.container.ListBlobs();
            foreach (var blob in blobs)
            {
                if (blob.GetType() == typeof(CloudBlockBlob))
                {
                    this.Packages.Add((CloudBlockBlob)blob);
                }
            }
        }

        public async void OpenQuizPackage(string path)
        {
            var process = Process.GetProcessesByName("POWERPNT").FirstOrDefault();
            process?.Kill();

            var tempFolder = Path.Combine(Path.GetTempPath(), "Quiztroller");
            if (Directory.Exists(tempFolder))
            {
                foreach (var file in Directory.GetFiles(tempFolder))
                {
                    while (true)
                    {
                        try
                        {
                            File.Delete(file);
                            await Task.Delay(1000);
                            break;
                        }
                        catch { }
                    }
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

            this.LoadQuestionsVisibility = Visibility.Collapsed;
        }

        private async void HandleLoadPackage(CloudBlockBlob blob)
        {
            using (var fileStream = File.OpenWrite("quiz"))
            {
                this.QuestionsLoadingVisibility = Visibility.Visible;
                await blob.DownloadToStreamAsync(fileStream);
                this.QuestionsLoadingVisibility = Visibility.Collapsed;
            }

            this.OpenQuizPackage("quiz");
        }

        private bool CanPrevious()
        {
            return this.currentQuestionIndex > 0;
        }

        private void HandlePrevious()
        {
            this.currentQuestionIndex--;
            this.CurrentQuestion = Questions[this.currentQuestionIndex];
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