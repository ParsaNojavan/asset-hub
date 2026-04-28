import Link from "next/link";
import Image from "next/image";
import { cookies } from "next/headers";
import { createIconGetter } from "@/utils/getIcon";

interface InboxItem {
  asset: {
    id: string,
    fileName: string,
    contentType: string,
  },
  user: {
    username: string,
    email: string,
    imgUrl: string
  }
}

export default async function InboxTable({ inboxItems }: { inboxItems: InboxItem[] }) {

  const cookieStore = await cookies();
  const iconPack = cookieStore.get("iconPack")?.value || "default";

  const getIcon = createIconGetter({
    server: () => iconPack
  });

  return (
    <div className="overflow-x-auto border border-zinc-800 rounded-lg">
      <table className="min-w-full text-sm">
        <thead className="bg-zinc-900/60 text-table-text-color">
          <tr>
            <th className="text-left font-medium px-6 py-4 min-w-[160px]">
              Sender
            </th>
            <th className="text-left font-medium px-6 py-4 min-w-[180px]">
              File
            </th>
            <th className="text-right font-medium px-6 py-4 min-w-[120px]">
              Action
            </th>
          </tr>
        </thead>

        <tbody>
          {inboxItems.map((item) => (
            <tr
              key={item.asset.id}
              className="border-t border-zinc-800 hover:bg-zinc-900/40 transition"
            >
              {/* Sender */}
              <td className="px-6 py-4">
                <div className="flex items-center gap-3">
                  <img
                    src={`https://localhost:7024/${item.user.imgUrl}`}
                    alt={item.user.username}
                    className="w-8 h-8 rounded-full object-cover bg-zinc-800"
                  />
                  <span className="text-[text-color]">{item.user.email}</span>
                </div>
              </td>

              {/* File */}
              <td className="px-6 py-4">
                <div className="flex items-center gap-3">
                  <Image
                    src={getIcon(item.asset.contentType)}
                    alt={item.asset.fileName}
                    width={50}
                    height={50}
                    className="drop-shadow-lg"
                  />
                  <span className="text-[small-text-color]">{item.asset.fileName}</span>
                </div>
              </td>

              {/* Action */}
              <td className="px-6 py-4 text-right">
                <Link
                  className="
                      px-4 py-1.5 text-sm
                      rounded-md
                      bg-zinc-700/70 text-zinc-200
                      hover:bg-zinc-600
                      transition
                      cursor-pointer
                    "
                  href={`/dashboard/inbox/${item.asset.id}`}
                >
                  Details
                </Link>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}