
$service = "urTribeService"
$display = "urTribe Service"

$fullPathExe = "C:\Users\Thomas\Documents\Visual Studio 2013\Projects\urTribeWebAPI\urTribeWindowService\bin\Debug\urTribeWindowService.exe"


if (Get-Service $service -ErrorAction SilentlyContinue)
{

        echo "Stopping $service..."
	sc.exe stop $service  

        echo "Removing service $service..."
	sc.exe delete $service 

        if ($? -ne $true){
           echo "Unable to delete service $service"
        }
}

start-sleep 2
echo "Installing...."
echo $fullPathExe
$passwordText = Read-Host 'Enter Passowrd ' -AsSecureString
$credential = New-Object System.Management.Automation.PSCredential "VIRTUAL-PC\Thomas",$passwordText
New-Service -name $service -binaryPathName $fullPathExe -displayName $display -startupType Automatic -credential $credential -Verbose

echo "Staring $service..."
Start-Service $service
Get-Service $service 


