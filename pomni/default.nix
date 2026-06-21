builtins.mapAttrs (
  name: args:
  (builtins.fetchTarball {
    url = args.url;
    sha256 = args.hash;
  })
) (builtins.fromJSON (builtins.readFile ./pomni.lock.json))
