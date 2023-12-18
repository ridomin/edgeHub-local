hub=tests
edgeId=e1
#connStr=$(az iot hub module-identity connection-string show -n $hub -d $edgeId -m '$edgeHub' --query connectionString | tr -d '"')
connStr='HostName=tests.azure-devices.net;DeviceId=e1;ModuleId=$edgeHub;SharedAccessKey=L3kmGFECSSSzJBsBLFwAFXjEjHK+ZqnP+iQy7rXxkl8='

 #dotnet dev-certs https -ep _certs/localhost.pem --format PEM --no-password
 #cp _certs/localhost.pem _certs/ca.pem

docker run -it --rm \
    -e IotHubConnectionString="$connStr" \
    -e EdgeModuleHubServerCertificateFile=/certs/localhost.pem \
    -e EdgeModuleHubServerCAChainCertificateFile=/certs/ca.pem \
    -e EdgeHubDevServerCertificateFile=/certs/localhost.pem \
    -e EdgeHubDevTrustBundleFile=/certs/ca.pem \
    -e EdgeHubDevServerPrivateKeyFile=/certs/localhost.key \
    -v $(pwd)/_certs:/certs \
    -p 8883:8883 \
    mcr.microsoft.com/azureiotedge-hub:1.4