SKIP_DOWNLOAD=false
API_VERSION=""

# Parse arguments
while [[ "$#" -gt 0 ]]; do
    case $1 in
        -s|--skip-download) SKIP_DOWNLOAD=true ;;
        -v|--version) API_VERSION="$2"; shift ;;
        -h|--help) echo "Usage: $0 [-s|--skip-download] [-v|--version <version>]"; exit 0 ;;
        *) echo "Unknown parameter passed: $1"; exit 1 ;;
    esac
    shift
done

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

if [ "$SKIP_DOWNLOAD" = false ]; then
  curl "https://generativelanguage.googleapis.com/\$discovery/rest?version=${API_VERSION:-v1beta}&key=$GEMINI_API_KEY" \
    -H "Content-Type: application/json" \
    -o generativelanguage.json

  curl "https://aiplatform.googleapis.com/\$discovery/rest?version=${API_VERSION:-v1beta}1" \
    -H "Content-Type: application/json" \
    -o aiplatform.json

  jq --sort-keys . generativelanguage.json > $SOURCE/discovery.json
  jq --sort-keys . aiplatform.json > $SOURCE/discovery.vertex.json

  rm generativelanguage.json
  rm aiplatform.json
fi

# Generate C# types...
dotnet run --project ./Mscc.CodeGenerator.csproj $SOURCE/discovery.json ./Types
dotnet run --project ./Mscc.CodeGenerator.csproj $SOURCE/discovery.vertex.json ./Types

cp ./Types/* $TARGET
rm -f ./Types/*
