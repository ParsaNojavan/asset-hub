"use client";

import { useEffect, useState } from "react";
import { FileText, BarChart3, Users } from "lucide-react";

export default function DashboardHome() {
  const [loaded, setLoaded] = useState(false);

  useEffect(() => {
    setLoaded(true);
  }, []);

  return (
    <div
      className={`flex justify-center transition-all duration-700 ${loaded ? "opacity-100 translate-y-0" : "opacity-0 translate-y-6"
        }`}
    >
      <div className="p-8 max-w-6xl w-full text-center">

        {/* Title */}
        <h1 className="text-3xl font-bold text-foreground mb-3">
          Welcome to AssetHub
        </h1>

        <p className="text-zinc-400 max-w-2xl mx-auto">
          Manage your assets, generate powerful reports, and keep everything
          organized in one place. Use the tools below to get started quickly.
        </p>

        {/* Features */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mt-12">

          {/* Card */}
          <div className="bg-container/50 border border-zinc-800 rounded-xl p-6 transition-all duration-300 delay-150 hover:border-emerald-500 hover:bg-container/80">

            <div className="flex flex-col items-center gap-4 mb-4">
              <div className="p-3 bg-emerald-500/10 rounded-lg">
                <FileText className="text-emerald-400" size={26} />
              </div>

              <h2 className="text-lg font-semibold text-forground">
                Asset Management
              </h2>
            </div>

            <p className="text-zinc-400 text-sm leading-relaxed">
              Easily add, edit, and organize all your assets with a fast and
              intuitive interface.
            </p>

          </div>

          {/* Card */}
          <div className="bg-container/50 border border-zinc-800 rounded-xl p-6 transition-all duration-300 delay-150 hover:border-emerald-500 hover:bg-container/80">

            <div className="flex flex-col items-center gap-4 mb-4">
              <div className="p-3 bg-emerald-500/10 rounded-lg">
                <BarChart3 className="text-emerald-400" size={26} />
              </div>

              <h2 className="text-lg font-semibold text-foreground">
                Analytics & Reports
              </h2>
            </div>

            <p className="text-zinc-400 text-sm leading-relaxed">
              Get clear insights into your assets with powerful analytics and
              easy‑to‑read reports.
            </p>

          </div>

          {/* Card */}
          <div className="bg-container/50 border border-zinc-800 rounded-xl p-6 transition-all duration-300 delay-150 hover:border-emerald-500 hover:bg-container/80">

            <div className="flex flex-col items-center gap-4 mb-4">
              <div className="p-3 bg-emerald-500/10 rounded-lg">
                <Users className="text-emerald-400" size={26} />
              </div>

              <h2 className="text-lg font-semibold text-foreground">
                Team Collaboration
              </h2>
            </div>

            <p className="text-zinc-400 text-sm leading-relaxed">
              Collaborate with your team, manage permissions, and keep everyone
              aligned with shared asset data.
            </p>

          </div>

        </div>
      </div>
    </div>
  );
}
