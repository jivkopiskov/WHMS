namespace WHMS.Common
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.IO;
    using System.Linq;

    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    public static class ExcelHelperClass
    {
        public static bool TryValidate(object obj, out ICollection<ValidationResult> results)
        {
            var context = new ValidationContext(obj, serviceProvider: null, items: null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(
                obj,
                context,
                results,
                validateAllProperties: true);
        }

        public static DataTable GetDataTableFromExcel(Stream stream)
        {
            ISheet sheet;
            stream.Seek(0, SeekOrigin.Begin);
            XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
            sheet = hssfwb.GetSheetAt(0);

            var dataTable = new DataTable(sheet.SheetName);

            // write the header row
            var headerRow = sheet.GetRow(0);
            foreach (var headerCell in headerRow)
            {
                dataTable.Columns.Add(headerCell.ToString());
            }

            // write the rest
            for (int i = 1; i < sheet.PhysicalNumberOfRows; i++)
            {
                var sheetRow = sheet.GetRow(i);
                var dataRow = dataTable.NewRow();
                dataRow.ItemArray = dataTable.Columns
                    .Cast<DataColumn>()
                    .Select(c => sheetRow.GetCell(c.Ordinal, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString())
                    .ToArray();
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }
}
