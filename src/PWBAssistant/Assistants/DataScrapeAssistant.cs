using Microsoft.Playwright;
using System.Data;

namespace PWBAssiatant.Assistants
{
    public class DataScrapeAssistant
    {
        public static async Task<DataTable> ScrapeTable(IPage page, string tableSelector)
        {
            DataTable dataTable = new();

            // Find the table element based on the provided selector
            var table = await page.QuerySelectorAsync(tableSelector);
            if (table != null)
            {
                // Get the table headers (if present in the thead)
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
        public static async Task<DataTable> ScrapeTable(IPage page, string HeadSelector, string BodySelector)
        {
            var dataTable = new DataTable();

            // Locate the table header (thead) and body (tbody) elements
            var thead = await page.QuerySelectorAsync(HeadSelector) ?? throw new Exception("Table not detected with the given selector string");
            var tbody = await page.QuerySelectorAsync(BodySelector) ?? throw new Exception("Table body not detected with the given selector string"); ;

            // Extract column headers
            var columnHeaders = await thead.EvaluateAsync<string[]>("() => Array.from(document.querySelectorAll('th')).map(e => e.innerText)");

            // Add columns to the DataTable
            foreach (var header in columnHeaders)
            {
                dataTable.Columns.Add(header);
            }
            // Iterate through each row (tr) in the tbody
            var rows = await tbody.QuerySelectorAllAsync("tr");
            foreach (var row in rows)
            {
                var dataRow = dataTable.NewRow();

                // Extract data from each row
                var cells = await row.QuerySelectorAllAsync("td");

                // Populate the DataRow with cell values
                for (int i = 0; i < cells.Count; i++)
                {
                    dataRow[i] = await cells[i].InnerTextAsync();
                }

                // Add the populated DataRow to the DataTable
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        public static async Task<DataTable> ScrapeTable(IPage page, List<string> CustomHeaders, string BodySelector)
        {
            var dataTable = new DataTable();

            // Locate the table body (tbody) element
            var tbodyHandle = await page.QuerySelectorAsync(BodySelector) ?? throw new Exception("Invalid body selector");

            if (CustomHeaders == null || CustomHeaders.Count == 0)
            {
                // Extract column headers from the first row of the table body
                var firstRow = await tbodyHandle.QuerySelectorAsync("tr") ?? throw new Exception("No headers passed");

                var firstRowCells = await firstRow.QuerySelectorAllAsync("td");

                CustomHeaders = [];
                foreach (var cell in firstRowCells)
                {
                    CustomHeaders.Add($"Column {CustomHeaders.Count + 1}");
                }
            }

            // Add columns to the DataTable based on the manual column headers provided
            for (int i = 0; i < CustomHeaders.Count; i++)
            {
                dataTable.Columns.Add(i < CustomHeaders.Count ? CustomHeaders[i] : $"<empty>");
            }

            // Iterate through each row (tr) in the tbody
            var rows = await tbodyHandle.QuerySelectorAllAsync("tr");
            foreach (var rowHandle in rows)
            {
                var dataRow = dataTable.NewRow();

                // Extract data from each row
                var cells = await rowHandle.QuerySelectorAllAsync("td");

                var cellTextTasks = cells.Select(cell => cell.InnerTextAsync());
                var cellTexts = await Task.WhenAll(cellTextTasks);

                // Check if the number of cell texts exceeds the number of columns
                if (cellTexts.Length > dataTable.Columns.Count)
                {
                    int columnsToAdd = cellTexts.Length - dataTable.Columns.Count;
                    for (int i = 0; i < columnsToAdd; i++)
                    {
                        dataTable.Columns.Add($"<empty_{i + 1}>");
                    }
                }

                for (int i = 0; i < cellTexts.Length; i++)
                {
                    if (i < dataTable.Columns.Count)
                    {
                        dataRow[i] = cellTexts[i];
                    }
                    else
                    {
                        dataRow[dataTable.Columns.Count - 1] = cellTexts[i];
                    }
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }
}
