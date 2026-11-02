<#
.SYNOPSIS
    Starts FoodSense API + Blazor Frontend with one command.

.DESCRIPTION
    The API is built into a private run folder (.build\api-run) and started from
    that copy, so bin\Debug of FoodSense.API is never locked by a running process.
    Without this, building the API (or the whole solution) while it is running
    fails with MSB3026/MSB3027. Since the Frontend no longer references the API
    project, `dotnet build` of the Frontend succeeds even with the API running.

    The Frontend runs under `dotnet watch` (hot reload) and listens on 0.0.0.0,
    so it is reachable from a phone on the LAN.

.LINK
    ./Start.ps1                      # build, start API and Frontend
    ./Start.ps1 -NoWatch             # Frontend without dotnet watch (plain dotnet run)
    ./Start.ps1 -FrontendOnly        # Frontend only
    ./Start.ps1 -Stop                # stop processes started by this script
    ./Start.ps1 -BuildOnly           # build both, do not start anything
#>
param(
    [int]$ApiPort = 5226,
    [int]$FrontendPort = 5000,
    [switch]$NoWatch,
    [switch]$FrontendOnly,
    [switch]$Stop,
    [switch]$BuildOnly
)

$ErrorActionPreference = 'Stop'
$Root = $PSScriptRoot
$ApiProject      = "$Root\FoodSense.API\FoodSense.API.csproj"
$FrontendProject = "$Root\FoodSense.BlazorFrontend\FoodSense.BlazorFrontend.csproj"
$ApiRunDir       = "$Root\.build\api-run"

function Get-PortOwner([int]$Port) {
    Get-NetTCPConnection -State Listen -LocalPort $Port -ErrorAction SilentlyContinue |
        Select-Object -First 1 -ExpandProperty OwningProcess
}

function Stop-ByPort([int]$Port, [string]$Name) {
    $procId = Get-PortOwner $Port
    if ($procId) {
        Write-Host "Stopping $Name (PID $procId) listening on :$Port"
        Stop-Process -Id $procId -Force -ErrorAction SilentlyContinue
        Start-Sleep -Seconds 1
    }
}

function Start-Window([string]$Title, [string]$WorkDir, [string]$Command) {
    $b64 = [Convert]::ToBase64String(
        [Text.Encoding]::Unicode.GetBytes($Command)
    )
    Start-Process $PSExe -ArgumentList @(
        '-NoProfile', '-NoExit', '-EncodedCommand', $b64
    ) -WorkingDirectory $WorkDir
    Write-Host "Started $Title (window '$Title')"
}

function Get-LanUrls([int]$Port) {
    Get-NetIPAddress -AddressFamily IPv4 -ErrorAction SilentlyContinue |
        Where-Object { $_.IPAddress -notlike '127.*' -and $_.IPAddress -notlike '169.254.*' } |
        ForEach-Object { "http://$($_.IPAddress):$Port" }
}

function Build-Api() {
    Write-Host "== Building API (normal bin\Debug output) =="
    dotnet build $ApiProject -v minimal
    if ($LASTEXITCODE -ne 0) { throw "API build failed" }
}

function Sync-ApiRun() {
    # The running API holds the files in the run folder, so the old instance is
    # stopped before the fresh build is copied in.
    Stop-ByPort $ApiPort 'API'
    Write-Host "== Copying build to $ApiRunDir (API runs from this copy) =="
    robocopy "$Root\FoodSense.API\bin\Debug\net10.0" $ApiRunDir /MIR /NJH /NJS /NDL /NFL | Out-Null
    if ($LASTEXITCODE -ge 8) { throw "Copy to run folder failed (robocopy exit $LASTEXITCODE)" }
    $LASTEXITCODE = 0

    # Static-assets manifests live in the project folder, not in the build output.
    Get-ChildItem "$Root\FoodSense.API" -Filter '*.staticwebassets.*.json' -ErrorAction SilentlyContinue |
        Copy-Item -Destination $ApiRunDir -Force
}

function Build-Frontend() {
    Write-Host "== Building Frontend =="
    dotnet build $FrontendProject -v minimal
    if ($LASTEXITCODE -ne 0) { throw "Frontend build failed" }
}

function Start-Api() {
    $cmd = "\$env:ASPNETCORE_ENVIRONMENT='Development'; " +
           "dotnet FoodSense.API.dll --urls http://localhost:$ApiPort"
    Start-Window 'FoodSense-API' $ApiRunDir $cmd
    Write-Host "API: http://localhost:$ApiPort (swagger: http://localhost:$ApiPort/swagger)"
}

function Start-Frontend() {
    Stop-ByPort $FrontendPort 'Frontend'
    $urls = "http://0.0.0.0:$FrontendPort"
    $env = "\$env:ASPNETCORE_ENVIRONMENT='Development'; "
    # The project path contains a space ("FoodSense Aspire"), so it must be quoted
    # inside the generated command string; PowerShell would split an unquoted path
    # into two arguments and dotnet would run a different project.
    $proj = "`"$FrontendProject`""
    if ($NoWatch) {
        $cmd = $env + "dotnet run --project $proj --no-launch-profile --urls $urls"
    }
    else {
        $cmd = $env + "dotnet watch --project $proj run --no-launch-profile --urls $urls"
    }
    Start-Window 'FoodSense-Frontend' $Root $cmd
    Write-Host "Frontend: http://localhost:$FrontendPort"
    foreach ($u in Get-LanUrls $FrontendPort) { Write-Host "Frontend (LAN, for phone): $u" }
}

$PSExe = (Get-Process -Id $PID).MainModule.FileName
$env:ASPNETCORE_ENVIRONMENT = 'Development'

if ($Stop) {
    Stop-ByPort $FrontendPort 'Frontend'
    Stop-ByPort $ApiPort 'API'
    return
}

if (-not $FrontendOnly) {
    Build-Api
    Sync-ApiRun
    Build-Frontend
}

if ($BuildOnly) {
    Write-Host "`nBuild finished. Nothing was started."
    return
}

if (-not $FrontendOnly) { Start-Api }
Start-Frontend

Write-Host "`nDone. Frontend logs are in the 'FoodSense-Frontend' window."
if (-not $FrontendOnly) { Write-Host "API logs are in the 'FoodSense-API' window." }
if (-not $NoWatch) {
    Write-Host "Edit .razor/.cs -> Frontend hot-reloads (dotnet watch). Ctrl+C in the Frontend window stops it."
}
