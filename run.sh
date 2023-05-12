hub=rido
edgeId=tr-gateway
connStr=$(az iot hub module-identity connection-string show -n $hub -d $edgeId -m '$edgeHub' --query connectionString | tr -d '"')
docker run -it --rm -e IotHubConnectionString="$connStr" -e RuntimeLogLevel=debug -p 8883:8883 ghcr.io/ridomin/edgehub:local
