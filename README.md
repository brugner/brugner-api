# Introduction
Web API for a simple blog application developed with .NET 6 and an Azure SQL Database.

# Build and Run
1. git clone https://github.com/brugner/brugner-api
2. Open the solution with Visual Studio
3. Create an appsettings.Development.json based on appsettings.json
4. Update the JWT and Connection String settings
5. Run the project using either the Brugner.API profile or the Docker profile
6. Use swagger or the Postman collection to test the API

# ~~Manually push image to registry~~
~~Run these commands where the dockerfile is located:~~
1. ~~docker build -t brugnerapi .~~
2. ~~docker tag brugnerapi brugner.azurecr.io/api~~
3. ~~docker push brugner.azurecr.io/api~~