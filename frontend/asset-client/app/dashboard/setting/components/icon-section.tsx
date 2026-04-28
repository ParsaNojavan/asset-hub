"use client";

import { AppWindow, Monitor, Apple } from "lucide-react";
import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";

const ICON_PACKS = [
  { id: 'Windows', icon: <AppWindow size={18}/>, label: 'Windows 11' },
  // { id: 'MacOS', icon: <Apple size={18}/>, label: 'macOS Monterey' },
  // { id: 'Linux', icon: <Monitor size={18}/>, label: 'Linux (Papirus)' },
];

export default function IconSection() {
  const router = useRouter();
  const [activePack, setActivePack] = useState<string>("Windows");

  useEffect(() => {
    const savedPack = document.cookie
      .split("; ")
      .find((row) => row.startsWith("iconPack="))
      ?.split("=")[1];
    
    if (savedPack) {
      setActivePack(savedPack);
    }
  }, []);

  const handleSelect = (id: string) => {
    setActivePack(id);
    
    document.cookie = `iconPack=${id}; path=/; max-age=31536000; SameSite=Lax`;
    
    router.refresh();
  };

  return (
    <section className="border-t border-zinc-800 pt-12 grid grid-cols-1 md:grid-cols-3 gap-8">

      <div className="space-y-1">
        <h2 className="text-lg font-medium text-foreground">Icon Pack</h2>
        <p className="text-sm text-zinc-500">
          Swap the default icons with platform‑styled icon packs.
        </p>
      </div>

      <div className="md:col-span-2 space-y-6">
        <label className="text-sm font-medium text-[small-text-color]">Style</label>
        <p className="text-xs text-zinc-500 mb-2">
          Choose between different OS‑inspired icon appearances.
        </p>

        <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
          {ICON_PACKS.map((pack) => {
            const isActive = activePack === pack.id;
            
            return (
              <button
                key={pack.id}
                onClick={() => handleSelect(pack.id)}
                className={`
                  flex cursor-pointer flex-col items-center gap-3 p-4 border rounded-lg transition group
                  ${isActive 
                    ? "border-emerald-500 bg-emerald-500/10 ring-1 ring-emerald-500/50" 
                    : "border-zinc-800 hover:border-zinc-700 bg-zinc-900/50"
                  }
                `}
              >
                <div className={`
                  transition-colors
                  ${isActive ? "text-emerald-500" : "text-[icon-color] group-hover:text-emerald-500"}
                `}>
                  {pack.icon}
                </div>
                <span className={`
                  text-sm font-medium
                  ${isActive ? "text-emerald-400" : "text-[small-text-color] group-hover:text-emerald-500"}
                `}>
                  {pack.label}
                </span>
              </button>
            );
          })}
        </div>
      </div>
    </section>
  );
}
