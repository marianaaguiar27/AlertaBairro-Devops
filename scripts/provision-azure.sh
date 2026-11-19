#!/usr/bin/env bash

az group create --name rg-alertabairro --location mexicocentral

az vm create \
  --resource-group rg-alertabairro \
  --name vm-alertabairro \
  --location mexicocentral \
  --image Canonical:ubuntu-24_04-lts:server:latest \
  --size Standard_B2s_v2 \
  --admin-username azureuser \
  --generate-ssh-keys \
  --public-ip-sku Standard \
  --nsg-rule NONE