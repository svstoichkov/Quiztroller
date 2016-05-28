namespace Quizbuilder
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Helpers;

    using Microsoft.Win32;

    public class MainViewModel : ViewModelBase
    {
        private string excelPath;
        private string powerpointPath;
        private List<string> speedRound1ImagesPaths;
        private List<string> speedRound2ImagesPaths;

        public MainViewModel()
        {
            this.SelectPowerpoint = new RelayCommand(this.HandleSelectPowerpoint);
            this.SelectExcel = new RelayCommand(this.HandleSelectExcel);
            this.SelectSpeedRound1Images = new RelayCommand(this.HandleSelectSpeedRound1Images);
            this.SelectSpeedRound2Images = new RelayCommand(this.HandleSelectSpeedRound2Images);
            this.Save = new RelayCommand(this.HandleSave);//, this.CanSave);
        }

        public ICommand SelectPowerpoint { get; }

        public ICommand SelectExcel { get; }

        public ICommand SelectSpeedRound1Images { get; }

        public ICommand SelectSpeedRound2Images { get; }

        public ICommand Save { get; }

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

        public List<string> SpeedRound1ImagesPaths
        {
            get
            {
                return this.speedRound1ImagesPaths;
            }
            set
            {
                this.Set(() => this.SpeedRound1ImagesPaths, ref this.speedRound1ImagesPaths, value);
            }
        }

        public List<string> SpeedRound2ImagesPaths
        {
            get
            {
                return this.speedRound2ImagesPaths;
            }
            set
            {
                this.Set(() => this.SpeedRound2ImagesPaths, ref this.speedRound2ImagesPaths, value);
            }
        }

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

            var asd = PptxEditor.Edit(spreadsheet, this.SpeedRound1ImagesPaths, this.SpeedRound2ImagesPaths, this.PowerpointPath);

        }

        private void HandleSelectSpeedRound2Images()
        {
            var dialog = this.GetDialog("Image files|*.jpg", true);
            var showDialog = dialog.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                this.SpeedRound2ImagesPaths.Clear();
                this.SpeedRound2ImagesPaths.AddRange(dialog.FileNames);
            }
        }

        private void HandleSelectSpeedRound1Images()
        {
            var dialog = this.GetDialog("Image files|*.jpg", true);
            var showDialog = dialog.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                this.SpeedRound1ImagesPaths.Clear();
                this.SpeedRound1ImagesPaths.AddRange(dialog.FileNames);
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
    }
}