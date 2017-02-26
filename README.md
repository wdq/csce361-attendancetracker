# csce361-attendancetracker

## Getting the project running

The Visual Studio project for the web application is in the Code/visualstudio folder. I'm using Visual Studio 2013, you can use Visual Studio 2013, or anything newer. You can either use [Visual Studio Community](https://www.visualstudio.com/vs/community/), or use a better version from Dreamspark.

The database is running on Microsoft SQL Server Express 2014, you can use any version, as long as it's 2014 or newer. You can get [SQL Server 2016 Express](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express), [SQL Server 2016 Developer](https://www.microsoft.com/en-us/sql-server/sql-server-editions-developers), or SQL Server 2016 Enterprise from Dreamspark. You'll probably want [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) too.

The SQL folder in the visualstudio folder has scripts that you can use to create a blank database for the project.

You will need to have IIS installed (along with most of its features) on your computer to work with the project (Turn Windows features on or off...). You should also enable the .NET Frameworks on your machines. Make sure you run Visual Studio as an administrator so that it can work with IIS.

If you set everything up correctly you can build the project, and then open the application in your browser: [http://localhost/AttendanceTracker/](http://localhost/AttendanceTracker). The first time it loads it may take longer than usual.

## Azure instance

I setup an Azure instance of the project that pulls changes from this repository's master branch every hour. You can find it at [http://13.65.210.250/AttendanceTracker/](http://13.65.210.250/AttendanceTracker/).
