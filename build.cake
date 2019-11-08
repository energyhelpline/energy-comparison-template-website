#load ".build\parameters.cake"
#load ".build\tasks.cake"

//////////////////////////////////////////////////////////////////////
// PARAMETERS
//////////////////////////////////////////////////////////////////////

const string solution = "BareboneUi.sln";
const string octoProject = "";
const string octoPublishProject = "";

isDotNetCore = true;

//////////////////////////////////////////////////////////////////////
// CUSTOM TASKS
// Add tasks as required, optional
//////////////////////////////////////////////////////////////////////

Task("Unit-Tests")
    .Does(()=> DotNetCoreTest("./BareboneUi.Tests"));

Task("Acceptance-Tests")
    .Does(()=> DotNetCoreTest("./BareboneUi.Acceptance.Tests"));

Task("Upgrade-Chromedriver")
    .Does(()=> {
        ChocolateyInstall("chromedriver");
        ChocolateyUpgrade("chromedriver");
    });

Task("Base-Build")
    .IsDependentOn("Info")
    .IsDependentOn("Get-Build-Number")
    .IsDependentOn("Upgrade-Chocolatey")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
// Change the default as required
// Optionally add more
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Base-Build")
    .IsDependentOn("Unit-Tests");

Task("Run-Acceptance-Tests")
    .IsDependentOn("Base-Build")
    .IsDependentOn("Upgrade-Chromedriver")
    .IsDependentOn("Acceptance-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
