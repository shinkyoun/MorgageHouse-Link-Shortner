# MorgageHouse-Link-Shortner

To run
1. Set up database
- Execute create-tables.sql under database folder
- Update UrlLinkDbConnection in appsettings.json with correct database connection string
- Or run by setting UrlLinkDbConnection in environment variables with correct database connection string
2. If SpaProxyServerUrl is changed or applicationUrl in launchSettings.json, please change port in UrlLinkShortnerService.GetFullPath accordingly


**NOTE on the 2nd** 
- The changes are only needed when the application is running with debugger attached
- The 2 inline port checking in UrlLinkShortnerService.GetFullPath need to get around angular routing conflicts with short url generated

