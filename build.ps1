<#

.SYNOPSIS
This is a Powershell script to bootstrap a Cake build.

.DESCRIPTION
This Powershell script will download NuGet if missing, restore NuGet tools (including Cake)
and execute your Cake build script with the parameters you provide.

.PARAMETER Script
The build script to execute.
.PARAMETER ScriptArgs
Remaining arguments are added here.

.LINK
http://cakebuild.net

#>

[CmdletBinding()]
Param(
    [string]$Script = "build.cake",
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$ScriptArgs
)

Write-Host "Preparing to run build script..."

if(!$PSScriptRoot){
    $PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent
}

$TOOLS_DIR = Join-Path $PSScriptRoot "tools"
$NUGET_EXE = Join-Path $TOOLS_DIR "nuget.exe"
$CAKE_EXE = Join-Path $TOOLS_DIR "Cake/Cake.exe"
$NUGET_URL = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"

# Make sure tools folder exists
if ((Test-Path $PSScriptRoot) -and !(Test-Path $TOOLS_DIR)) {
    Write-Verbose -Message "Creating tools directory..."
    New-Item -Path $TOOLS_DIR -Type directory | out-null
}

# Try find NuGet.exe in path if not exists
if (!(Test-Path $NUGET_EXE)) {
    Write-Verbose -Message "Trying to find nuget.exe in PATH..."
    $existingPaths = $Env:Path -Split ';' | Where-Object { (![string]::IsNullOrEmpty($_)) -and (Test-Path $_ -PathType Container) }
    $NUGET_EXE_IN_PATH = Get-ChildItem -Path $existingPaths -Filter "nuget.exe" | Select -First 1
    if ($NUGET_EXE_IN_PATH -ne $null -and (Test-Path $NUGET_EXE_IN_PATH.FullName)) {
        Write-Verbose -Message "Found in PATH at $($NUGET_EXE_IN_PATH.FullName)."
        $NUGET_EXE = $NUGET_EXE_IN_PATH.FullName
    }
}

# Try download NuGet.exe if not exists
if (!(Test-Path $NUGET_EXE)) {
    Write-Verbose -Message "Downloading NuGet.exe..."
    try {
        (New-Object System.Net.WebClient).DownloadFile($NUGET_URL, $NUGET_EXE)
    } catch {
        Throw "Could not download NuGet.exe."
    }
}

# Save nuget.exe path to environment to be available to child processed
$ENV:NUGET_EXE = $NUGET_EXE

# Restore Cake build tool locally.
Write-Verbose "Restoring cake tools to $TOOLS_DIR"
if (!(Test-Path $TOOLS_DIR)) {
    Write-Verbose -Message "Creating tools directory..."
    New-Item -Path $TOOLS_DIR -Type directory | out-null
}

Push-Location
Set-Location $TOOLS_DIR

if ((Test-Path "packages.config")) {
    Write-Verbose -Message "Restoring tools from NuGet..."
    $NuGetOutput = Invoke-Expression "&`"$ENV:NUGET_EXE`" install -ExcludeVersion -OutputDirectory `"$TOOLS_DIR`""
} else {
    Write-Verbose -Message "Installing Cake from NuGet..."
    $NuGetOutput = Invoke-Expression "&`"$ENV:NUGET_EXE`" install Cake -ExcludeVersion -OutputDirectory `"$TOOLS_DIR`""
}

if ($LASTEXITCODE -ne 0) {
    Pop-Location
    Throw "An error occured while restoring NuGet tools.";
}
Write-Verbose -Message ($NuGetOutput | out-string)

# Make sure that Cake has been installed.
if (!(Test-Path "./Cake/Cake.exe")) {
    Throw "Could not find Cake.exe at ./Cake/Cake.exe"
}

Pop-Location

# Start Cake
Write-Host "Running build script..."
Write-Host "& `"$CAKE_EXE`" `"$Script`" $ScriptArgs"
Invoke-Expression "& `"$CAKE_EXE`" `"$Script`" $ScriptArgs" | Tee-Object .\build-log.txt
exit $LASTEXITCODE
