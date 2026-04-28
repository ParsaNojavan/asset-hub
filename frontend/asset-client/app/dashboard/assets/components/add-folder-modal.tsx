"use client";

import { useState } from "react";
import Modal from "@/app/components/modal";
import { FolderPlus, Loader2 } from "lucide-react";
import toast from "react-hot-toast";
import { useRouter } from "next/navigation";

interface AddFolderModalProps {
  open: boolean;
  onClose: () => void;
  parentFolderId?: string | null; 
}

export default function AddFolderModal({ open, onClose, parentFolderId }: AddFolderModalProps) {
  const [name, setName] = useState("");
  const [isCreating, setIsCreating] = useState(false);
  const router = useRouter();

  const handleClose = () => {
    setName("");
    setIsCreating(false);
    onClose();
  };

  const handleCreate = async () => {
  if (!name.trim()) {
    toast.error("Folder name is required");
    return;
  }

  setIsCreating(true);

  const payload: any = { name: name.trim() };

  console.log(parentFolderId)

  if (parentFolderId) {
    payload.ParentFolderId = parentFolderId;
  }

  try {
    const res = await fetch("/api/folders/create", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload)
    });

    if (!res.ok) {
      toast.error("Failed to create folder");
      setIsCreating(false);
      return;
    }

    toast.success("Folder created successfully");
    handleClose();
    router.refresh();
  } catch (err) {
    toast.error("Server error");
    setIsCreating(false);
  }
};


  if (!open) return null;

  return (
    <Modal open={open} onClose={handleClose} title="Create Folder">
      <div className="flex flex-col gap-6">

        {/* Input */}
        <div className="flex flex-col gap-2">
          <label className="text-sm text-zinc-400">Folder Name</label>
          <input
            type="text"
            disabled={isCreating}
            value={name}
            onChange={(e) => setName(e.target.value)}
            placeholder="My New Folder"
            className="
              bg-[container] border border-zinc-700 rounded-lg px-3 py-2 
              text-[text-color] placeholder-[placeholder-color]
              outline-none focus:border-emerald-600 transition
            "
          />
        </div>

        {/* Buttons */}
        <div className="flex justify-end gap-3 mt-4">
          <button
            onClick={handleClose}
            disabled={isCreating}
            className="px-4 py-2 text-sm cursor-pointer border border-zinc-700 rounded-md text-[text-color] hover:bg-zinc-800 hover:text-white"
          >
            Cancel
          </button>

          <button
            onClick={handleCreate}
            disabled={isCreating || !name.trim()}
            className="
              px-4 py-2 text-sm cursor-pointer rounded-md bg-emerald-600 hover:bg-emerald-700 
              text-white flex items-center gap-2
              disabled:bg-zinc-700 disabled:text-zinc-500
            "
          >
            {isCreating ? (
              <Loader2 size={18} className="animate-spin" />
            ) : (
              <>
                <FolderPlus size={18} />
                Create
              </>
            )}
          </button>
        </div>

      </div>
    </Modal>
  );
}
