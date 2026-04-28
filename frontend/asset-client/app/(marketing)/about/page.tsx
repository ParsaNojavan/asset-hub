import { Metadata } from "next";
import { ShieldCheck, FolderKanban, Share2, Mail } from "lucide-react";

export const metadata: Metadata = {
  title: "About",
  description: "About AssetHub platform",
};

export default function AboutPage() {
  return (
    <section className="relative mx-auto max-w-6xl px-6 py-28">

      {/* glow */}
      <div className="absolute right-1/2 top-0 w-[700px] h-[500px] bg-emerald-500/10 blur-[140px] rounded-full pointer-events-none" />

      {/* header */}
      <div className="text-center">
        <h1 className="text-5xl font-bold tracking-tight">
          About <span className="text-emerald-500">AssetHub</span>
        </h1>

        <p className="mt-6 text-zinc-400 max-w-2xl mx-auto">
          AssetHub is a modern platform for organizing, managing and sharing
          digital assets. Built for developers, teams and businesses that want
          full control over their files and data.
        </p>
      </div>

      {/* features */}
      <div className="grid md:grid-cols-3 gap-8 mt-20">
        <div className="p-6 rounded-xl border border-zinc-800 bg-zinc-900/50">
          <FolderKanban className="text-emerald-500 mb-4" size={28} />
          <h3 className="text-lg font-medium">Smart Organization</h3>
          <p className="text-zinc-400 text-sm mt-2">
            Organize your assets into folders and structures that scale with your projects.
          </p>
        </div>

        <div className="p-6 rounded-xl border border-zinc-800 bg-zinc-900/50">
          <Share2 className="text-emerald-500 mb-4" size={28} />
          <h3 className="text-lg font-medium">Easy Sharing</h3>
          <p className="text-zinc-400 text-sm mt-2">
            Share assets with teammates or clients instantly with secure access controls.
          </p>
        </div>

        <div className="p-6 rounded-xl border border-zinc-800 bg-zinc-900/50">
          <ShieldCheck className="text-emerald-500 mb-4" size={28} />
          <h3 className="text-lg font-medium">Security First</h3>
          <p className="text-zinc-400 text-sm mt-2">
            Built with security in mind using modern authentication and protected storage.
          </p>
        </div>
      </div>

      {/* developer section */}
      <div className="mt-32">
        <h2 className="text-3xl font-bold text-center">
          Developer <span className="text-emerald-500">Info</span>
        </h2>

        <p className="text-center text-zinc-400 mt-4 max-w-2xl mx-auto">
          AssetHub is being developed and maintained by Parsa Nojavan .
        </p>

        <div className="mt-14 flex flex-col items-center text-center">

          {/* profile image */}
          <img
            src="profile.jpg"
            alt="Developer"
            className="w-32 h-32 rounded-full object-cover border border-zinc-700 shadow-lg"
          />

          {/* name */}
          <h3 className="mt-6 text-xl font-semibold">Parsa Nojavan</h3>

          {/* description */}
          <p className="mt-3 text-zinc-400 max-w-lg">
           Computer Engineering student in Sahand university of technology,backend developer
           and fan of building of API's,Modern Applications,and scalable systems .
          </p>

          {/* email */}
          <div className="mt-6 flex items-center gap-4">

            {/* Email button */}
            <a
              href="mailto:parsa.nojavan85@gmail.com"
              className="
                px-5 py-2 rounded-lg text-sm font-medium
                bg-emerald-500 text-white
                hover:bg-emerald-600
                transition
                shadow
              "
            >
              Email Me
            </a>

            {/* GitHub button */}
            <a
              href="https://github.com/parsanojavan" 
              target="_blank"
              className="
                px-5 py-2 rounded-lg text-sm font-medium
                border border-emerald-500
                text-emerald-400
                hover:bg-emerald-600 hover:text-white
                transition
              "
            >
              GitHub
            </a>

          </div>

        </div>
      </div>

    </section>
  );
}
