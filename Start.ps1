# Start.ps1
param ([switch]$watch)

$apiProjectPath = "FoodSense.API"
$frontendProjectPath = "FoodSense.BlazorFrontend"

$verb = if ($watch) { "watch run" } else { "run" }

Write-Host "Launching API in a new window..."
Start-Process powershell -ArgumentList "-NoExit", "-Command", "dotnet $verb --project $apiProjectPath"

Write-Host "Launching Frontend in a new window..."
Start-Process powershell -ArgumentList "-NoExit", "-Command", "dotnet $verb --project $frontendProjectPath"