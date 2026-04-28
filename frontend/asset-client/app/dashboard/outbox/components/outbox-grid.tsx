import Link from "next/link";
import Image from "next/image";
import { cookies } from "next/headers";
import { createIconGetter } from "@/utils/getIcon";


interface OutboxFile {
  asset: {
    id: string,
    fileName: string,
    contentType: string,
  },
  shareId: string
}

export default async function OutboxGrid({ outboxFiles }: { outboxFiles: OutboxFile[] }) {

  const cookieStore = await cookies();
  const iconPack = cookieStore.get("iconPack")?.value || "default";

  const getIcon = createIconGetter({
    server: () => iconPack
  });

  return (
    <div className="grid grid-cols-2 sm:grid-cols-4 lg:grid-cols-6 gap-8">
      {outboxFiles.map((file) => (
        <div
          key={file.asset.fileName}
          className="flex flex-col items-center group relative"
        >
          <div className="cursor-pointer transition-transform group-hover:scale-105">
            <Image
              src={getIcon(file.asset.contentType)}
              alt={file.asset.fileName}
              width={50}
              height={50}
              className="drop-shadow-lg"
            />
          </div>

          <span className="text-sm text-[small-text-color] mt-2 text-center w-28 truncate">
            {file.asset.fileName}
          </span>

          {/* Details Button */}
          <Link
            className="
                mt-3 px-4 py-1.5 text-sm
                rounded-md
                bg-zinc-700/70 text-zinc-200
                lg:opacity-0 group-hover:opacity-100
                transition-all
                hover:bg-zinc-600
                cursor-pointer
              "
            href={`/dashboard/outbox/${file.shareId}`}
          >
            Details
          </Link>
        </div>
      ))}
    </div>
  );
}