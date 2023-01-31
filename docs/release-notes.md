# RSoft.Auth
User authentication service API. Generates a JWT token for authenticate user in your application.

##### Available Authentication types
- Internal users (own database)

### Release Notes

#### Version 1.0.0
- Internal user authentication
- Application-Client authentication.
- Application-Client manager (CRUD).
- User manager (CRUD).
- Roles manager (CRUD)

#### Version 1.0.1
- Upgrade RSoft.Framework and RSoft.Logs NuGet packages

#### Version 1.0.2
- Added globalization feature with en-US and pt-BR languages
- Upgrade RSoft.Framework to version 1.0.0-rc1.8 because supported langauge globalization

#### Version 1.0.3
- bug fixes

#### Version 1.0.4
- Upgrade from .NET Core 3.1 to .NET 5

#### Version 1.0.5
- Change package 'RSoft.Framework.Web' to 'RSoft.Lib.Web'
- Change package 'RSoft.Framework' to 'RSoft.Lib.Common' and 'RSoft.Lib.Design'

#### Version 1.0.6
- Update RSoft.Lib.* package versions to oficial released version

#### Version 1.0.7
- Update RSoft.Logs package

#### Version 1.0.12
- Update RSoft.Logs nuget package to version 1.2.5 (Fix top proccessor consumer)

#### Version 1.1.0
- Update RSoft.Logs nuget package to version 1.3.0 (Fix top proccessor consumer and infinite loop)

#### Version 1.2.0
- Added publish events for Create, Update and Delete users

#### Version 1.3.0
- Added Import/Export Application-Client

#### Version 1.3.1
- Bug fixes

#### Version 1.4.0
- Refactor App-Client-Headers (only ClientId is necessary for endpoints, except Authenticate)
- Policy UserAdminOrAuthorizedService added

#### Version 1.4.1
- Upgrade to .NET 6

#### Version 1.4.2
- Created work server for send mail (create/recovery credential)

#### Version 1.4.3
- Fix language resource in work service