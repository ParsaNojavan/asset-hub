"use client";

import { useState } from "react";
import Modal from "@/app/components/modal";
import { toast, Toaster } from "react-hot-toast";

interface ModalProps {
  open: boolean;
  onClose: () => void;
}

const ChangePassword = ({ open, onClose }: ModalProps) => {
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async () => {
    if (!currentPassword || !newPassword || !confirmPassword) {
      toast.error("Please fill all fields");
      return;
    }

    if (newPassword !== confirmPassword) {
      toast.error("Passwords do not match");
      return;
    }

    setLoading(true);

    try {
      const res = await fetch("/api/user/change-password", {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          currentPassword,
          newPassword,
        }),
      });

      if (!res.ok) {
        const text = await res.json();
        toast.error(text.Message || "Failed to change password");
        setLoading(false);
        return;
      }

      toast.success("Password updated successfully");

      // reset + close
      setCurrentPassword("");
      setNewPassword("");
      setConfirmPassword("");
      setLoading(false);
      onClose();

    } catch (error) {
      console.error(error);
      toast.error("Server error");
      setLoading(false);
    }
  };

  return (
    <>
      <Modal open={open} onClose={onClose} title="Change Password">
        <div className="space-y-4">

          <div>
            <label className="text-sm text-zinc-400">
              Current Password
            </label>
            <input
              type="password"
              value={currentPassword}
              onChange={(e) => setCurrentPassword(e.target.value)}
              className="w-full mt-1 text-foreground bg-container border border-zinc-800 rounded-md px-3 py-2"
            />
          </div>

          <div>
            <label className="text-sm text-zinc-400">
              New Password
            </label>
            <input
              type="password"
              value={newPassword}
              onChange={(e) => setNewPassword(e.target.value)}
              className="w-full mt-1 text-foreground bg-container border border-zinc-800 rounded-md px-3 py-2"
            />
          </div>

          <div>
            <label className="text-sm text-zinc-400">
              Confirm Password
            </label>
            <input
              type="password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              className="w-full mt-1 text-foreground bg-container border border-zinc-800 rounded-md px-3 py-2"
            />
          </div>

          <button
            onClick={handleSubmit}
            disabled={loading}
            className="w-full text-white bg-emerald-600 hover:bg-emerald-500 py-2 rounded-md cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {loading ? "Updating..." : "Update Password"}
          </button>

        </div>
      </Modal>

      {/* toast inside modal portal */}
      <Toaster
         position="top-center"
           toastOptions={{
              style: {
                background: "#333",
                color: "#fff",
                border: "1px solid #333",
              },
          }}/>
    </>
  );
};

export default ChangePassword;
