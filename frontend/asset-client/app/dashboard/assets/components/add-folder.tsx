"use client";

import { useState } from "react";
import AddFolderModal from "./add-folder-modal";
import { FolderPlus, Plus } from "lucide-react";

export default function AddFolder({ folderId }: { folderId?: string | null }) {
  const [addFolder , setAddFolder] = useState(false);

  console.log(folderId)

  return (
    <>
      <button 
      onClick={() => setAddFolder(true)}
      className="inline-flex justify-center text-white bg-emerald-500 hover:bg-emerald-700 transition cursor-pointer rounded-lg py-2 px-3 w-full md:w-auto text-center"
      ><FolderPlus/></button>

      <AddFolderModal
        open={addFolder}
        onClose={() => setAddFolder(false)}
        parentFolderId={folderId}
   />
    </>
  );
}
