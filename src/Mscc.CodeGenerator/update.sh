#!/bin/bash

if [ -f .env ]; then
  # 1. Turn on "allexport" -> any variable defined in the next step is automatically exported
  set -a
  # 2. Source the file (execute it)
  source .env
  # 3. Turn off "allexport"
  set +a
fi

mkdir -p ./Types
rm -f ./Types/*

curl "https://generativelanguage.googleapis.com/\$discovery/rest?version=v1beta&key=$GEMINI_API_KEY" \
  -H "Content-Type: application/json" \
  -o generativelanguage.json

curl "https://aiplatform.googleapis.com/\$discovery/rest?version=v1beta1" \
  -H "Content-Type: application/json" \
  -o aiplatform.json

jq --sort-keys . generativelanguage.json > $SOURCE/discovery.json
jq --sort-keys . aiplatform.json > $SOURCE/discovery.vertex.json

# Generate C# types...
dotnet run --project ./Mscc.CodeGenerator.csproj $SOURCE/discovery.json ./Types
dotnet run --project ./Mscc.CodeGenerator.csproj $SOURCE/discovery.vertex.json ./Types

cp ./Types/* $TARGET
rm -f ./Types/*
