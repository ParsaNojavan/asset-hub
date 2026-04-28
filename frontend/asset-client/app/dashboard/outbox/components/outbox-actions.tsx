"use client";

import { useState } from "react";
import { Plus, Trash2 } from "lucide-react";
import SearchModal from "@/app/dashboard/outbox/components/user-search";
import Modal from "@/app/components/modal";
import toast from "react-hot-toast";
import { useRouter } from "next/navigation";

export default function ActionButtons({assetId} : {assetId : string}) {

  const router = useRouter();
  const [searchOpen, setSearchOpen] = useState(false);
  const [unshareOpen, setUnshareOpen] = useState(false);

  return (
    <>
      <div className="flex flex-col sm:flex-row gap-4">

        <button
          onClick={() => setSearchOpen(true)}
          className="px-6 py-2 cursor-pointer bg-emerald-600 hover:bg-emerald-700 rounded-lg text-white inline-flex items-center gap-2 transition"
        >
          <Plus size={20} />
          Add User
        </button>

        <button
          onClick={() => setUnshareOpen(true)}
          className="px-6 py-2 cursor-pointer bg-red-500 hover:bg-red-600 rounded-lg text-white inline-flex items-center gap-2 transition"
        >
          <Trash2 size={20} />
          Delete Asset
        </button>

      </div>

      <SearchModal assetId={assetId} open={searchOpen} onClose={() => setSearchOpen(false)} />

      <Modal open={unshareOpen} onClose={() => setUnshareOpen(false)} title="Unshare Asset">
        <div className="flex flex-col gap-6">
          
          <h4 className="text-foreground text-base font-medium">
            Are you sure you want to unshare this asset?
          </h4>

          <span className="text-zinc-500 text-sm">
            All shared users will lose access.
          </span>

          <div className="flex justify-end gap-3">
            <button 
            className="px-4 py-2 cursor-pointer rounded-md bg-red-600 text-white"
            onClick={
              async () => {
                const res = await fetch(`/api/share/unshare/${assetId}`,{
                  method : 'DELETE',
                  credentials : 'include'
                });

                setUnshareOpen(false);

                router.push('/dashboard/outbox');

                toast.success('unshared asset successfully')
              }
            }
            >
              Yes, unshare
            </button>
            <button
              onClick={() => setUnshareOpen(false)}
              className="px-4 py-2 cursor-pointer hover:bg-zinc-800 hover:text-white rounded-md border border-zinc-700 text-[text-color]"
            >
              Cancel
            </button>
          </div>

        </div>
      </Modal>
    </>
  );
}
