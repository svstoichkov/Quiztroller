namespace Quiztroller.Models
{
    using Microsoft.WindowsAzure.Storage.Blob;

    public class Quiz
    {
        public CloudBlockBlob MainBlob { get; set; }

        public CloudBlockBlob PlaylistBlob { get; set; }
    }
}
