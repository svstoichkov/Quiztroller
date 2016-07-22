namespace Quizbuilder
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Microsoft.Azure;
    using Microsoft.Win32;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    using Quiztroller.Models;

    public class ManagePackagesViewModel : ViewModelBase
    {
        private readonly CloudBlobContainer container;
        private Visibility isLoading = Visibility.Collapsed;

        public ManagePackagesViewModel()
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var blobClient = storageAccount.CreateCloudBlobClient();
            this.container = blobClient.GetContainerReference("quizbuilder");
            this.container.CreateIfNotExists();

            this.GetPackages();

            this.Upload = new RelayCommand(this.HandleUpload);
            this.Delete = new RelayCommand<Quiz>(this.HandleDelete);
        }

        public ICommand Upload { get; }

        public ICommand Delete { get; }

        public Visibility IsLoading
        {
            get
            {
                return this.isLoading;
            }
            set
            {
                this.Set(() => this.IsLoading, ref this.isLoading, value);
            }
        }

        public ObservableCollection<Quiz> Packages { get; } = new ObservableCollection<Quiz>();

        private void GetPackages()
        {
            this.Packages.Clear();
            var blobs = this.container.ListBlobs().Cast<CloudBlockBlob>().ToList();
            foreach (var blob in blobs.Where(x => !x.Name.EndsWith("qzmz")))
            {
                var quiz = new Quiz();
                quiz.MainBlob = blob;
                quiz.PlaylistBlob = blobs.FirstOrDefault(x => x.Name == quiz.MainBlob.Name + ".qzmz");
                this.Packages.Add(quiz);
            }
        }

        private void HandleDelete(Quiz quiz)
        {
            quiz.MainBlob.Delete();
            quiz.PlaylistBlob.Delete();
            this.Packages.Remove(quiz);
        }

        private async void HandleUpload()
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.CheckFileExists = true;
            dialog.AddExtension = true;
            dialog.CheckPathExists = true;
            dialog.Filter = "Quiz package|*.qz;*.qzmz";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var showDialog = dialog.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                this.IsLoading = Visibility.Visible;

                if (dialog.FileNames.Length == 1 && !dialog.FileName.EndsWith("qz"))
                {
                    var filename = Path.GetFileNameWithoutExtension(dialog.FileName);
                    var blob = this.container.GetBlockBlobReference(filename);
                    using (var fileStream = File.OpenRead(dialog.FileName))
                    {
                        await blob.UploadFromStreamAsync(fileStream);
                    }
                }
                else if (dialog.FileNames.Length == 2 && dialog.FileNames.Any(x => x.EndsWith("qz")) && dialog.FileNames.Any(x => x.EndsWith("qzmz")))
                {
                    var quiz = Path.GetFileNameWithoutExtension(dialog.FileNames.First(x => x.EndsWith("qz")));
                    var blob1 = this.container.GetBlockBlobReference(quiz);
                    using (var fileStream = File.OpenRead(dialog.FileNames.First(x => x.EndsWith("qz"))))
                    {
                        await blob1.UploadFromStreamAsync(fileStream);
                    }

                    var playlist = Path.GetFileName(dialog.FileNames.First(x => x.EndsWith("qzmz")));
                    var blob2 = this.container.GetBlockBlobReference(playlist);
                    using (var fileStream = File.OpenRead(dialog.FileNames.First(x => x.EndsWith("qzmz"))))
                    {
                        await blob2.UploadFromStreamAsync(fileStream);
                    }
                }
                
                this.IsLoading = Visibility.Collapsed;
                this.GetPackages();
            }
        }
    }
}
