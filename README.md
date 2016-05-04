# AppLogger

Log how long you have been working on documents.

## General info

Simple C# forms application to log how long certain windows were open.

Binaries are included in `Debug/Release/applogger.exe`

Optional command line argument: bg will background the application on startup and automatically start logging (run: `applogger.exe bg`).

Some todo items can be found in `applogger/applogger.cs` (I guess these are to be converted into Github issues).

## Code structure

This is a standard Visual Studio 2012 project. The main file is `Program.cs` which bootstraps the main form `applogger.cs`. The other `.cs` files are all called directly from the main form.

