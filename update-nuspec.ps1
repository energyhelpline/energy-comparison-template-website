 param (
    [string]$versionNumber
 )

$filePath = $pwd.Path + "\BareboneUi.nuspec";

$xml = [xml](get-content (get-item $filePath) );
$xml.package.metadata.version = $versionNumber;
$xml.Save($filePath);
