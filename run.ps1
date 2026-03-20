Write-Host "Building solution..."
dotnet build Boolk.sln
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$api = Start-Process dotnet -ArgumentList "run --project src/Boolk.API/Boolk.API.csproj --no-build" -PassThru -NoNewWindow
$client = Start-Process dotnet -ArgumentList "run --project src/Boolk.Client/Boolk.Client.csproj --no-build" -PassThru -NoNewWindow

Write-Host "Started API (PID $($api.Id)) and Client (PID $($client.Id)). Press Ctrl+C to stop both."

try {
    $api.WaitForExit()
    $client.WaitForExit()
} finally {
    if (!$api.HasExited) { $api.Kill() }
    if (!$client.HasExited) { $client.Kill() }
}
