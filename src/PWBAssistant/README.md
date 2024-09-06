# PWBAssistant - PlayWright Bot assistant(0.3.0 Prerelease)

## Introduction

### Automation assist while scraping data out of a table or a list element

## Configuration

Works by default keeping the page object that it itself initialized.
We can receive the page using the inbuilt method ```GetPageObject()```

## Error Handling

Hnadles error by storing screenshots in the configured path. Default status is Off

## Methods

### GlobalContaxt

- Initialize()
- Initialize(bool useHeadless,string executablePath, string screenshotPath)
- GetPageObject()
- CleanUp()

### DataExtraction

- ScrapeTable(IPage page, string tableSelector)
- ScrapeTable(IPage page, string HeadSelector, string BodySelector)
