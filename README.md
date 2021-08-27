<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/234338147/19.2.4%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T856361)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# How to use Report and Dashboard Server's export API from the ASP.NET Core MVC application

This example demonstrates how to use the [Report and Dashboard Server](https://docs.devexpress.com/ReportServer/12432/index)'s export API.


**Run the Example**

* Open the command line prompt and navigate to the example's root folder.
 
* In the console, generate user secrets for the Server account's username and password:

    ``dotnet user-secrets set "ReportServer:UserName" "Guest"``
    
    ``dotnet user-secrets set "ReportServer:UserPassword" ""``

* Run the command below to trust the **https** certificate for ASP.NET Core development:

    ``dotnet dev-certs https --trust``
    
* Type `dotnet run` to build and run the example application.

After the application is built, open your browser and go to _http://localhost:5000/_ or _https://localhost:5001/_ to see the result.

**Documentation**

  https://docs.devexpress.com/ReportServer/401403/configuration-and-api/http-api#api-list
