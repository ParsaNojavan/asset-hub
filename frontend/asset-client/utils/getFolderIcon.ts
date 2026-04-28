// src/utils/getFolderIcon.ts

export function createFolderIconGetter(resolver: {
  client?: () => string | null;
  server?: () => string | null;
}) {
  return function getFolderIcon() {
    let pack = "default";

    if (typeof window !== "undefined" && resolver.client) {
      pack = resolver.client() || "default";
    } else if (resolver.server) {
      pack = resolver.server() || "default";
    }

    return `/icons/${pack}/folder.ico`;
  };
}
