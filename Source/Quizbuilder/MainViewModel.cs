namespace Quizbuilder
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Helpers;

    using Microsoft.Win32;

    using Newtonsoft.Json;

    public class MainViewModel : ViewModelBase
    {
        private string excelPath;
        private string powerpointPath;

        public MainViewModel()
        {
            this.SelectPowerpoint = new RelayCommand(this.HandleSelectPowerpoint);
            this.SelectExcel = new RelayCommand(this.HandleSelectExcel);
            this.SelectSpeedRound1Images = new RelayCommand(this.HandleSelectSpeedRound1Images);
            this.SelectSpeedRound2Images = new RelayCommand(this.HandleSelectSpeedRound2Images);
            this.Save = new RelayCommand(this.HandleSave, this.CanSave);

            this.MoveLeft1 = new RelayCommand<string>(this.HandleMoveLeft1, this.CanMoveLeft1);
            this.MoveLeft2 = new RelayCommand<string>(this.HandleMoveLeft2, this.CanMoveLeft2);

            this.MoveRight1 = new RelayCommand<string>(this.HandleMoveRight1, this.CanMoveRight1);
            this.MoveRight2 = new RelayCommand<string>(this.HandleMoveRight2, this.CanMoveRight2);

        }

        public ICommand SelectPowerpoint { get; }

        public ICommand SelectExcel { get; }

        public ICommand SelectSpeedRound1Images { get; }

        public ICommand SelectSpeedRound2Images { get; }

        public ICommand Save { get; }

        public ICommand MoveLeft1 { get; set; }

        public ICommand MoveLeft2 { get; set; }

        public ICommand MoveRight1 { get; set; }

        public ICommand MoveRight2 { get; set; }

        public string PowerpointPath
        {
            get
            {
                return this.powerpointPath;
            }
            set
            {
                this.Set(() => this.PowerpointPath, ref this.powerpointPath, value);
            }
        }

        public string ExcelPath
        {
            get
            {
                return this.excelPath;
            }
            set
            {
                this.Set(() => this.ExcelPath, ref this.excelPath, value);
            }
        }

        public ObservableCollection<string> SpeedRound1ImagesPaths { get; } = new ObservableCollection<string>();

        public ObservableCollection<string> SpeedRound2ImagesPaths { get; } = new ObservableCollection<string>();

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(this.PowerpointPath)
                   && !string.IsNullOrWhiteSpace(this.ExcelPath)
                   && this.SpeedRound1ImagesPaths.Count == 10
                   && this.SpeedRound2ImagesPaths.Count == 10;
        }

        private void HandleSave()
        {
            var questions = QuestionsParser.GetQuestions(this.ExcelPath);
            var spreadsheet = QuestionsParser.ParseSpreadsheet(this.ExcelPath);

            var pptxPath = PptxEditor.Edit(spreadsheet, this.SpeedRound1ImagesPaths, this.SpeedRound2ImagesPaths, this.PowerpointPath);

            var dialog = new SaveFileDialog();
            dialog.Filter = "Quiz|*.quiz";
            dialog.Title = "Select a location to save the quiz file";
            dialog.OverwritePrompt = true;
            dialog.CheckPathExists = true;
            dialog.AddExtension = true;
            dialog.DefaultExt = "quiz";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var showDialog = dialog.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                var fileName = dialog.FileName;
                var directory = Path.GetDirectoryName(pptxPath);
                File.WriteAllText(Path.Combine(directory, "questions.txt"), JsonConvert.SerializeObject(questions));
                ZipFile.CreateFromDirectory(directory, fileName, CompressionLevel.Optimal, false);
            }
        }

        private void HandleSelectSpeedRound2Images()
        {
            var dialog = this.GetDialog("Image files|*.jpg", true);
            var showDialog = dialog.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                if (dialog.FileNames.Count() != 10)
                {
                    MessageBox.Show("Image count must be 10", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    this.SpeedRound2ImagesPaths.Clear();
                    foreach (var fileName in dialog.FileNames)
                    {
                        this.SpeedRound2ImagesPaths.Add(fileName);
                    }
                }
            }
        }

        private void HandleSelectSpeedRound1Images()
        {
            var dialog = this.GetDialog("Image files|*.jpg", true);
            var showDialog = dialog.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                if (dialog.FileNames.Count() != 10)
                {
                    MessageBox.Show("Image count must be 10", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    this.SpeedRound1ImagesPaths.Clear();
                    foreach (var fileName in dialog.FileNames)
                    {
                        this.SpeedRound1ImagesPaths.Add(fileName);
                    }
                }
            }
        }

        private void HandleSelectExcel()
        {
            var dialog = this.GetDialog("Excel files|*.xlsx", false);
            var showDialog = dialog.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                this.ExcelPath = dialog.FileName;
            }
        }

        private void HandleSelectPowerpoint()
        {
            var dialog = this.GetDialog("Powerpoint files|*.pptx", false);
            var showDialog = dialog.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                this.PowerpointPath = dialog.FileName;
            }
        }

        private OpenFileDialog GetDialog(string filter, bool multiselect)
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = multiselect;
            dialog.CheckFileExists = true;
            dialog.AddExtension = true;
            dialog.CheckPathExists = true;
            dialog.Filter = filter;
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            return dialog;
        }

        private bool CanMoveRight2(string arg)
        {
            return this.SpeedRound2ImagesPaths.IndexOf(arg) < this.SpeedRound2ImagesPaths.Count - 1;
        }

        private void HandleMoveRight2(string obj)
        {
            var index = this.SpeedRound2ImagesPaths.IndexOf(obj);
            this.SpeedRound2ImagesPaths.Move(index, index + 1);
        }

        private bool CanMoveRight1(string arg)
        {
            return this.SpeedRound1ImagesPaths.IndexOf(arg) < this.SpeedRound1ImagesPaths.Count - 1;
        }

        private void HandleMoveRight1(string obj)
        {
            var index = this.SpeedRound1ImagesPaths.IndexOf(obj);
            this.SpeedRound1ImagesPaths.Move(index, index + 1);
        }

        private bool CanMoveLeft2(string path)
        {
            return this.SpeedRound2ImagesPaths.IndexOf(path) > 0;
        }

        private void HandleMoveLeft2(string path)
        {
            var index = this.SpeedRound2ImagesPaths.IndexOf(path);
            this.SpeedRound2ImagesPaths.Move(index, index - 1);
        }

        private bool CanMoveLeft1(string path)
        {
            return this.SpeedRound1ImagesPaths.IndexOf(path) > 0;
        }

        private void HandleMoveLeft1(string path)
        {
            var index = this.SpeedRound1ImagesPaths.IndexOf(path);
            this.SpeedRound1ImagesPaths.Move(index, index - 1);
        }
    }
}