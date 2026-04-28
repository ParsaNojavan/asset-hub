'use client'

import { useState } from "react";
import EditProfileModal from "./edit-profile";
import { useAuthStore } from "@/app/stores/auth-store";
import UserAvatar from "@/app/components/user-avatar";

const EditProfile = () => {

  const [editOpen, setEditOpen] = useState(false);
  const user = useAuthStore((s) => s.user);

    return (

        <><section className="grid grid-cols-1 md:grid-cols-3 gap-8">

        {/* Left Label */}
        <div className="space-y-1">
          <h2 className="text-lg font-medium text-foreground">Profile</h2>
          <p className="text-sm text-zinc-500">
            Update your public profile information and personal details.
          </p>
        </div>

        {/* Right Card */}
        <div className="md:col-span-2">

          <div className="flex flex-col sm:flex-row items-center sm:items-start gap-4 bg-container border border-zinc-800 rounded-xl p-6">

            <UserAvatar size={16} urlPath=""/>

            <div className="flex flex-col text-center sm:text-left">
              <span className="text-foreground font-medium">{user?.userName}</span>
              <span className="text-zinc-500 text-sm">
                {user?.email}
              </span>
            </div>

            <button
              onClick={() => setEditOpen(true)}
              className="sm:ml-auto px-4 py-2 my-auto cursor-pointer text-sm hover:text-white border border-zinc-700 rounded-md hover:bg-zinc-800 transition w-full sm:w-auto"
            >
              Edit Profile
            </button>

          </div>

        </div>

      </section>
      <EditProfileModal open={editOpen} onClose={() => setEditOpen(false)} />
      </>
    );
}

export default EditProfile;