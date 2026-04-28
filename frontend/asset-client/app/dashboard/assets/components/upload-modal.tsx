"use client";

import { useState, useRef, DragEvent, ChangeEvent } from "react";
import Modal from "@/app/components/modal";
import { UploadCloud, File, X, Loader2 } from "lucide-react";
import toast from "react-hot-toast";
import { useRouter } from "next/navigation";

interface UploadModalProps {
  open: boolean;
  onClose: () => void;
  folderId?: string | null;
  folderPath: string;
}

export default function UploadModal({ open, onClose, folderId,folderPath }: UploadModalProps) {
  const [file, setFile] = useState<File | null>(null);
  const [isUploading, setIsUploading] = useState(false);
  const inputRef = useRef<HTMLInputElement | null>(null);
  const router = useRouter();

  const handleClose = () => {
    setFile(null);
    setIsUploading(false);
    onClose();
  };

  const handleBrowse = (e: ChangeEvent<HTMLInputElement>) => {
    const f = e.target.files?.[0] || null;
    setFile(f);
  };

  const handleDrop = (e: DragEvent<HTMLDivElement>) => {
    e.preventDefault();
    const f = e.dataTransfer.files?.[0] || null;
    setFile(f);
  };

  const handleUpload = async () => {
    if (!file) return;
    setIsUploading(true);

    const safeFolderPath = folderPath ?? "";

    const fullPath =
  safeFolderPath === "" 
    ? file.name 
    : safeFolderPath.endsWith("/")
        ? safeFolderPath + file.name
        : safeFolderPath + "/" + file.name;

    try {
      const formData = new FormData();
      formData.append("file", file);
      formData.append("path", fullPath);
      if (folderId) formData.append("folderId", folderId);

      const res = await fetch("/api/asset/upload", {
        method: "POST",
        body: formData,
        credentials: "include"
      });

      if (!res.ok) {
        toast.error("Upload failed");
        handleClose();
        return;
      }

      handleClose();
      router.refresh(); 

    } catch (err) {
        toast.error("Upload failed");
        handleClose();
        return;
    }

    setIsUploading(false);
  };

  if (!open) return null;

  return (
    <Modal open={open} onClose={handleClose} title="Upload File">
      <div className="flex flex-col gap-6">

        {/* Drop Zone */}
        <div
          onDrop={handleDrop}
          onDragOver={(e) => e.preventDefault()}
          onClick={() => !isUploading && inputRef.current?.click()}
          className="border cursor-pointer border-zinc-700 border-dashed rounded-xl p-10 flex flex-col items-center justify-center text-center bg-container hover:bg-zinc-800/50 transition"
        >
          <UploadCloud size={40} className="text-zinc-400 mb-3" />
          <p className="text-[text-color] text-sm">Drag & Drop your file here</p>
          <p className="text-zinc-600 text-xs mt-1">or click to browse</p>

          <input
            ref={inputRef}
            type="file"
            className="hidden"
            onChange={handleBrowse}
            disabled={isUploading}
          />
        </div>

        {/* Selected File */}
        {file && (
          <div className="bg-zinc-900 border border-zinc-800 rounded-lg p-4 space-y-3">
            <h4 className="text-sm text-zinc-400">Selected File</h4>

            <div className="flex items-center justify-between p-2 bg-zinc-800 rounded-md">
              <div className="flex items-center gap-3">
                <File size={18} className="text-zinc-400" />
                <span className="text-sm text-zinc-300 truncate max-w-[200px]">
                  {file.name}
                </span>
              </div>

              {!isUploading ? (
                <button
                  onClick={() => setFile(null)}
                  className="text-zinc-500 hover:text-red-500 cursor-pointer"
                >
                  <X size={16} />
                </button>
              ) : (
                <Loader2 size={18} className="animate-spin text-zinc-400" />
              )}
            </div>
          </div>
        )}

        {/* Buttons */}
        <div className="flex justify-end gap-3">
          <button
            onClick={handleClose}
            disabled={isUploading}
            className="px-4 py-2 cursor-pointer text-sm rounded-md border border-zinc-700 text-[text-color] hover:bg-zinc-800 hover:text-white"
          >
            Cancel
          </button>

          <button
            onClick={handleUpload}
            disabled={!file || isUploading}
            className="px-4 py-2 cursor-pointer text-sm rounded-md bg-emerald-600 hover:bg-emerald-700 text-white disabled:bg-zinc-700 disabled:text-zinc-500"
          >
            {isUploading ? (
              <Loader2 size={18} className="animate-spin" />
            ) : "Upload"}
          </button>
        </div>
      </div>
    </Modal>
  );
}
