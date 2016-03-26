namespace TriviaGoldMine.Client
{
    using System;

    using Microsoft.Office.Interop.PowerPoint;

    public static class PptxToXpsConverter
    {
        private static _Application ppApp;

        static PptxToXpsConverter()
        {
            ppApp = new Microsoft.Office.Interop.PowerPoint.Application();
            ppApp.DisplayAlerts = PpAlertLevel.ppAlertsNone;
        }

        /// 

        /// It uses powerpoint to save the specified input file in the xps format to the specified destination file.
        /// It considers all the slides but no notes.
        /// 
        public static void ConvertPowerPointToXPSDoc(string ppDocName, string xpsDocName)
        {
            ppApp.Presentations.Open(ppDocName, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse);

            var p = ppApp.Presentations[ppApp.Presentations.Count];

            try
            {
                p.ExportAsFixedFormat(xpsDocName, PpFixedFormatType.ppFixedFormatTypeXPS);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                p.Close();
            }
        }
    }
}
