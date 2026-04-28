// src/utils/getIcon.ts

export type IconPackResolver = {
    client?: () => string | null;
    server?: () => string | null;
};

export function createIconGetter(resolver: IconPackResolver) {
    // mapping MIME -> filename
    const mimeToIcon: Record<string, string> = {
        "application/pdf": "pdf.png",
        "text/plain": "txt.ico",
        "text/csv": "excel.ico",
        "text/rtf": "txt.ico",
        "application/msonenote" : "onenote.ico",
        "application/msword": "word.ico",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document": "word.ico",
        "application/vnd.ms-excel": "excel.ico",
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet": "excel.ico",
        "application/vnd.ms-powerpoint": "powerpoint.ico",
        "application/vnd.openxmlformats-officedocument.presentationml.presentation": "powerpoint.ico",

        "image/jpeg": "photo.ico",
        "image/png": "photo.ico",
        "image/webp": "photo.ico",
        "image/gif": "photo.ico",
        "image/svg+xml": "photo.ico",
        "image/bmp": "photo.ico",
        "image/tiff": "photo.ico",
        "image/x-icon": "photo.ico",
        "image/heic": "photo.ico",
        "image/heif": "photo.ico",

        "video/mp4": "video.ico",
        "video/mpeg": "video.ico",
        "video/webm": "video.ico",
        "video/ogg": "video.ico",
        "video/quicktime": "video.ico",
        "video/x-msvideo": "video.ico",
        "video/x-matroska": "video.ico",

        "audio/mpeg": "audio.ico",
        "audio/wav": "audio.ico",
        "audio/aac": "audio.ico",
        "audio/ogg": "audio.ico",
        "audio/flac": "audio.ico",
        "audio/webm": "audio.ico",
        "audio/midi": "audio.ico",

        "application/x-zip-compressed" : "zip.ico",
        "application/zip": "zip.ico",
        "application/x-rar-compressed": "zip.ico",
        "application/gzip": "zip.ico",
        "application/x-7z-compressed": "zip.ico",
        "application/x-tar": "zip.ico",
        "application/x-bzip2": "zip.ico",

        "application/x-msdownload" : "program.ico"
    };

    function getIcon(mime: string): string {
        const fileName = mimeToIcon[mime] || "generic.ico";

        if (typeof window !== "undefined" && resolver.client) {
            const pack = resolver.client() || "default";
            return `/icons/${pack}/${fileName}`;
        }

        const pack = resolver.server ? resolver.server() : "default";

        return `/icons/${pack}/${fileName}`;
    }

    return getIcon;
}
