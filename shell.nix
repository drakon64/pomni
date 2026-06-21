with import (import ./pomni.nix).nixpkgs { };

mkShell {
  packages = [ dotnetCorePackages.sdk_10_0 ];
}
