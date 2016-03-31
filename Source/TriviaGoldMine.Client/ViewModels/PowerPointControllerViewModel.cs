namespace Quiztroller.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Helpers;

    using Ppt = Microsoft.Office.Interop.PowerPoint;

    public class PowerPointControllerViewModel : ViewModelBase
    {
        private Ppt.Application pptApplication;
        private Ppt.Presentation pptPresentation;
        private Ppt.Slide slide;
        private int slideIndex;
        private Ppt.Slides slides;
        private int slidesCount;
        private Visibility visibility = Visibility.Visible;
        private string whatToDo = "Drop 1st image (*.jpg)";
        private List<string> paths = new List<string>();

        public PowerPointControllerViewModel()
        {
            this.FindPowerPoint = new RelayCommand(this.HandleFindPowerPoint, this.CanFindPowerPoint);
            this.Next = new RelayCommand(this.HandleNext, this.CanNext);
            this.Previous = new RelayCommand(this.HandlePrevious, this.CanPrevious);
            this.ShowSlideshow = new RelayCommand(this.HandleShowSlideshow, this.CanShowSlideshow);
        }

        public ICommand FindPowerPoint { get; set; }

        public ICommand ShowSlideshow { get; set; }

        public ICommand Next { get; set; }

        public ICommand Previous { get; set; }

        public Visibility Visibility
        {
            get
            {
                return this.visibility;
            }
            set
            {
                this.Set(() => this.Visibility, ref this.visibility, value);
            }
        }

        public string WhatToDo
        {
            get
            {
                return this.whatToDo;
            }
            set
            {
                this.Set(() => this.WhatToDo, ref this.whatToDo, value);
            }
        }

        public string AcceptedExtension { get; set; } = ".jpg";

        private bool CanFindPowerPoint()
        {
            return this.pptApplication == null;
        }

        private void HandleFindPowerPoint()
        {
            try
            {
                this.pptApplication = Marshal.GetActiveObject("PowerPoint.Application") as Ppt.Application;
                this.pptApplication.PresentationClose += this.OnClose;
                this.pptPresentation = this.pptApplication.ActivePresentation;
                this.slides = this.pptPresentation.Slides;
                this.slidesCount = this.slides.Count;
                try
                {
                    this.slide = this.slides[this.pptApplication.ActiveWindow.Selection.SlideRange.SlideNumber];
                }
                catch
                {
                    this.slide = this.pptApplication.SlideShowWindows[1].View.Slide;
                }
            }
            catch
            {
                this.pptApplication = null;
                this.slideIndex = 0;
                this.slidesCount = 0;
                this.slides = null;
                this.pptPresentation = null;
                this.slide = null;
                MessageBox.Show("Please run PowerPoint first", "PowerPoint not found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnClose(Ppt.Presentation pres)
        {
            this.pptApplication = null;
            this.slideIndex = 0;
            this.slidesCount = 0;
            this.slides = null;
            this.pptPresentation = null;
            this.slide = null;
            this.paths.Clear();
            this.AcceptedExtension = ".jpg";
            this.WhatToDo = "Drop 1st image (*.jpg)";
            this.Visibility = Visibility.Visible;
        }

        private bool CanShowSlideshow()
        {
            return this.pptApplication != null;
        }

        private void HandleShowSlideshow()
        {
            this.pptPresentation.SlideShowSettings.Run();
        }

        public void AddFilePath(string path)
        {
            this.paths.Add(path);
            if (this.paths.Count == 1)
            {
                this.WhatToDo = "Drop 2nd image (*.jpg)";
            }
            else if (this.paths.Count == 2)
            {
                this.WhatToDo = "Drop 3rd image (*.jpg)";
            }
            else if (this.paths.Count == 3)
            {
                this.WhatToDo = "Drop video (*.mp4)";
                this.AcceptedExtension = ".mp4";
            }
            else if (this.paths.Count == 4)
            {
                this.WhatToDo = "Drop presentation (*.pptx)";
                this.AcceptedExtension = ".pptx";
            }
            else if (this.paths.Count == 5)
            {
                try
                {
                    var editedPptx = PptxEditor.Edit(QuestionsViewModel.Questions, this.paths[0], this.paths[1], this.paths[2], this.paths[3], this.paths[4]);
                    this.Visibility = Visibility.Collapsed;
                    Process.Start(editedPptx);
                }
                catch
                {
                    MessageBox.Show("Pptx cannot be edited", "Invalid pptx format", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.paths.Remove(this.paths.Last());
                }
            }
        }

        private bool CanNext()
        {
            return this.pptApplication != null && this.slideIndex < this.slidesCount;
        }

        private void HandleNext()
        {
            this.slideIndex = this.slide.SlideIndex + 1;

            try
            {
                this.slide = this.slides[this.slideIndex];
                this.slides[this.slideIndex].Select();
            }
            catch
            {
                this.pptApplication.SlideShowWindows[1].View.Next();
                this.slide = this.pptApplication.SlideShowWindows[1].View.Slide;
            }
        }

        private bool CanPrevious()
        {
            return this.pptApplication != null && this.slideIndex > 1;
        }

        private void HandlePrevious()
        {
            this.slideIndex = this.slide.SlideIndex - 1;

            try
            {
                this.slide = this.slides[this.slideIndex];
                this.slides[this.slideIndex].Select();
            }
            catch
            {
                this.pptApplication.SlideShowWindows[1].View.Previous();
                this.slide = this.pptApplication.SlideShowWindows[1].View.Slide;
            }
        }
    }
}