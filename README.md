# RSoft.Auth

User authentication service API. Generates a JWT token for authenticate user in your application.

##### Available Authentication types
- Internal users (internal database)

### Requeriments
- `MySql Server` 5.7.22 or higher for the application to create and use its own database. See https://dev.mysql.com/.
- `Elastic Server` for the application to send log records. See https://github.com/rodriguesrm/rsoft-logs.
- `RSoft-Mail API Service` for sending credential creation and retrieval emails. See https://github.com/rodriguesrm/rsoft-mail-service.

### NuGet Package Dependencies
- `RSoft.Framework (>= 1.0.0-rc1.8)`. See https://www.nuget.org/packages/RSoft.Framework.
- `RSoft.Framework.Web (>= 1.0.0-rc1.3)`. See https://www.nuget.org/packages/RSoft.Framework.Web.
- `RSoft.Logs (>= 1.0.0-rc1.7)`. See https://www.nuget.org/packages/RSoft.Logs.

It was presented here the dependencies related to other projects also available in this repository. To view the total list of project dependencies, I suggest you open the .net core solution and explore the projects.

### Installaction
The project aimed to publish the service in a container environment, to generate the image follow the following procedures.

1. Make sure you have the docker-engine and docker-compose installed (version 19.03.12 or higher).
2. Open a `bash` console window.
2. Checkout the repository in a folder of your choice `git clone <url_repository> <folder_destination>`.
2. Go to the `<your_folder>/src` folder in the repository.
3. Run the `docker-compose build` command.
4. Create a container using the "docker command" or "docker-compose" according to your preferences. See the required environment variables and settings in the "Settings and Environment Variables" section below this document.

##### File 'docker-compose' example
&nbsp;
Use this file as an example to create your own ```docker-compose.yaml``` file to upload this service's container.
See the "requirements" and "configuration and environment variables" sections for more details.

```
version: '2'

services:

  rsoft-auth:
    container_name: rsoft-auth-service
    build:
      context: .
      dockerfile: RSoft.Auth.Web.Api/Dockerfile
    image: rsoft/auth-service:1.0.4
    ports:
      - "8080:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - Logging:Seq:Uri=http://localhost:5431
      - ConnectionStrings:DbServer=Server=localhost;Port=3306;database=rsoft_auth;uid=admin;pwd=admin@2020;
      - Application:RSoftApis:Uri=http://localhost:8081
    network_mode: host
```

### Settings and Environment Variables

##### Environment Variables
- `ASPNETCORE_ENVIRONMENT` must be assigned correctly to inform the service execution environment. It must be completed according to the standards established by Microsoft for this variable (Development, Staging or Production).
##### Configurations
&nbsp;
The application uses the standard `appsettings.json` and its variants recommended by Microsoft. Below is a preview of the file for the production environment.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    },
    "Seq": {
      "Enable": true,
      "Uri": "http://localhost:5431",
      "IgnoreCategories": [
        "Microsoft.Hosting.Lifetime"
      ]
    },
    "RequestResponseMiddleware": {
      "LogRequest": true,
      "LogResponse": true,
      "SecurityActions": [
        {
          "Method": "POST",
          "Path": "/api/v1.0/auth"
        },
        {
          "Method": "POST",
          "Path": "/api/v1.0/Credential/first"
        },
        {
          "Method": "PUT",
          "Path": "/api/v1.0/Credential/recovery"
        }
      ],
      "IgnoreActions": [
        {
          "Method": "GET",
          "Path": "/favicon.ico"
        },
        {
          "Method": "GET",
          "Path": "/swagger/v1.0/swagger.json"
        },
        {
          "Method": "GET",
          "Path": "/index.html"
        },
        {
          "Method": "GET",
          "Path": "/favicon-32x32.png"
        },
        {
          "Method": "GET",
          "Path": "/hc"
        },
        {
          "Method": "GET",
          "Path": "/"
        },
        {
          "Method": "GET",
          "Path": "/swagger-ui.css"
        },
        {
          "Method": "GET",
          "Path": "/swagger-ui-bundle.js"
        },
        {
          "Method": "GET",
          "Path": "/swagger-ui-standalone-preset.js"
        }
      ]
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DbServer": "Server=localhost;Port=3306;database=rsoft_auth;uid=root;pwd=admin;"
  },
  "Scope": {
    "Key": "92a4ce2a-26ed-4ae2-9813-b7e5e6a8678d",
    "Access": "8f7318ee-4027-4cde-a6d3-529e6382f532"
  },
  "Jwt": {
    "Issuer": "RSoft.Auth",
    "Hash": "QayqWdfq7YrW-xACEzsD4gp49CV8FWM7T",
    "Audience": "http://localhost:8080",
    "TimeLife": 240
  },
  "Swagger": {
    "Title": "RSoft Authentication API",
    "Description": "API for managing authentication and authorization roles.",
    "Contact": "Rodrigo Rodrigues",
    "Uri": "https://github.com/rodriguesrm",
    "EnableTryOut": false,
    "EnableJwtTokenAuthentication": false
  },
  "Application": {
    "Security": {
      "Secret": "2S02SuCkzNudEtIRbAtkKCQLrrLv6Zbe",
      "Lockout": {
        "Times": 3,
        "Minutes": 5
      }
    },
    "Credential": {
      "Token": {
        "TimeLife":  30
      }
    },
    "RSoftApis": {
      "Uri": "http://localhost:8081",
      "MailService": "/api/v1.0/Mail"
    },
    "Culture": {
      "SupportedLanguage": [
        "en-US",
        "pt-BR"
      ],
      "DefaultLanguage": "en-US"
    }
  }
}

```

* `Logging` See microsoft https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1 for defaults parameters and https://github.com/rodriguesrm/rsoft-logs for RSoft.Logs parameters.
* `ConnectionStrings` needs a `DbServer` parameter to connect MySqlDatabase like in example.
* `Scope` is a service identification section in the RSoft ecosystem. This key is standard and should not be changed as it will influence the communication between the systems and services of the RSoft platform.
* `Jwt` is the section where token reading and generation parameters must be informed, in addition to the lifetime. For more details, see Jwt's documentation at https://jwt.io/introduction/.
* `Swagger` is the section where the behavior definition information for the swagger-ui user interface will be placed. For more information see the `RSoft.Framework.Web` repository at https://github.com/rodriguesrm/rsoft-framework-web.
* `Application` is the section of settings related to the Authentication Service API, of which:
* `Application:Security: Secret` is the section for entering a 32-character alphanumeric key for composing each user's individual password. The purpose of this key is to combat the use of brute force to break access to the MD5 key from the hash recorded in the database. It is highly recommended that you change this value to a key specific to your environment, since the keys contained in this repository are exposed to the public.
* `Application:Security:Lockout` is the section for defining temporary user lockout options when they enter their password for a certain number of times and the time, in minutes, that it will remain locked.
* `Application:Credential` is the section for defining the lifetime of the token for creating and retrieving access credentials, either in the first access process or in password recovery in case of forgetfulness.
* `RSoftApis` is the section of the urls to access the services of the RSoft ecosystem that this application uses. See the "requirements" section of this document.
* `Culture` is the section for configuring supported languages and the default application language.

### API Documentation
This API service has all the documentation made through swagger-doc that can be accessed through the installation url.

### TODO's

##### Environment
- Docker-compose with all requirements/dependencies

###### Backlog Authentication types
- LDAP
- 2FA
- Google
- Facebook
- Github
- Linkedin

### License
MIT
