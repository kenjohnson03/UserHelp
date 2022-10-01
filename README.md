# UserHelp

This solution was designed to assist administrators hoping to reduce users reliance on Help Desk elevations and increase self-service.

To use this solution, download and open with Visual Studio. A code-signing certificate will be required to sign the output MSIX application to deploy to your systems.

The default location for the UserHelp JSON file is under %PROGRAMDATA%\UserHelp\UserHelp.json. This folder path, by default, only gives standard users rights to read the contents of the file. Therefore a change agent must be used with system or administrator rights to update this file location.

In addition to standard commands users may need to use to resolve standard issues, this tool is intended to be used alongside PowerShell JEA to give users an easy way to resolve issues that previously required escalation. The JEA configuration should also be stored in a read only location that standard users cannot modify.

If this was helpful and you would like to contribute please use this link:
[Donate](https://www.paypal.com/paypalme/jahobeecoding)
