using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Aspose.Pdf;

namespace PDFtoHTMLAndView
{
    public class PDFtoHTMLController : ApiController
    {
        [HttpPost]
        public KeyValuePair<bool, string> ConvertPDFtoHTMLAndView()
        {
            try
            {
                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    // Get the uploaded image from the Files collection
                    var httpPostedFile = HttpContext.Current.Request.Files["UploadedPDF"];

                    if (httpPostedFile != null)
                    {
                        // Validate the uploaded file

                        if (httpPostedFile.FileName.ToLower().Contains(".pdf"))
                        {
                            // vaerify folder path is valid
                            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/UploadedFiles")))
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadedFiles"));
                            }
                            // Get the complete file path
                            var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedFiles"), httpPostedFile.FileName);

                            // Save the uploaded file to "UploadedFiles" folder
                            httpPostedFile.SaveAs(fileSavePath);

                            // Load source PDF file
                            Document doc = new Document(fileSavePath);

                            // Instantiate HTML Save options object
                            HtmlSaveOptions newOptions = new HtmlSaveOptions();

                            // Enable option to embed all resources inside the HTML
                            newOptions.PartsEmbeddingMode = HtmlSaveOptions.PartsEmbeddingModes.EmbedAllIntoHtml;

                            // This is just optimization for IE and can be omitted 
                            newOptions.LettersPositioningMethod = HtmlSaveOptions.LettersPositioningMethods.UseEmUnitsAndCompensationOfRoundingErrorsInCss;
                            newOptions.RasterImagesSavingMode = HtmlSaveOptions.RasterImagesSavingModes.AsEmbeddedPartsOfPngPageBackground;
                            newOptions.FontSavingMode = HtmlSaveOptions.FontSavingModes.SaveInAllFormats;
                            // Output file path 
                            Guid newFileTempname = new Guid();
                            string outHtmlFile = HttpContext.Current.Server.MapPath("~/UploadedFiles") + "/" + newFileTempname.ToString() + ".html";

                            doc.Save(outHtmlFile, newOptions);

                            System.Text.StringBuilder strbuld = new System.Text.StringBuilder();
                            using (System.IO.StreamReader reader = new System.IO.StreamReader(outHtmlFile))
                            {
                                strbuld.Append(reader.ReadToEnd());
                            }

                            // Delete uploaded and created temp files
                            File.Delete(fileSavePath);
                            File.Delete(outHtmlFile);

                            return new KeyValuePair<bool, string>(true, strbuld.ToString());
                        }
                        else
                        {
                            return new KeyValuePair<bool, string>(true, "Invalid File, Please select valid PDF file.");
                        }
                    }

                    return new KeyValuePair<bool, string>(true, "Could not get the uploaded file.");
                }

                return new KeyValuePair<bool, string>(true, "No file found to upload, please select PDF file.");
            }
            catch (Exception ex)
            {
                return new KeyValuePair<bool, string>(false, "An error occurred while uploading the file. Error Message: " + ex.Message);
            }
        }
    }
}