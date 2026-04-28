'use client'

import { useState, useRef } from "react";
import Modal from "@/app/components/modal";
import { useAuthStore } from "@/app/stores/auth-store";

interface ModalProps {
  open: boolean;
  onClose: () => void;
}

export default function EditProfileModal({ open, onClose }: ModalProps) {
  const user = useAuthStore((s) => s.user);
  const setUser = useAuthStore((s) => s.setUser);

  const [name, setName] = useState(user?.userName ?? "");
  const [avatarPreview, setAvatarPreview] = useState(`https://localhost:7024/${user?.imgUrl}`);
  const [avatarFile, setAvatarFile] = useState<File | null>(null);
  const [loading, setLoading] = useState(false);

  const fileInputRef = useRef<HTMLInputElement | null>(null);

  function handleAvatarChange(e: React.ChangeEvent<HTMLInputElement>) {
    const file = e.target.files?.[0];
    if (!file) return;

    setAvatarFile(file);

    const previewUrl = URL.createObjectURL(file);
    setAvatarPreview(previewUrl);
  }

  async function handleSave() {
  try {
    setLoading(true);

    const formData = new FormData();
    formData.append("UserName", name);
    
    if (avatarFile) {
      formData.append("Image", avatarFile); 
    }

    const res = await fetch("/api/user/update-profile", {
      method: "PATCH",
      body: formData,
      credentials : 'include'
    });

    const data = await res.json();
    if (!res.ok) throw new Error(data.message || "Update failed");
    
    if (user && name) {
        const updatedUser = { ...user, userName: name };
        setUser(updatedUser);
    }

    onClose();

  } catch (err) {
    alert(err);
  } finally {
    setLoading(false);
  }
}


  return (
    <Modal open={open} onClose={onClose} title="Edit Profile">
      <div className="md:col-span-2 space-y-6">

        {/* Name */}
        <div className="space-y-2">
          <label className="text-sm font-medium text-[small-text-color]">Name</label>
          <input
            type="text"
            placeholder="Your Name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            className="w-full bg-container text-foreground border border-zinc-800 rounded-md px-3 py-2 focus:outline-none focus:ring-2 focus:ring-emerald-500/50 transition"
          />
        </div>

        {/* Avatar */}
        <div className="flex items-center gap-4">

          <div className="w-16 h-16 rounded-full bg-zinc-800 border border-zinc-700 overflow-hidden">
            <img src={avatarPreview} className="w-full h-full object-cover" />
          </div>

          <input
            type="file"
            accept="image/*"
            ref={fileInputRef}
            onChange={handleAvatarChange}
            className="hidden"
          />

          <button
            className="px-3 py-1.5 text-sm font-medium text-white bg-zinc-800 hover:bg-zinc-700 border border-zinc-700 rounded-md transition cursor-pointer"
            onClick={() => fileInputRef.current?.click()}
          >
            Change Avatar
          </button>
        </div>

        {/* Save button */}
        <button
          onClick={handleSave}
          disabled={loading}
          className="bg-emerald-600 hover:bg-emerald-500 text-white px-4 py-2 rounded-md font-medium text-sm transition cursor-pointer"
        >
          {loading ? "Updating…" : "Update Profile"}
        </button>

      </div>
    </Modal>
  );
}
