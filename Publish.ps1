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

if ([System.IO.File]::Exists("changesSinceLastTag.patch"))
{
    Remove-Item changesSinceLastTag.patch
}

if ([System.IO.File]::Exists("changesSinceLastTag.temp.patch"))
{
    Remove-Item changesSinceLastTag.temp.patch
}

$latestTag = git tag --sort=-creatordate | Select-Object -First 1
Write-Host "`$latestTag is $latestTag"

$newTag = GetNextTag "$latestTag"
Write-Host "`$newTag is $newTag"

git tag -a $newTag -m "Release version $newTag"
git diff $latestTag $newTag | Out-File -encoding ASCII changesSinceLastTag.temp.patch
((Get-Content .\changesSinceLastTag.temp.patch) -join "`n")  | Out-File -encoding ASCII changesSinceLastTag.patch

Remove-Item .\barebone-ui-public -Recurse -Force
git clone git@github.com:energyhelpline/barebone-ui-public.git
Push-Location .\barebone-ui-public
git apply ..\changesSinceLastTag.patch
git add .
git reset .\BareboneUi\appsettings.json
git commit -m "Release version $newTag"
git tag -a $newTag -m "Release version $newTag"
git checkout .
git push --follow-tags
Pop-Location