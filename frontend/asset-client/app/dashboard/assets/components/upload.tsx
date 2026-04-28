"use client";

import { useState } from "react";
import UploadModal from "./upload-modal";

export default function Upload({ folderId, folderPath }: { folderId?: string | null, folderPath: string }) {
  const [upload, setUpload] = useState(false);

  return (
    <>
      <button 
      onClick={() => setUpload(true)}
      className="bg-emerald-500 text-white hover:bg-emerald-700 transition cursor-pointer rounded-lg py-2 px-6 w-full md:w-auto text-center"
      >Upload</button>

      <UploadModal
        open={upload}
        onClose={() => setUpload(false)}
        folderId={folderId}
        folderPath={folderPath}  
   />
    </>
  );
}
