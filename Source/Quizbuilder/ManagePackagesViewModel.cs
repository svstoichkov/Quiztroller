namespace Quizbuilder
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Microsoft.Azure;
    using Microsoft.Win32;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

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
            this.Delete = new RelayCommand<CloudBlockBlob>(this.HandleDelete);
        }

        public ICommand Upload { get; }

        public ICommand Delete { get; }

        public ObservableCollection<CloudBlockBlob> Packages { get; } = new ObservableCollection<CloudBlockBlob>();

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

        private void GetPackages()
        {
            this.Packages.Clear();
            var blobs = this.container.ListBlobs();
            foreach (var blob in blobs)
            {
                if (blob.GetType() == typeof (CloudBlockBlob))
                {
                    this.Packages.Add((CloudBlockBlob)blob);
                }
            }
        }

        private void HandleDelete(CloudBlockBlob blob)
        {
            blob.Delete();
            this.Packages.Remove(blob);
        }

        private async void HandleUpload()
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.CheckFileExists = true;
            dialog.AddExtension = true;
            dialog.CheckPathExists = true;
            dialog.Filter = "Quiz package|*.qz";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var showDialog = dialog.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                var filename = Path.GetFileNameWithoutExtension(dialog.FileName);
                var blob = this.container.GetBlockBlobReference(filename);
                using (var fileStream = File.OpenRead(dialog.FileName))
                {
                    this.IsLoading = Visibility.Visible;
                    await blob.UploadFromStreamAsync(fileStream);
                    this.IsLoading = Visibility.Collapsed;
                    this.GetPackages();
                }
            }
        }
    }
}
