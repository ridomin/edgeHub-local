FROM mcr.microsoft.com/azureiotedge-hub:1.4
COPY certs /certs
ENV EdgeModuleHubServerCertificateFile=/certs/localhost.pem
ENV EdgeModuleHubServerCAChainCertificateFile=/certs/ca.pem
ENV EdgeHubDevServerCertificateFile=/certs/localhost.pem
ENV EdgeHubDevTrustBundleFile=/certs/ca.pem
ENV EdgeHubDevServerPrivateKeyFile=/certs/localhost.key
EXPOSE 8883