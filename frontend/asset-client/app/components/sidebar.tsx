'use client'

import { useState } from "react"
import { usePathname, useRouter } from "next/navigation";
import { useAuthStore } from "@/app/stores/auth-store";
import Link from "next/link"
import {
  LayoutDashboard,
  Folder,
  Share2,
  Inbox,
  Settings,
  ChevronLeft,
  ChevronRight,
  Zap,
  LogOut
} from "lucide-react"
import UserAvatar from "./user-avatar";

const menuItems = [
  { name: "My Assets", icon: Folder, path: "/dashboard/assets" },
  { name: "Outbox", icon: Share2, path: "/dashboard/outbox" },
  { name: "Inbox", icon: Inbox, path: "/dashboard/inbox" },
  { name: "Settings", icon: Settings, path: "/dashboard/setting" },
]

export default function Sidebar() {
  const router = useRouter();
  const [isCollapsed, setIsCollapsed] = useState(false)
  const pathname = usePathname();

  const user = useAuthStore((s) => s.user);

  return (
    <aside
      className={`
        hidden sm:flex
        relative flex flex-col h-screen border-r border-zinc-800
        transition-all duration-300 ease-in-out z-40
        ${isCollapsed ? "w-18" : "w-64"}
      `}
    >

      <button
        onClick={() => setIsCollapsed(!isCollapsed)}
        className="
          absolute cursor-pointer -right-3 top-8 flex h-6 w-6 items-center justify-center
          rounded-full border border-zinc-800 bg-container text-zinc-400
          hover:text-foreground transition-colors z-50 shadow-md
        "
      >
        {isCollapsed ? <ChevronRight size={14} /> : <ChevronLeft size={14} />}
      </button>

      {/* Brand Section */}
      <div className="flex h-20 items-center px-4 overflow-hidden">
        <div className="min-w-[40px] flex justify-center">
            <img width={"40px"} src={"/logo.png"}/>
        </div>
        <span
          className={`
            ml-3 font-bold text-lg text-foreground tracking-wide transition-all duration-300
            ${isCollapsed ? "opacity-0 invisible w-0" : "opacity-100 visible w-auto"}
            whitespace-nowrap
          `}
        >
          AssetHub
        </span>
      </div>

      {/* Menu Items */}
      <nav className="flex flex-col gap-2 p-2 overflow-hidden">
  {menuItems.map((item) => {
    const isActive = pathname === item.path;

    return (
      <Link
        key={item.path}
        href={item.path}
        className={`flex items-center p-2.5 rounded-lg transition-all cursor-pointer group
          ${isActive
            ? "bg-emerald-600 text-white"
            : "text-zinc-400 hover:bg-container hover:text-emerald-500"
          }
        `}
      >
        <div className="min-w-[32px] flex justify-center">
          <item.icon size={20} className="shrink-0" />
        </div>

        <span
          className={`
            ml-3 text-sm font-medium transition-all duration-300
            ${isCollapsed ? "opacity-0 invisible w-0" : "opacity-100 visible w-auto"}
            whitespace-nowrap
          `}
        >
          {item.name}
        </span>
      </Link>
    );
  })}
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
            ${isCollapsed ? "opacity-0 invisible w-0" : "opacity-100 visible w-auto"}
            whitespace-nowrap
          `}
        >
          Logout
        </span>
      </button>
</nav>


      {/* Footer / User Profile (Optional) */}
      <div className="mt-auto p-4 border-t border-zinc-900 overflow-hidden">
         <div className="flex items-center">
            <UserAvatar size={8} urlPath="/dashboard/setting"/>

            <div className={`ml-3 transition-all duration-300 ${isCollapsed ? "opacity-0 w-0 hidden" : "opacity-100 w-auto"}`}>
                <p className="text-xs font-medium text-foreground truncate">{user?.userName}</p>
                <p className="text-[10px] text-zinc-500 truncate">Free Plan</p>
            </div>
         </div>
      </div>
    </aside>
  )
}
