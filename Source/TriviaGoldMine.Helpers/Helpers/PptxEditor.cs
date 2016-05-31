namespace TriviaGoldMine.Helpers.Helpers
{
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Web;

    using Models;

    public static class PptxEditor
    {
        public static string Edit(SpreadsheetParseResult spreadsheet, IList<string> round1Images, IList<string> round2Images, string pptxPath)
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), "Quizbuilder");
            var newFile = Path.Combine(tempFolder, Path.GetFileName(pptxPath));

            if (Directory.Exists(tempFolder))
            {
                foreach (var file in Directory.GetFiles(tempFolder))
                {
                    File.Delete(file);
                }
            }
            else
            {
                Directory.CreateDirectory(tempFolder);
            }

            File.Copy(pptxPath, newFile);

            using (var archive = ZipFile.Open(newFile, ZipArchiveMode.Update))
            {
                ReplaceMedia(round1Images, round2Images, archive);
                ReplaceQuestions(spreadsheet, archive, tempFolder);
            }

            var slides = Directory.GetFiles(tempFolder).Where(x => x.Contains("slide"));
            foreach (var slide in slides)
            {
                File.Delete(slide);
            }

            return newFile;
        }

        private static void ReplaceQuestions(SpreadsheetParseResult spreadsheet, ZipArchive archive, string tempFolder)
        {
            var slides = archive.Entries.Where(x => x.FullName.StartsWith("ppt/slides/") && x.FullName.EndsWith(".xml"));
            slides = slides.OrderBy(x => int.Parse(x.FullName.Substring(x.FullName.LastIndexOf('e') + 1, x.FullName.IndexOf('.') - x.FullName.LastIndexOf('e') - 1))).ToList();

            foreach (var slide in slides)
            {
                var slidePath = Path.Combine(tempFolder, Path.GetFileName(slide.FullName));
                slide.ExtractToFile(slidePath);

                var content = File.ReadAllText(slidePath);

                while (content.Contains("QSTN_"))
                {
                    var qstnIndex = content.IndexOf("QSTN_") + 5;
                    if (qstnIndex > -1)
                    {
                        var questionNumber = int.Parse(content.Substring(qstnIndex, content.IndexOf("<", qstnIndex) - qstnIndex));
                        content = content.Replace($"QSTN_{questionNumber}", HttpUtility.HtmlEncode(spreadsheet.Questions[questionNumber - 1]));
                    }
                }

                while (content.Contains("CAT_"))
                {
                    var catIndex = content.IndexOf("CAT_") + 4;
                    if (catIndex > -1)
                    {
                        var categoryNumber = int.Parse(content.Substring(catIndex, content.IndexOf("<", catIndex) - catIndex));
                        content = content.Replace($"CAT_{categoryNumber}", HttpUtility.HtmlEncode(spreadsheet.Categories[categoryNumber - 1]));
                    }
                }

                while (content.Contains("ANSWR_"))
                {
                    var catIndex = content.IndexOf("ANSWR_") + 6;
                    if (catIndex > -1)
                    {
                        var categoryNumber = int.Parse(content.Substring(catIndex, content.IndexOf("<", catIndex) - catIndex));
                        var answer = "";
                        try
                        {
                            answer = HttpUtility.HtmlEncode(spreadsheet.Answers[categoryNumber - 1]);
                        }
                        catch
                        {
                        }

                        content = content.Replace($"ANSWR_{categoryNumber}", answer);
                    }
                }

                File.WriteAllText(slidePath, content);

                slide.Delete();
                archive.CreateEntryFromFile(slidePath, slide.FullName);
            }
        }

        private static void ReplaceMedia(IList<string> round1Images, IList<string> round2Images, ZipArchive archive)
        {
            var img1 = archive.Entries.Single(x => x.Length == 9928);
            var img2 = archive.Entries.Single(x => x.Length == 17738);
            var img3 = archive.Entries.Single(x => x.Length == 13483);
            var img4 = archive.Entries.Single(x => x.Length == 13380);
            var img5 = archive.Entries.Single(x => x.Length == 10562);
            var img6 = archive.Entries.Single(x => x.Length == 13320);
            var img7 = archive.Entries.Single(x => x.Length == 11377);
            var img8 = archive.Entries.Single(x => x.Length == 13146);
            var img9 = archive.Entries.Single(x => x.Length == 11773);
            var img10 = archive.Entries.Single(x => x.Length == 13896);

            var img11 = archive.Entries.Single(x => x.Length == 7505);
            var img12 = archive.Entries.Single(x => x.Length == 5147);
            var img13 = archive.Entries.Single(x => x.Length == 4899);
            var img14 = archive.Entries.Single(x => x.Length == 7320);
            var img15 = archive.Entries.Single(x => x.Length == 11987);
            var img16 = archive.Entries.Single(x => x.Length == 16761);
            var img17 = archive.Entries.Single(x => x.Length == 6435);
            var img18 = archive.Entries.Single(x => x.Length == 4985);
            var img19 = archive.Entries.Single(x => x.Length == 8045);
            var img20 = archive.Entries.Single(x => x.Length == 11981);

            img1.Delete();
            img2.Delete();
            img3.Delete();
            img4.Delete();
            img5.Delete();
            img6.Delete();
            img7.Delete();
            img8.Delete();
            img9.Delete();
            img10.Delete();

            img11.Delete();
            img12.Delete();
            img13.Delete();
            img14.Delete();
            img15.Delete();
            img16.Delete();
            img17.Delete();
            img18.Delete();
            img19.Delete();
            img20.Delete();

            archive.CreateEntryFromFile(round1Images[0], img1.FullName);
            archive.CreateEntryFromFile(round1Images[1], img2.FullName);
            archive.CreateEntryFromFile(round1Images[2], img3.FullName);
            archive.CreateEntryFromFile(round1Images[3], img4.FullName);
            archive.CreateEntryFromFile(round1Images[4], img5.FullName);
            archive.CreateEntryFromFile(round1Images[5], img6.FullName);
            archive.CreateEntryFromFile(round1Images[6], img7.FullName);
            archive.CreateEntryFromFile(round1Images[7], img8.FullName);
            archive.CreateEntryFromFile(round1Images[8], img9.FullName);
            archive.CreateEntryFromFile(round1Images[9], img10.FullName);

            archive.CreateEntryFromFile(round2Images[0], img11.FullName);
            archive.CreateEntryFromFile(round2Images[1], img12.FullName);
            archive.CreateEntryFromFile(round2Images[2], img13.FullName);
            archive.CreateEntryFromFile(round2Images[3], img14.FullName);
            archive.CreateEntryFromFile(round2Images[4], img15.FullName);
            archive.CreateEntryFromFile(round2Images[5], img16.FullName);
            archive.CreateEntryFromFile(round2Images[6], img17.FullName);
            archive.CreateEntryFromFile(round2Images[7], img18.FullName);
            archive.CreateEntryFromFile(round2Images[8], img19.FullName);
            archive.CreateEntryFromFile(round2Images[9], img20.FullName);
        }
    }
}