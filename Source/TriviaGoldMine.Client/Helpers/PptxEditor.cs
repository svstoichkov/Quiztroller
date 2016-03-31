namespace Quiztroller.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Web;

    using Models;

    public static class PptxEditor
    {
        public static string Edit(List<Question> questions, string img1Path, string img2Path, string img3Path, string videoPath, string pptxPath)
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), "Quiztroller");
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
                ReplaceMedia(img1Path, img2Path, img3Path, videoPath, archive);
                ReplaceQuestions(questions, archive, tempFolder);
            }

            return newFile;
        }

        private static void ReplaceQuestions(List<Question> questions, ZipArchive archive, string tempFolder)
        {
            var slides = archive.Entries.Where(x => x.FullName.StartsWith("ppt/slides/") && x.FullName.EndsWith(".xml"));
            slides = slides.OrderBy(x => int.Parse(x.FullName.Substring(x.FullName.LastIndexOf('e') + 1, x.FullName.IndexOf('.') - x.FullName.LastIndexOf('e') - 1))).ToList();

            var currentQuestionIndex = 0;
            foreach (var slide in slides)
            {
                var slidePath = Path.Combine(tempFolder, Path.GetFileName(slide.FullName));
                slide.ExtractToFile(slidePath);

                var content = File.ReadAllText(slidePath);

                if (content.Contains("%"))
                {
                    content = content.Replace("%QSTN%", HttpUtility.HtmlEncode(questions[currentQuestionIndex].MainQuestion ?? " "));
                    content = content.Replace("%PT%", HttpUtility.HtmlEncode(questions[currentQuestionIndex].Points.ToString()));
                    content = content.Replace("%ANSWR%", HttpUtility.HtmlEncode(questions[currentQuestionIndex].Answer ?? " "));

                    if (slide.FullName.Contains("27"))
                    {
                        var answers = questions[currentQuestionIndex].Answer.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < answers.Length; i++)
                        {
                            content = content.Replace($"%ANSWR{i + 1}%", HttpUtility.HtmlEncode(answers[i]));
                        }
                        for (int i = 1; i <= 10; i++)
                        {
                            content = content.Replace($"%ANSWR{i}%", " ");
                        }
                    }
                    else if (slide.FullName.Contains("28"))
                    {
                        for (int i = 24; i < 29; i++)
                        {
                            content = content.Replace($"%QSTN{i - 23}%", HttpUtility.HtmlEncode(questions[i].MainQuestion ?? " "));
                        }

                        currentQuestionIndex = 28;
                    }


                    File.WriteAllText(slidePath, content);

                    slide.Delete();
                    archive.CreateEntryFromFile(slidePath, slide.FullName);
                    currentQuestionIndex++;
                }
            }
        }

        private static void ReplaceMedia(string img1Path, string img2Path, string img3Path, string videoPath, ZipArchive archive)
        {
            var img1 = archive.Entries.FirstOrDefault(x => x.Length == 2585);
            var img2 = archive.Entries.FirstOrDefault(x => x.Length == 2583);
            var img3 = archive.Entries.FirstOrDefault(x => x.Length == 2930);
            var video = archive.Entries.FirstOrDefault(x => x.Length == 7352);

            img1.Delete();
            img2.Delete();
            img3.Delete();
            video.Delete();

            archive.CreateEntryFromFile(img1Path, img1.FullName);
            archive.CreateEntryFromFile(img2Path, img2.FullName);
            archive.CreateEntryFromFile(img3Path, img3.FullName);
            archive.CreateEntryFromFile(videoPath, video.FullName);
        }
    }
}
