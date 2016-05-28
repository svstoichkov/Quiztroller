namespace Quiztroller.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    using Autofac;

    using Excel;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Models;

    using Quizbuilder;

    public class QuestionsViewModel : ViewModelBase
    {
        private Question currentQuestion;
        private int currentQuestionIndex;
        private readonly PowerPointControllerViewModel controller;

        public QuestionsViewModel()
        {
            this.Next = new RelayCommand(this.HandleNext, this.CanNext);
            this.Previous = new RelayCommand(this.HandlePrevious, this.CanPrevious);

            this.controller = ViewModelLocator.Container.Resolve<PowerPointControllerViewModel>();
        }

        public ICommand Next { get; set; }

        public ICommand Previous { get; set; }

        public static List<Question> Questions { get; set; } = new List<Question>();

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
