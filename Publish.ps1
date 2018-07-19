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
    git add .
    git reset .\BareboneUi\appsettings.json
    git commit -m "Release version $tag"
    git checkout .
    git tag -a $tag -m "Release version $tag"
}

function ReclonePublicRepo{
    Remove-Item .\barebone-ui-public -Recurse -Force
    git clone git@github.com:energyhelpline/barebone-ui-public.git
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

ReclonePublicRepo
Push-Location .\barebone-ui-public
ApplyPatch $newTag ..\changesSinceLastTag.patch
git push --follow-tags
Pop-Location

# git push tags on private repo
git push --tags