param(
    [Parameter(Mandatory)]
    [string] $Subject,
    [Parameter(Mandatory)]
    [string] $Upn,
    [Parameter(Mandatory)]
    [string] $Password
)

$Certificate = New-SelfSignedCertificate `
    -CertStoreLocation Cert:\CurrentUser\My `
    -Type Custom `
    -Subject $Subject `
    -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.2","2.5.29.17={text}upn=$Upn") `
    -KeyUsage DigitalSignature `
    -KeyAlgorithm RSA `
    -KeyLength 2048 `
    -NotAfter (Get-Date).AddMonths(6)

$CertPassword = ConvertTo-SecureString `
    -String $Password `
    -Force `
    -AsPlainText

$Certificate | Export-PfxCertificate `
    -FilePath .\mtls.pfx `
    -Password $CertPassword

$PfxBytes = Get-Content .\mtls.pfx -AsByteStream

$PfxBase64 = [System.Convert]::ToBase64String($PfxBytes)

Remove-Item -Path .\mtls.pfx -Force

Write-Host "Certificate Thumbprint:" -ForegroundColor Green
Write-Host $Certificate.Thumbprint
Write-Host "Certificate Base64:" -ForegroundColor Green
Write-Host $PfxBase64
