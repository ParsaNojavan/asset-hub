import { ArrowLeft } from "lucide-react";
import Link from "next/link";

export function BreadcrumbBar({ breadcrumbs,parentId } : {breadcrumbs : string[],parentId:string}) {

  return (
    <div className="flex items-center gap-4 text-sm text-zinc-400">
          {parentId && (
            <Link
            href={`http://localhost:3000/dashboard/assets?folderId=${parentId}`}
            className="p-2 rounded hover:bg-zinc-800 cursor-pointer">
            <ArrowLeft size={18} />
            </Link>
          )}    

          <div className="flex items-center gap-2 flex-wrap">
            {breadcrumbs.map((crumb, i) => (
              <div key={i} className="flex items-center gap-2">
                <span className="text-[small-text-color]">{crumb}</span>
                {i !== breadcrumbs.length - 1 && (
                  <span className="text-zinc-500">&gt;</span>
                )}
              </div>
            ))}
          </div>

        </div>
  )
}
