{ pkgs, ... }: {

  # Which nixpkgs channel to use.
  channel = "stable-24.05"; # or "unstable"

  # Use https://search.nixos.org/packages to find packages
  packages = [
    pkgs.dotnetCorePackages.sdk_9_0
  ];

  # Sets environment variables in the workspace
  env = {
    DOTNET_NOLOGO=true;
    DOTNET_CLI_TELEMETRY_OPTOUT=true;
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true;
  };

  # Search for the extensions you want on https://open-vsx.org/ and use "publisher.id"
  idx.extensions = [
    "muhammad-sammy.csharp"
    "ms-azuretools.vscode-docker"
    "humao.rest-client"
    "rangav.vscode-thunder-client"
  ];
}
