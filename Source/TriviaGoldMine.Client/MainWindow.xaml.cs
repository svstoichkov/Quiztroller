namespace TriviaGoldMine.Client
{
    using System;
    using System.IO;
    using System.IO.Packaging;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Xps.Packaging;

    public partial class MainWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            //PptxToXpsConverter.ConvertPowerPointToXPSDoc(@"C:\Users\Svetlio\Desktop\ppt\ppt.pptx", @"C:\Users\Svetlio\Desktop\ppt\xps.xps");
            
            var xpsDoc = new XpsDocument(@"C:\Users\Svetlio\Desktop\ppt\xps.xps", FileAccess.Read);
            var fixedDocumentSequence = xpsDoc.GetFixedDocumentSequence();
            this.documentViewer.Document = fixedDocumentSequence; // displaying document in viewer
            xpsDoc.Close();
        }
    }
}
