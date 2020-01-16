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

  Link to export api documentation here