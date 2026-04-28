"use client";

import { Monitor, Moon, Sun } from "lucide-react";
import { useEffect, useState } from "react";

export default function AppearanceSection() {

  const [theme, setTheme] = useState("system");

  useEffect(() => {
    const saved = localStorage.getItem("theme") || "system";
    applyTheme(saved);
    setTheme(saved);
  }, []);

  function applyTheme(t: string) {
    const html = document.documentElement;

    html.classList.remove("light", "dark", "system");
    html.classList.add(t);

    localStorage.setItem("theme", t);
    setTheme(t);
  }

  const themes = [
    { id: "light", icon: <Sun size={18} />, label: "Light" },
    { id: "dark", icon: <Moon size={18} />, label: "Dark" },
    { id: "system", icon: <Monitor size={18} />, label: "System" },
  ];

  return (
    <section className="border-t border-zinc-800 pt-12 grid grid-cols-1 md:grid-cols-3 gap-8">

      <div className="space-y-1">
        <h2 className="text-lg font-medium text-foreground">Appearance</h2>
        <p className="text-sm text-zinc-500">
          Choose how AssetHub looks on your device.
        </p>
      </div>

      <div className="md:col-span-2 space-y-10">

        {/* Theme Selector */}
        <div className="space-y-3">
          <label className="text-sm font-medium text-[small-text-color]">Theme</label>
          <p className="text-xs text-zinc-500">
            Select your preferred color scheme. You can follow system theme automatically.
          </p>

          <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
            {themes.map((t) => (
              <button
                key={t.id}
                onClick={() => applyTheme(t.id)}
                className={`flex flex-col items-center gap-3 p-4 border rounded-lg transition group cursor-pointer
                ${
                  theme === t.id
                    ? "text-emerald-500 border-emerald-500 bg-emerald-500/10 ring-1 ring-emerald-500/50"
                    : "border-zinc-800 hover:border-zinc-700 bg-zinc-900/50"
                }`}
              >
                <div className="text-[icon-color] group-hover:text-emerald-500 transition">
                  {t.icon}
                </div>
                <span className="text-sm text-[small-text-color] group-hover:text-emerald-500">{t.label}</span>
              </button>
            ))}
          </div>
        </div>
      </div>

    </section>
  );
}
