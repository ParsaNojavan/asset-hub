'use client'

import { useState } from "react";
import DeleteAccount from "./delete-account";

export default function DangerZone () {

    const [deleteOpen, setDeleteOpen] = useState(false);

    return (
        <><section className="border-t border-zinc-800 pt-12 grid grid-cols-1 md:grid-cols-3 gap-8">

        <div className="space-y-1">
          <h2 className="text-lg font-medium text-red-500">Danger Zone</h2>
          <p className="text-sm text-zinc-500">
            Deleting your account is permanent and cannot be undone.
          </p>
        </div>

        <div className="md:col-span-2">
          <button
            onClick={() => setDeleteOpen(true)}
            className="px-4 cursor-pointer py-2 border border-red-600 text-red-500 rounded-md hover:bg-red-600/10 transition w-full sm:w-auto">
            Delete Account
          </button>
        </div>

      </section>
      <DeleteAccount open={deleteOpen} onClose={() => setDeleteOpen(false)} />
        </>
                
    );
}