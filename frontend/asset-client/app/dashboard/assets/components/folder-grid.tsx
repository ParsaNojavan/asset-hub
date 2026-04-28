'use client'

import Modal from "@/app/components/modal";
import { Download, Edit, Share2, Trash2 } from "lucide-react";
import Link from "next/link";
import {useRouter} from "next/navigation";
import { useMemo, useState } from "react";
import RenameModal from "./rename-modal";
import { createFolderIconGetter } from "@/utils/getFolderIcon";

interface folder {
    name : string
    id: string;
}

export function FolderGrid({ folders } : { folders : folder[]}) {

  const getFolderIcon = useMemo(() => {
      return createFolderIconGetter({
        client: () => {
          if (typeof document === "undefined") return "default";
          const value = `; ${document.cookie}`;
          const parts = value.split(`; iconPack=`);
          if (parts.length === 2) return parts.pop()?.split(";").shift() || "default";
          return "default";
        }
      });
    }, []); 

  const router = useRouter();
  const [renameOpen , setRenameOpen] = useState(false);
  const [deleteOpen, setDeleteOpen] = useState(false);
  const [selectedFolder, setSelectedFolder] = useState<folder | null>(null);

  const openDelete = (folder: folder) => {
    setSelectedFolder(folder);
    setDeleteOpen(true);
  };

    const openRename = (folder : folder) => {
      setSelectedFolder(folder)
    setRenameOpen(true);
  }

  return (
    <>
    <div className="grid grid-cols-2 sm:grid-cols-4 lg:grid-cols-6 gap-8 mb-12">
        {folders.map((f) => (
          <div key={f.id} className="flex flex-col items-center group cursor-pointer">

  <Link
    href={`/dashboard/assets?folderId=${f.id}`}
    className="flex flex-col items-center group cursor-pointer">
       <img width={60} src={getFolderIcon()} className="text-blue-500 mb-2 transition-transform group-hover:scale-105" />
       <span className="text-sm text-[small-text-color]">{f.name}</span>
  </Link>

  <div className="flex gap-2 mt-2 opacity-100 lg:opacity-0 group-hover:opacity-100 transition-opacity bg-zinc-800/80 backdrop-blur-md p-1.5 rounded-lg border border-zinc-700/50">
    <button
      onClick={(e) => {
        e.stopPropagation();
        e.preventDefault();
        openDelete(f);
      }}
      className="p-1 hover:bg-red-500/80 rounded text-zinc-400 hover:text-white cursor-pointer"
    >
      <Trash2 size={14} />
    </button>
    <button
        onClick={() => openRename(f)}
        className="p-1 hover:bg-amber-600 rounded text-zinc-400 hover:text-white cursor-pointer transition-colors"
    >
      <Edit size={14} />
  </button>
  </div>
</div>

        ))}
    </div>

    {/* DELETE MODAL */}
          <Modal open={deleteOpen} onClose={() => setDeleteOpen(false)} title="Delete Asset">
            <div className="flex flex-col gap-6">
              <h4 className="text-foreground text-base font-medium">
                Delete {selectedFolder?.name}?
              </h4>
              <span className="text-zinc-500 text-sm">
                This action cannot be undone.this asset will be deleted permenantly
              </span>
    
              <div className="flex flex-col sm:flex-row gap-3 sm:justify-end">
                <button
                onClick={async () => {
                    await fetch(`/api/folders/delete/${selectedFolder?.id}`, {
                      method: "DELETE",
                      credentials: "include"
                    });
    
                    setDeleteOpen(false); 
                    router.refresh()
                  }}
                 className="px-4 py-2 cursor-pointer rounded bg-red-600 hover:bg-red-700 text-white text-sm">
                  Yes, Delete
                </button>
                <button
                  onClick={() => setDeleteOpen(false)}
                  className="px-4 py-2 rounded cursor-pointer border border-zinc-700 text-[text-color] hover:bg-zinc-800 text-sm hover:text-white"
                >
                  Cancel
                </button>
              </div>
            </div>
          </Modal>

          <RenameModal
              open={renameOpen}
              onClose={()=>setRenameOpen(false)}
              Id={selectedFolder?.id}
              endpoint="http://localhost:3000/api/folders/rename"
          />
    </>
  )
}
