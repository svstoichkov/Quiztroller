namespace TriviaGoldMine.Client.ViewModels
{
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Ppt = Microsoft.Office.Interop.PowerPoint;

    public class PowerPointControllerViewModel : ViewModelBase
    {
        private Ppt.Application pptApplication;
        private Ppt.Presentation pptPresentation;
        private Ppt.Slide slide;
        private int slideIndex;
        private Ppt.Slides slides;
        private int slidesCount;

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
        }

        private bool CanShowSlideshow()
        {
            return this.pptApplication != null;
        }

        private void HandleShowSlideshow()
        {
            this.pptPresentation.SlideShowSettings.Run();
        }

        //public void Start(string path)
        //{
        //    Process.Start(path);
        //
        //    while (true)
        //    {
        //        try
        //        {
        //            this.pptApplication = Marshal.GetActiveObject("PowerPoint.Application") as Ppt.Application;
        //            this.pptApplication.PresentationClose += this.OnClose;
        //            this.pptPresentation = this.pptApplication.ActivePresentation;
        //            this.slides = this.pptPresentation.Slides;
        //            this.slidesCount = this.slides.Count;
        //            break;
        //        }
        //        catch
        //        {
        //        }
        //    }
        //
        //    try
        //    {
        //        this.slide = this.slides[this.pptApplication.ActiveWindow.Selection.SlideRange.SlideNumber];
        //    }
        //    catch
        //    {
        //        this.slide = this.pptApplication.SlideShowWindows[1].View.Slide;
        //    }
        //}

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