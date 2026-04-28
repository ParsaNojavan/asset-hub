'use client'

import { useState } from "react"
import Link from "next/link"
import { Folder, Share2, Inbox, Settings, Menu, X, LogOut } from "lucide-react"
import { usePathname, useRouter } from "next/navigation"
import UserAvatar from "./user-avatar"
import { useAuthStore } from "../stores/auth-store"

const menuItems = [
  { name: "My Assets", icon: Folder, path: "/dashboard/assets" },
  { name: "Outbox", icon: Share2, path: "/dashboard/outbox" },
  { name: "Inbox", icon: Inbox, path: "/dashboard/inbox" },
  { name: "Settings", icon: Settings, path: "/dashboard/setting" },
]

export default function MobileSidebar() {

  const router = useRouter();
  const pathname = usePathname();
  const [open, setOpen] = useState(false);

  const user = useAuthStore((s) => s.user);

  return (
    <>
      {/* Hamburger */}
      <button
        onClick={() => setOpen(true)}
        className="md:hidden fixed top-4 right-4 z-50 p-3 bg-container border border-zinc-800 rounded-lg text-foreground"
      >
        <Menu size={22} />
      </button>

      {/* Backdrop */}
      {open && (
        <div
          onClick={() => setOpen(false)}
          className="fixed inset-0 bg-black/50 backdrop-blur-sm z-40 md:hidden"
        />
      )}

      {/* Drawer */}
      <aside
        className={`
        fixed top-0 left-0 h-screen w-72 bg-background border-r border-zinc-800
        transform transition-transform duration-300 ease-in-out z-50 md:hidden
        ${open ? "translate-x-0" : "-translate-x-full"}
        `}
      >
        {/* Header */}
        <div className="flex items-center justify-between h-20 px-4 border-b border-zinc-900">
          <div className="flex items-center gap-3">
            <img src="/logo.png" width={36} />
            <span className="text-foreground font-bold text-lg">AssetHub</span>
          </div>

          <button
            onClick={() => setOpen(false)}
            className="text-zinc-400 hover:text-foreground"
          >
            <X size={20} />
          </button>
        </div>

        {/* Menu */}
        <nav className="flex flex-col gap-2 p-3">
          {menuItems.map((item) => {

            const isActive = pathname === item.path;

            return(
            <Link
              key={item.path}
              href={item.path}
              onClick={() => setOpen(false)}
              className={`flex items-center gap-3 p-3 rounded-lg transition-colors
                ${isActive
            ? "bg-emerald-600 text-white"
            : "text-zinc-400 hover:bg-container hover:text-emerald-500"
          }`}
            >
              <item.icon size={20} />
              <span className="text-sm font-medium">{item.name}</span>
            </Link>
          )})}
           <button
        onClick={
          async () => {
        await fetch("/api/auth/logout", {
          method: "POST",
          credentials: "include",
        });
        router.refresh();
      }
        }
        className={`flex items-center p-2.5 rounded-lg transition-all cursor-pointer group
          text-zinc-400 hover:bg-container hover:text-red-500`}
      >
        <div className="min-w-[32px] flex justify-center">
          <LogOut size={20} className="shrink-0" />
        </div>

        <span
          className={`
            ml-3 text-sm font-medium transition-all duration-300
            whitespace-nowrap
          `}
        >
          Logout
        </span>
      </button>
        </nav>

        {/* Footer */}
        <div className="flex items-center row gap-20 absolute bottom-0 w-full p-4 border-t border-zinc-900">
          <div className="flex items-center gap-3">
            <UserAvatar size={8} urlPath="/dashboard/setting"/>
            <div>
              <p className="text-sm text-foreground font-medium">{user?.userName}</p>
              <p className="text-xs text-zinc-500">Free Plan</p>
            </div>
          </div>
          
        </div>
      </aside>
    </>
  )
}
