using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using Packliste.Assets;
using Packliste.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Packliste.Helper
{
    public class PdfCreator
    {
        private Journey _journey;
        public PdfCreator(Journey journey)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF(*.pdf)|*pdf";
            saveFileDialog.DefaultExt = "pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                _journey = journey;
                Save(saveFileDialog.FileName);
            }
        }

        public void Save(string filePath)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            Font times = new Font(bfTimes, 16, Font.BOLD, Color.BLACK);

            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

            doc.Open();

            //General information
            PdfPTable basicData = new PdfPTable(2);
            basicData.HorizontalAlignment = Element.ALIGN_LEFT;
            basicData.DefaultCell.Border = 3;
            basicData.WidthPercentage = 50;
            basicData.AddCell("Reiseziel:");
            basicData.AddCell(_journey.Destination);
            basicData.AddCell("Von");
            basicData.AddCell(_journey.StartDate.ToShortDateString());
            basicData.AddCell("Bis:");
            basicData.AddCell(_journey.EndDate.ToShortDateString());
            basicData.AddCell("Mitreisende:");
            basicData.AddCell(string.Join(",", _journey.Travelers.Select(x => x.Person.Name)));
            doc.Add(basicData);
            doc.Add(new iTextSharp.text.Paragraph("\n"));

            // Packing List
            WeightConverter weightConverter = new WeightConverter();
            foreach (Traveler traveler in _journey.Travelers)
            {
                string totalWeight = weightConverter.Convert(traveler.TotalWeight, null, null, CultureInfo.CurrentCulture).ToString();
                doc.Add(new iTextSharp.text.Paragraph(traveler.Person.Name + " - " + totalWeight, times));
                doc.Add(new iTextSharp.text.Paragraph("\n"));
                PdfPTable table = createItemTable(traveler);
                doc.Add(table);
            }

            doc.Close();

        }

        private PdfPTable createItemTable(Traveler traveler)
        {
            float[] relativeWidths = { 0.15f, 0.75F, 0.1F };
            PdfPTable itemsTable = new PdfPTable(relativeWidths);
            itemsTable.HorizontalAlignment = Element.ALIGN_LEFT;
            itemsTable.DefaultCell.Border = 3;
            itemsTable.WidthPercentage = 80;
            itemsTable.AddCell("Gepackt");
            itemsTable.AddCell("Gegenstand");
            itemsTable.AddCell("Menge");
            itemsTable.HeaderRows = 1;
            foreach (var itemSet in traveler.itemSets)
            {
                itemsTable.AddCell("");
                itemsTable.AddCell(itemSet.Item.Name);
                itemsTable.AddCell(itemSet.Count.ToString());
            }
            return itemsTable;

        }
    }
}
