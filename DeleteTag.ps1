Param(
    [Parameter(Mandatory=$True)]
    [string]$tagToDelete
)

git tag --delete $tagToDelete
git push --delete origin $tagToDelete

Push-Location .\energy-comparison-template-website
git tag --delete $tagToDelete
git push --delete origin $tagToDelete
Pop-Location