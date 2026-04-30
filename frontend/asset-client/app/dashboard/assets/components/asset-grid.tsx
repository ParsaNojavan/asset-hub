"use client";

import { useMemo, useState } from "react";
import { createIconGetter } from "@/utils/getIcon";
import Link from "next/link";
import { Share2, Trash2, Download, Pen, Edit } from "lucide-react";
import Modal from "@/app/components/modal";
import { useRouter } from "next/navigation";
import toast from "react-hot-toast";
import RenameModal from "./rename-modal";

type Asset = {
  fileName: string;
  id: string;
  contentType: string;
};

export default function AssetGrid({ assets }: { assets: Asset[] }) {
  const router = useRouter();
  const getIcon = useMemo(() => {
    return createIconGetter({
      client: () => {
        if (typeof document === "undefined") return "default";
        const value = `; ${document.cookie}`;
        const parts = value.split(`; iconPack=`);
        if (parts.length === 2) return parts.pop()?.split(";").shift() || "default";
        return "default";
      }
    });
  }, []); 

  const [deleteOpen, setDeleteOpen] = useState(false);
  const [renameOpen , setRenameOpen] = useState(false);
  const [shareOpen, setShareOpen] = useState(false);
  const [selectedAsset, setSelectedAsset] = useState<Asset | null>(null);

  const openDelete = (asset: Asset) => {
    setSelectedAsset(asset);
    setDeleteOpen(true);
  };

  const openShare = (asset: Asset) => {
    setSelectedAsset(asset);
    setShareOpen(true);
  };

  const openRename = (asset : Asset) => {
    setSelectedAsset(asset);
    setRenameOpen(true);
  }

  return (
    <>
      {/* FILE GRID */}
      <div className="grid grid-cols-2 sm:grid-cols-4 lg:grid-cols-6 gap-8">
        {assets.map((file) => (
          <div key={file.id} className="flex flex-col items-center group">

            {/* LINK (Icon + Name) */}
            <Link
              href={`/dashboard/assets/${file.id}`}
              className="flex flex-col items-center cursor-pointer transition-transform group-hover:scale-105"
            >
              <img 
                src={getIcon(file.contentType)} 
                alt="file icon"
                width={50} 
                height={50}
                className="text-zinc-400 object-contain" 
              />

              <span className="text-sm text-[small-text-color] mt-2 text-center w-24 truncate">
                {file.fileName}
              </span>
            </Link>

            {/* ACTION BUTTONS */}
            <div className="flex gap-2 mt-2 opacity-100 lg:opacity-0 group-hover:opacity-100 transition-opacity bg-zinc-800/80 backdrop-blur-md p-1.5 rounded-lg border border-zinc-700/50">
              
              <button
                onClick={() => openShare(file)}
                className="p-1 hover:bg-zinc-600 rounded text-zinc-400 hover:text-white cursor-pointer"
              >
                <Share2 size={14} />
              </button>

              <a
                href={`/api/asset/download/${file.id}`}
                className="p-1 hover:bg-zinc-600 rounded text-zinc-400 hover:text-white cursor-pointer"
              >
                <Download size={14} />
              </a>

              <button
                onClick={() => openRename(file)}
                className="p-1 hover:bg-amber-600 rounded text-zinc-400 hover:text-white cursor-pointer transition-colors"
              >
                <Edit size={14} />
              </button>


              <button
                onClick={() => openDelete(file)}
                className="p-1 hover:bg-red-500/80 rounded text-zinc-400 hover:text-white cursor-pointer"
              >
                <Trash2 size={14} />
              </button>

            </div>
          </div>
        ))}
      </div>

      {/* DELETE MODAL */}
      <Modal open={deleteOpen} onClose={() => setDeleteOpen(false)} title="Delete Asset">
        <div className="flex flex-col gap-6">
          <h4 className="text-foreground text-base font-medium">
            Delete {selectedAsset?.fileName}?
          </h4>
          <span className="text-zinc-500 text-sm">
            This action cannot be undone. This asset will be deleted permanently.
          </span>

          <div className="flex flex-col sm:flex-row gap-3 sm:justify-end">
            <button
              onClick={async () => {
                await fetch(`http://localhost:3000/api/asset/delete/${selectedAsset?.id}`, {
                  method: "DELETE",
                  credentials: "include",
                });

                setDeleteOpen(false);
                router.refresh();
              }}
              className="px-4 py-2 cursor-pointer rounded bg-red-600 hover:bg-red-700 text-white text-sm"
            >
              Yes, Delete
            </button>

            <button
              onClick={() => setDeleteOpen(false)}
              className="px-4 py-2 rounded cursor-pointer border border-zinc-700 text-[text-color] hover:bg-zinc-800 text-sm"
            >
              Cancel
            </button>
          </div>
        </div>
      </Modal>

      {/* SHARE MODAL */}
      <Modal open={shareOpen} onClose={() => setShareOpen(false)} title="Share Asset">
        <div className="flex flex-col gap-6">
          <h4 className="text-forground text-base font-medium">
            Start streaming {selectedAsset?.fileName}?
          </h4>
          <span className="text-zinc-500 text-sm">
            You can add users later.
          </span>

          <div className="flex flex-col sm:flex-row gap-3 sm:justify-end">
            <button 
            onClick={async () => {
                try {
                  const res = await fetch(`http://localhost:3000/api/asset/share/${selectedAsset?.id}`, {
                  method: "POST",
                  credentials: "include",
                });

                if (!res.ok) {
                    toast.error("Failed to share the asset");
                    return;
                  }

                  toast.success("Asset shared successfully");
                  setShareOpen(false);
                }catch{
                  toast.error("Server error");
                }
                finally {
                  setShareOpen(false);
                }
              }}
            className="px-4 py-2 rounded cursor-pointer bg-emerald-600 hover:bg-emerald-700 text-white text-sm">
              Yes, Share
            </button>

            <button
              onClick={() => setShareOpen(false)}
              className="px-4 py-2 rounded cursor-pointer border border-zinc-700 text-[text-color] hover:bg-zinc-800 text-sm"
            >
              Cancel
            </button>
          </div>
        </div>
      </Modal>

      {/* rename modal */}
        <RenameModal
            open={renameOpen}
            onClose={()=>setRenameOpen(false)}
            Id={selectedAsset?.id}
            endpoint="http://localhost:3000/api/asset/rename"
        />
    </>
  );
}
