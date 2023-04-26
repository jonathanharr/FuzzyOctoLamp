param (
    [Parameter(Mandatory=$true)][string]$AccessToken,
    [Parameter(Mandatory=$true)][string]$ApiUser,
    [Parameter(Mandatory=$true)][string]$LightId
)

$baseUri = "https://api.meethue.com"

$headers = @{
    "Authorization" = "Bearer $AccessToken"
    "Content-Type" = "application/json"
}
$uri = "$baseUri/bridge/$ApiUser/lights/$lightId" # Replace <bridge-id> with your bridge ID
$method = "GET"

$response = Invoke-RestMethod -Method $method -Uri $uri -Headers $headers -Body $bodyJson

$hue = $response.state.hue
$saturation = $response.state.sat
$brightness = $response.state.bri

Write-Host "Hue: $hue"
Write-Host "Saturation: $saturation"
Write-Host "Brightness: $brightness"

$x = $response.state.xy[0]
$y = $response.state.xy[1]

$xyValues = New-Object -TypeName PSObject -Property @{
    X = $x
    Y = $y
}
return $xyValues