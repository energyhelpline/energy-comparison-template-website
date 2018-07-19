function GetNextTag($currentTag)
{
    $lastFullStopLocation = $currentTag.LastIndexOf(".")
    $textBeforeLastFullStop = $currentTag.Substring(0, $lastFullStopLocation)
    $buildNumber = [int]::Parse($currentTag.Substring($lastFullStopLocation + 1))
    $incrementedBuildNumber = $buildNumber + 1
    Write-Host "`$textBeforeLastFullStop is $textBeforeLastFullStop"
    Write-Host "`$buildNumber is $buildNumber"
    return "$textBeforeLastFullStop.$incrementedBuildNumber"
}

function ApplyPatch($tag, $patchFileName)
{
    git apply --whitespace=fix $patchFileName
    if( -not $?)
    {
        Write-Host "There was an error when applying patch."
        throw "Error applying patch."
    }

    git add .
    git reset .\BareboneUi\appsettings.json
    git commit -m "Release version $tag"
    git checkout .
    git tag -a $tag -m "Release version $tag"
}

function ReclonePublicRepo{
    if (Test-Path -Path "energy-comparison-template-website")
    {
        Remove-Item .\energy-comparison-template-website -Recurse -Force
    }

    git clone git@github.com:energyhelpline/energy-comparison-template-website.git
}

if ([System.IO.File]::Exists("changesSinceLastTag.patch"))
{
    Remove-Item changesSinceLastTag.patch
}

if ([System.IO.File]::Exists("changesSinceLastTag.temp.patch"))
{
    Remove-Item changesSinceLastTag.temp.patch
}

$latestTag = git tag | Select-Object -Last 1
Write-Host "`$latestTag is $latestTag"

$newTag = GetNextTag "$latestTag"
Write-Host "`$newTag is $newTag"

git tag -a $newTag -m "Release version $newTag"
git diff $latestTag $newTag --binary | Out-File -encoding ASCII changesSinceLastTag.temp.patch
((Get-Content .\changesSinceLastTag.temp.patch) -join "`n")  | Out-File -encoding ASCII changesSinceLastTag.patch

ReclonePublicRepo
Push-Location .\energy-comparison-template-website
try {
    ApplyPatch $newTag ..\changesSinceLastTag.patch
    git push --follow-tags
    Pop-Location
    git push --tags # git push tags on private repo
    return 0
}
catch {
    Pop-Location
    git tag --delete $newTag
    return 1
}