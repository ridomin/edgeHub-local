# edheHub-local

This repo produces a docker image to use Azure IoT Edge $edgeHub module in a local environment.

```
ghcr.io/ridomin/edgehub:local
```

## How to run

You need an IoT Edge device registered and configured in IoTHub to obtain the `$edgeHub` module connection-string. Then next `az` command gets the connection string:

```bash
hub=<hubName>
edgeId=<edgeDeviceId>
az iot hub module-identity connection-string show -n $hub -d $edgeId -m '$edgeHub' --query connectionString
```

The docker container can be instantiated locally with:

```cs
connStr=$(az iot hub module-identity connection-string show -n $hub -d $edgeId -m '$edgeHub' --query connectionString | tr -d '"')
docker run -it --rm -e IotHubConnectionString="$connStr" -p 8883:8883 ghcr.io/ridomin/edgehub:local
```

## Certificates

The certificates included in the container image are configured for `localhost` issued by the private CA [RidoFY23CA](certs/ca.pem)

> Using the same certificates as [mosquitto-local](https://github.com/ridomin/mosquitto-local)

### Complete instructions

This document provides more usage details: https://github.com/Azure/iotedge/tree/main/edge-hub
