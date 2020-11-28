using IronPdf;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace PDF_Deluxe_Editor
{
    public partial class Main : Window
    {
        private OpenFileDialog fileDialog = new OpenFileDialog();
        private SaveFileDialog sFileDialog = new SaveFileDialog();
        private CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog();

        private string imageFilter = "Image files | *.bmp; *.jpg; *.gif; *.png; *.tif";
        private string pdfFilter = "PDF files | *.pdf";
        public Main()
        {
            InitializeComponent();
        }

        private void convertPdfIntoImages(object sender, RoutedEventArgs e)
        {
            string urlFile = openFileDialog(pdfFilter);
            if (urlFile != null && !urlFile.Equals(""))
            {
                var pdf = PdfDocument.FromFile(Path.GetFullPath(urlFile));
                pdf.RasterizeToImageFiles(selectDestinationFolder() + "img_*.png");
                successfull();
            }
            else
            {
                fileNotSelected();
            }
        }

        private void createPdfWithImages(object sender, RoutedEventArgs e)
        {
            string[] urlFiles = openFilesDialog(imageFilter);
            if (urlFiles.Length > 0)
            {
                List<string> filesPath = new List<string>();
                foreach (string urlFile in urlFiles)
                {
                    filesPath.Add(Path.GetFullPath(urlFile));
                }
                ImageToPdfConverter.ImageToPdf(filesPath.ToArray()).SaveAs(saveFileDialog(pdfFilter));
                successfull();
            }
            else
            {
                fileNotSelected();
            }
        }

        private void mergePdf(object sender, RoutedEventArgs e)
        {
            string[] urlFiles = openFilesDialog(pdfFilter);
            var PDFs = new List<PdfDocument>();
            if (urlFiles.Length > 0)
            {
                for (int num = 0; num < urlFiles.Length; num++)
                {
                    PDFs.Add(PdfDocument.FromFile(Path.GetFullPath(urlFiles[num])));
                }
                PdfDocument PDF = PdfDocument.Merge(PDFs);
                PDF.SaveAs(saveFileDialog(pdfFilter));
                successfull();
            }
            else
            {
                fileNotSelected();
            }
        }

        protected string[] openFilesDialog(string filter)
        {
            fileDialog.Reset();
            if (filter != null)
            {
                fileDialog.Filter = filter;
            }
            fileDialog.Multiselect = true;
            fileDialog.ShowDialog();
            return fileDialog.FileNames;
        }

        protected string openFileDialog(string filter)
        {
            fileDialog.Reset();
            if (filter != null)
            {
                fileDialog.Filter = filter;
            }
            fileDialog.Multiselect = false;
            fileDialog.ShowDialog();
            return fileDialog.FileName;
        }

        protected string saveFileDialog(string filter)
        {
            sFileDialog.Reset();
            if (filter != null)
            {
                sFileDialog.Filter = filter;
            }
            sFileDialog.ShowDialog();
            return sFileDialog.FileName;
        }

        protected string selectDestinationFolder()
        {
            commonOpenFileDialog.ResetUserSelections();
            commonOpenFileDialog.IsFolderPicker = true;
            commonOpenFileDialog.ShowDialog();
            return commonOpenFileDialog.FileName;
        }

        protected void fileNotSelected()
        {
            MessageBox.Show("No File Selected", "Warning!");
        }

        protected void successfull()
        {
            MessageBox.Show("Operation Completed");
        }
    }
}
