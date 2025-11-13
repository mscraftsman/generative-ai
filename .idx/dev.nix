{ pkgs, ... }: {

  # Which nixpkgs channel to use.
  channel = "stable-25.05"; # or "unstable"

  # Use https://search.nixos.org/packages to find packages
  packages = [
    pkgs.dotnetCorePackages.sdk_10_0
    pkgs.dotnet-sdk_10
  ];

  # Sets environment variables in the workspace
  env = {
    DOTNET_NOLOGO = "true";
    DOTNET_CLI_TELEMETRY_OPTOUT = "true";
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "true";
  };

  # Search for the extensions you want on https://open-vsx.org/ and use "publisher.id"
  idx.extensions = [
    "muhammad-sammy.csharp"
    "ms-azuretools.vscode-docker"
    "humao.rest-client"
    "rangav.vscode-thunder-client"
  ];

  # enterShell = ''
  #   echo "Checks latest info in the README.md."
  #   echo "The tests provide the latest and greatest samples."
  # '';
}
