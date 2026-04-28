"use client";

import Modal from "@/app/components/modal";
import { useState } from "react";
import { useRouter } from "next/navigation";
import { toast } from "react-hot-toast";

interface ModalProps {
  open: boolean;
  onClose: () => void;
}

export default function DeleteAccount({ open, onClose }: ModalProps) {
  const router = useRouter();
  const [loading, setLoading] = useState(false);

  const handleDelete = async () => {
    try {
      setLoading(true);

      const res = await fetch("/api/user/delete-account", {
        method: "DELETE",
      });

      if (!res.ok) {
        const text = await res.text();
        let error = text;

        try {
          const json = JSON.parse(text);
          error = json.message || text;
        } catch {}

        toast.error(error);
        return;
      }

      toast.success("Account deleted successfully");

      router.replace("/login");

    } catch (err) {
      toast.error("Failed to delete account");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal open={open} onClose={onClose} title="Delete Account">
      <div className="flex flex-col gap-6">

        <div className="flex flex-col gap-1">
          <h4 className="text-white text-base font-medium">
            Are you sure you want to delete your account?
          </h4>
          <span className="text-zinc-500 text-sm">
            This action is permanent and cannot be undone. All your assets, data,
            and settings will be permanently removed.
          </span>
        </div>

        <div className="flex flex-col sm:flex-row sm:justify-end gap-3">

          <button
            disabled={loading}
            onClick={handleDelete}
            className="px-4 py-2 rounded-md bg-red-600 text-white text-sm font-medium 
            hover:bg-red-700 transition disabled:opacity-60 cursor-pointer"
          >
            {loading ? "Deleting..." : "Yes, delete my account"}
          </button>

          <button
            disabled={loading}
            onClick={onClose}
            className="px-4 py-2 rounded-md border border-zinc-700 text-[small-text-color] text-sm 
            hover:bg-zinc-800 hover:text-white cursor-pointer transition"
          >
            Cancel
          </button>

        </div>
      </div>
    </Modal>
  );
}
