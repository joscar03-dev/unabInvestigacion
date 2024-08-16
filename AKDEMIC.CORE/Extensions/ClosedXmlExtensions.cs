using System;
using ClosedXML.Excel;
using System.IO;
using AKDEMIC.CORE.Constants;

namespace AKDEMIC.CORE.Extensions
{
    public static class ClosedXmlExtensions
    {
        public static void AddHeaderToWorkSheet(this IXLWorksheet worksheet, string title, string logo, int tableLength = 5)
        {
            worksheet.Row(1).InsertRowsAbove(5);

            var mergeRangeColumn = 'F';

            if (tableLength != 5) mergeRangeColumn = NumberToLetter(tableLength);

            //if (!string.IsNullOrEmpty(logo))
            //{
            //    using (var memoryStream = new MemoryStream(File.ReadAllBytes(logo)))
            //    {
            //        worksheet.AddPicture(memoryStream).MoveTo(worksheet.Cell("A1")).WithSize(60, 60);
            //    }
            //}

            worksheet.Cell(2, 2).Value = GeneralConstants.GetInstitutionName().ToUpper();
            worksheet.Cell(2, 2).Style.Font.FontSize = 12;
            worksheet.Cell(2, 2).Style.Font.Bold = true;
            worksheet.Cell(2, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(2, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range($"B2:{mergeRangeColumn}2").Merge();

            worksheet.Cell(3, 2).Value = title;
            worksheet.Cell(3, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(3, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range($"B3:{mergeRangeColumn}3").Merge();
        }

        private static char NumberToLetter(int number)
        {
            switch (number)
            {
                case 1: return 'A';
                case 2: return 'B';
                case 3: return 'C';
                case 4: return 'D';
                case 5: return 'E';
                case 6: return 'F';
                case 7: return 'G';
                case 8: return 'H';
                case 9: return 'I';
                case 10: return 'J';
                case 11: return 'K';
                case 12: return 'L';
                case 13: return 'M';
                case 14: return 'N';
                case 15: return 'O';
                case 16: return 'P';
                case 17: return 'Q';
                case 18: return 'R';
                case 19: return 'S';
                case 20: return 'T';
                case 21: return 'U';
                case 22: return 'V';
                case 23: return 'W';
                case 24: return 'X';
                case 25: return 'Y';
                case 26: return 'Z';
                default:
                    return 'E';
            }
        }
    }
}
