# csce361-attendancetracker

## Getting the project running

### Visual Studio project
The Visual Studio project was made with Windows 10, but should be compatible with 8.1, 8, and 7 as well.

The Visual Studio project for the web application is in the Code/visualstudio folder. I'm using Visual Studio 2013, you can use Visual Studio 2013, or anything newer. You can either use [Visual Studio Community](https://www.visualstudio.com/vs/community/), or use a better version from Dreamspark.

The database is running on Microsoft SQL Server Express 2014, you can use any version, as long as it's 2014 or newer. You can get [SQL Server 2016 Express](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express), [SQL Server 2016 Developer](https://www.microsoft.com/en-us/sql-server/sql-server-editions-developers), or SQL Server 2016 Enterprise from Dreamspark. You'll probably want [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) too. Make sure you give your user account permission to access SQL Server when installing.

Depending on how you installed SQL Server you may need to change the connection string within the project from LOCALHOST/SQLEXPRESS to something like LOCALHOST or LOCALHOST/NAME. You can easily do this with a whole project find/replace in Visual Studio.

A database dump can be found at code/visualstudio/sql/AttendanceTracker.bak.

You will need to restore the database dump. The easiest way to do this is place the .bak file in the Backups folder within your SQL Server installation folder. You can then use the restore database functionality built into SQL Server Management Studio. There are many other ways to do this.

You will need to have IIS installed (along with most of its features, you can check all of the boxes excluding things like FTP servers) on your computer to work with the project (Turn Windows features on or off...). You should also enable the .NET Frameworks on your machines. Make sure you run Visual Studio as an administrator so that it can work with IIS.

You will also need to set the IIS application pool to use your local Windows account. This [guide](https://www.iis.net/learn/manage/configuring-security/application-pool-identities) on Microsoft's website has some info on changing the App Pool identity. Instead of selecting from the dropdown use the Local Account option beneath and enter your login info for your Windows account. This is used so that IIS has permission to access the SQL Server. Make sure you are setting the user for the correct app pool (the one the visual studio project uses), if you are unsure you can set them all to use your user account.

You will probably need to set the permissions of this project's folder to allow Everyone access. Here is a [guide](http://www.softwareok.com/?seite=faq-Windows-7&faq=105) with some more information on that. 

The project uses Nuget packages. Generally Visual Studio downloads and installs everything automatically when the project is built. If you have trouble then you can download the Nuget [executable](https://dist.nuget.org/index.html) and run the command to restore packages when the project directory.  

If you set everything up correctly you can build the project, and then open the application in your browser: [http://localhost/AttendanceTracker/](http://localhost/AttendanceTracker). The first time it loads it may take longer than usual.

### Bluetooth hardware

The Bluetooth hardware is a lot easier to setup. It requires a Debian based computer with support for Bluetooth. We used a Raspberry Pi with a USB Bluetooth dongle. 

To install the software dependencies run install.sh which is located in code/node/rpi. 

To run the program run main.py in code/node/rpi with Python 2.7. 

## Azure instance

I setup an Azure instance of the project that pulls changes from this repository's master branch every hour. You can find it at [http://cse-iis.quade.co/AttendanceTracker/](http://cse-iis.quade.co/AttendanceTracker/). It will go offline at some point after the final grades for the class have been posted.

## Code structure

### Visual Studio project

The Visual Studio project contains two applications. A web application called AttendanceTracker, and a command line service called SchedulerService. 

#### AttendanceTracker

The web application follows the MVC design pattern. This is the layout of the folders:

* App_Data is empty. It was created by default by Visual Studio. 
* App_Start is for some of the general initialization code that is included by default. Things like default routes, and bundles of Javascript/CSS files are setup here. 
* Content is for some of the application's CSS/Javascript files. Subfolders exist for some of the Controllers and relate to the View folders with the same names. A global Site.css is here as well.
* Controllers contains all of the controllers. The default controllers for the account management, and the home page are in the folder root, and then there are subfolders for the rest. The shared controller is used for the autocomplete input fields in the views. THe controllers are used for page routes, and their job is to collect data within the models and return it through the views. This is also where certain pages are configured to only work for certain user groups. 
* Fonts contains some fonts. 
* Models contains all of the models. There are some files in the root for the default models for authentication and other things. In the root is also a UserRolesModel which is used in the Controllers to know if a user is a Student, Teacher, or Admin. There are also subfolders for each of the controllers that are also in subfolders. This is where all of the data is collected and managed. 
* Scripts contains all of the Javascript/CSS libraries that are used for the views. There is also a shared global Site.css.
* Views contains all of the HTML files. There are folders for all of the controllers and cshtml files for many of the controller methods. Pages are based off of a default layout page at Shared/_Layout.cshtml.
* AttendanceDatabase.edmx contains database model information. 

#### SchedulerService

This is a very simple application. There is the same AttendanceTracker.edmx file containing database model information.

The Program.cs file is setup as a Windows Service that just continually loops through until it has something to do. The loop functionality isn't used for much though. Instead there is a websocket server that handles communication with the Bluetooth hardware devices. 

### Bluetooth hardware

The main.py file initializes the application and its two threads.

The first thread is for taking attendance. It contiually waits for students to appear in the student dictionary. If there are students in the dictionary it will attempt to ping the Bluetooth MAC address, keeping track of who is present and who is not. Once all of the students have been tested the thread reports the results back to the SchedulerService server.

The second thread is for keeping up to date. It will pull new changes from this git repository, as well as pull down updated student lists from the SchedulerService server. It continually runs.

The network.py file is used to define objects and methods reladed to the networking functions used in the main.py file. The ota.py file is used to pull changes from the git repository, keeping the node up to date. 

There is also a folder at code/node/esp containing code for the ESP32 that performs the same functions as the Raspberry Pi code. It isn't fully completed, but has a similar structure to the Python code.
