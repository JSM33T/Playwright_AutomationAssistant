using Microsoft.Playwright;
using System.Data;

namespace PWBAssiatant.Assistants
{
    public class DataScrapeAssistant
    {
        public static async Task<DataTable> ScrapeTable(IPage page, string tableSelector)
        {
            DataTable dataTable = new DataTable();

            // Find the table element based on the provided selector
            var table = await page.QuerySelectorAsync(tableSelector);

            if (table != null)
            {
                // Get the table headers (assuming they are in the thead section)
                var headerCells = await table.QuerySelectorAllAsync("thead th");
                foreach (var headerCell in headerCells)
                {
                    dataTable.Columns.Add(headerCell.InnerTextAsync().GetAwaiter().GetResult());
                }

                // Drill down into the table rows to collect data
                var rows = await table.QuerySelectorAllAsync("tbody tr");

                foreach (var row in rows)
                {
                    var dataRow = dataTable.NewRow();
                    var cells = await row.QuerySelectorAllAsync("td");

                    foreach (var cell in cells)
                    {
                        var cellText = await cell.InnerTextAsync();
                        dataRow[dataTable.Columns.IndexOf(cellText)] = cellText;
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }
        public static async Task ScrapeTable(IPage page, string TableSelector, string HeadSelector, string BodySelector)
        {
            //await page.GotoAsync(url);
        }

        public static async Task ScrapeTable(IPage page, string TableSelector, string BodySelector)
        {
            //await page.GotoAsync(url);
        }


    }
}
