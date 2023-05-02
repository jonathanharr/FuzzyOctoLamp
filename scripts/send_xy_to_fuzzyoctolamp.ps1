# Set-Color.ps1
param (
    [Parameter(Mandatory=$true)][string]$XValue,
    [Parameter(Mandatory=$true)][string]$YValue
)

$URL = "https://fuzzy-octo-lamp.azurewebsites.net/api/SetHueLampColor"
$HostKey = "3uumdf7AHbIE5BhMJAfU4SXsRcvxS4sjvNOCS3YVIC60AzFuwdEeTQ=="
$Headers = @{
    "Content-Type" = "application/json"
    "x-functions-key" = $HostKey
}
$Body = @{
    "xValue" = $XValue
    "yValue" = $YValue
} | ConvertTo-Json

Invoke-WebRequest -Uri $URL -Method POST -Headers $Headers -Body $Body -UseBasicParsing
