# Client - Server Sample

This sample deploys a generic Weather Service as an Azure App Service behind an Azure API Management gateway. There is also a sample WPF application that can authenticate

## Prereqs

In order to automate the deployment of this solution using the scrips below, you must have Azure PowerShell and Azure AD PowerShell modules installed.

You can install the modules by following instructions here:

- [Azure PowerShell](https://docs.microsoft.com/en-us/powershell/azure/install-az-ps?view=azps-5.4.0)

## Setup

1. Create an API and client application registration in Azure Active Directory. Expose an API and Scope in the API app registration and update the client app registration to require access to the API/scope exposed by the API app registration.
1. Record the Application IDs of the API and client app registrations in Azure Active Directory.
1. Record the Tenant ID of the Azure Active Directory tenant.
1. Run the script `./scripts/New-MutualTlsCertificate.ps1` to generate a self-signed certificate to be used for authentication between Azure APIM and the App Service API.
1. Use the values from the steps above (including those output while executing `./scripts/New-MutualTlsCertificate.ps1`) to fill out the `./templates/parameters.dev.json` file.
1. Login to Azure PowerShell using: `Login-AzAccount -TenantId 00000000-0000-0000-0000-000000000000` where the GUID is your actual Azure AD tenant.
1. Deploy the `./templates/azuredeploy.json` file to Azure by executing: `New-AzResourceGroupDeployment -ResourceGroupName <resource group name> -TemplateFile ./templates/azuredeploy.json -TemplateParameterFile ./templates/parameters.dev.json`
1. Publish the API project to the API App Service created in Azure.
1. Update the variables in App.xaml.cs of the WpfClient application with the values created above.

## License

The MIT License (MIT)

Copyright © 2020 Ryan Graham

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.