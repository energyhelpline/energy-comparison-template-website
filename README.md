# Energy Comparison Template Website

## Overview

The purpose of this project is give our API partners a starting point for integrating EnergyHelpline's API into their own solutions. We do this by providing you with a sample website that interacts with the API in our Staging environment to help create a switch.

This sample site has no styling, maintaining the focus on how to interact with our API.

## Intended Audience

This documentation is intended for the developer responsible for integrating our API into the partner's solution. It is assumed the developer has some familiarity with ASP.NET MVC and .NET Core.

## Prerequisites

* [.NET Core 2.0 SDK](https://www.microsoft.com/net/download/dotnet-core/2.0#sdk-2.0.0) installed to run the various commands below
* [Google Chrome](https://www.google.co.uk/chrome/) web browser and [Chromedriver](https://sites.google.com/a/chromium.org/chromedriver/downloads) to run acceptance tests

## Set Up

For security reasons, no client credentials are included in the repo. As such, to run this project, you'll need to update client key information. Actual credentials will be supplied by your account manager.

* Delete BareboneUi/appsettings.json
* Rename BareboneUi/appsettings.example.json file to BareboneUi/appsettings.json
* Open BareboneUi/appsettings.json and update the value for ApiKey, ApiSecretKey and ApiPartnerReference to that provided by your account manager and save.

## Building and testing the solution

Run this command in the project's root directory:

```
dotnet build
```

This should give you output similar to the following:

    Microsoft (R) Build Engine version 15.3.409.57025 for .NET Core
    Copyright (C) Microsoft Corporation. All rights reserved.

      BareboneUi -> C:\code\barebone-ui\BareboneUi\bin\Debug\netcoreapp2.0\BareboneUi.dll
      BareboneUi.Tests -> C:\code\barebone-ui\BareboneUi.Tests\bin\Debug\netcoreapp2.0\BareboneUi.Tests.dll
      BareboneUi.Acceptance.Tests -> C:\code\barebone-ui\BareboneUi.Acceptance.Tests\bin\Debug\netcoreapp2.0\BareboneUi.Acceptance.Tests.dll

    Build succeeded.
        0 Warning(s)
        0 Error(s)

Then, to run the tests:

```
./run_tests.sh
```

This should yield output similar to:

    Running all tests in C:\code\barebone-ui\BareboneUi.Tests\bin\Debug\netcoreapp2.0\BareboneUi.Tests.dll
    NUnit3TestExecutor converted 192 of 192 NUnit test cases
    NUnit Adapter 3.10.0.21: Test execution complete

    Total tests: 192. Passed: 192. Failed: 0. Skipped: 0.
    Test Run Successful.
    ...
    Running all tests in C:\code\barebone-ui\BareboneUi.Acceptance.Tests\bin\Debug\netcoreapp2.0\BareboneUi.Acceptance.Tests.dll
    NUnit3TestExecutor converted 17 of 17 NUnit test cases
    NUnit Adapter 3.10.0.21: Test execution complete

    Total tests: 17. Passed: 17. Failed: 0. Skipped: 0.
    Test Run Successful.

The acceptance tests run in Google Chrome and should take approximately two minutes.

## Running the Website

You can manually test the site by running the following:

```
dotnet BareboneUi/bin/Debug/netcoreapp2.0/BareboneUi.dll
```

This should give output similar to:

    Hosting environment: Development
    Content root path: C:\code\barebone-ui\BareboneUi
    Now listening on: http://localhost:5000
    Application started. Press Ctrl+C to shut down.

Navigating to http://localhost:5000 in your browser, you should see the Start Switch page prompting you to enter a postcode.
