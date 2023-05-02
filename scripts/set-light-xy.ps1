param (
    [Parameter(Mandatory=$true)][string]$AccessToken,
    [Parameter(Mandatory=$true)][string]$ApiUser,
    [Parameter(Mandatory=$true)][string]$LightId,
    [float]$XValue,
    [float]$YValue
)

Write-Host "X: $XValue, Y: $YValue"
Write-Host "Processing light command"

# Set up variables
$baseUri = "https://api.meethue.com"
$body = @{
    "on" = $true
    "xy" = @($XValue, $YValue)
    "bri" = 100
    "transitiontime" = 0
}

# Set up the request
$headers = @{
    "Authorization" = "Bearer $AccessToken"
    "Content-Type" = "application/json"
}
$uri = "$baseUri/bridge/$ApiUser/lights/$LightId/state" # Replace <bridge-id> with your bridge ID
$method = "PUT"
$bodyJson = $body | ConvertTo-Json

# Send the request
Invoke-RestMethod -Method $method -Uri $uri -Headers $headers -Body $bodyJson
