const previewableTypes = [
  "image/",
  "application/pdf",
  "text/",
  "video/",
  "audio/"
];

export function canPreview(contentType : string) {
  return previewableTypes.some(t => contentType.startsWith(t));
}