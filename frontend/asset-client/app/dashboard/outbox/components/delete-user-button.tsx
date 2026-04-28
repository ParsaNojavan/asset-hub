"use client";

import { useState } from "react";
import { Trash2 } from "lucide-react";
import Modal from "@/app/components/modal";
import toast from "react-hot-toast";
import { useRouter } from "next/navigation";

export default function DeleteUserButton({userId,shareId} : {userId : string,shareId:string}) {
  const [open, setOpen] = useState(false);
  const router = useRouter();

  return (
    <>
    <div className="text-right">
       <button
        onClick={() => setOpen(true)}
        className="p-1 cursor-pointer hover:bg-red-500/80 rounded text-[icon-color] hover:text-white"
      >
        <Trash2 size={20} />
      </button>
    </div>


      <Modal open={open} onClose={() => setOpen(false)} title="Delete User">
        <div className="flex flex-col gap-6">
          
          <h4 className="text-foreground text-base font-medium">
            Are you sure you want to delete this user?
          </h4>

          <span className="text-zinc-500 text-sm">
            this user will no longer see the selected asset.
          </span>

          <div className="flex justify-end gap-3">
            <button 
            className="px-4 py-2 cursor-pointer rounded-md bg-red-600 text-white"
            onClick={async () => {
              const res = await fetch('/api/share/unshare-user',{
                headers: {
                  "Content-Type": "application/json",
                },
                method : 'DELETE',
                body : JSON.stringify({
                  reciverId: userId,
                  shareId: shareId,
                })
              })

              if (res.status === 204) {
                  toast.success("Unshared successfully!");
                  setOpen(false);
                  router.refresh();
                } else {
                  toast.error("Failed to unshare");
                }
            }}
            >
              Yes, unshare
            </button>
            <button
              onClick={() => setOpen(false)}
              className="px-4 py-2 cursor-pointer rounded-md border border-zinc-700 hover:bg-zinc-800 hover:text-white text-[text-color]"
            >
              Cancel
            </button>
          </div>

        </div>
      </Modal>
    </>
  );
}
