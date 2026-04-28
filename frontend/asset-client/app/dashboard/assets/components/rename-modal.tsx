"use client";

import { useState } from "react";
import Modal from "@/app/components/modal";
import { Loader2 } from "lucide-react";
import toast from "react-hot-toast";
import { useRouter } from "next/navigation";

interface RenameModalProps {
  open: boolean;
  onClose: () => void;
  Id?: string | null;
  endpoint: string;
}

export default function RenameModal({ open, onClose, Id, endpoint }: RenameModalProps) {
  const [name, setName] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const router = useRouter();

  const handleClose = () => {
    setName("");
    setIsLoading(false);
    onClose();
  };

  const handleRename = async () => {
    if (!name.trim()) {
      toast.error("Name is required");
      return;
    }

    setIsLoading(true);

    const payload: any = { name: name.trim() };

    if (Id) {
      if (endpoint.includes("folders")) {
        payload.FolderId = Id;
      }

      if (endpoint.includes("asset")) {
        payload.FileId = Id;
      }
    }

    try {
      const res = await fetch(endpoint, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      });

      if (!res.ok) {
        toast.error("Rename failed");
        setIsLoading(false);
        return;
      }

      toast.success("Renamed successfully");
      handleClose();
      router.refresh();
    } catch {
      toast.error("Server error");
      setIsLoading(false);
    }
  };

  if (!open) return null;

  return (
    <Modal open={open} onClose={handleClose} title="Rename">
      <div className="flex flex-col gap-6">

        <div className="flex flex-col gap-2">
          <label className="text-sm text-zinc-400">New Name</label>
          <input
            type="text"
            disabled={isLoading}
            value={name}
            onChange={(e) => setName(e.target.value)}
            placeholder="Enter new name"
            className="
              bg-container border border-zinc-700 rounded-lg px-3 py-2 
              text-[text-color] placeholder-placeholder-color
              outline-none focus:border-emerald-600 transition
            "
          />
        </div>

        <div className="flex justify-end gap-3 mt-4">
          <button
            onClick={handleClose}
            disabled={isLoading}
            className="px-4 py-2 text-sm cursor-pointer border border-zinc-700 rounded-md text-[text-color] hover:bg-zinc-800 hover:text-white"
          >
            Cancel
          </button>

          <button
            onClick={handleRename}
            disabled={isLoading || !name.trim()}
            className="
              px-4 py-2 text-sm cursor-pointer rounded-md bg-emerald-600 hover:bg-emerald-700 
              text-white flex items-center gap-2
              disabled:bg-zinc-700 disabled:text-zinc-500
            "
          >
            {isLoading ? (
              <Loader2 size={18} className="animate-spin" />
            ) : (
              "Rename"
            )}
          </button>
        </div>

      </div>
    </Modal>
  );
}
