import Link from "next/link"
import { cookies } from "next/headers";
import jwt from "jsonwebtoken";
import UserAvatar from "./user-avatar";


export default async function Navbar() {

  const cookieStore = await cookies();
  const token = cookieStore.get("accessToken")?.value;

  let user: null | { id: string; username: string; avatar?: string } = null;


  if (token) {
    try {
      const decoded: any = jwt.decode(token);

      if (decoded && decoded.exp * 1000 > Date.now()) {
        user = {
          id: decoded.sub,
          username: decoded.username,
          avatar: decoded.avatar || null,
        };
      }
    } catch {
    }
  }

  return (
    <header className="w-full border-b border-zinc-800 bg-zinc-950 sticky z-50 top-0">
      <div className="mx-auto flex h-16 max-w-7xl items-center justify-between px-6">

        <div className="flex items-center gap-6">
          <Link href="/"
              className="text-lg font-semibold text-white">
                <div className="flex items-center gap-2">
                  <img width={"50px"} src={"/logo.png"}/>
                  <span>AssetHub</span>
                </div>
          </Link>
          

          <nav className="hidden md:flex items-center gap-6 text-sm text-zinc-400">
            <div className="relative group">
                <Link
                    href="/"
                    className="hover:text-white transition flex items-center gap-1"
                >
                    Home
                    <span className="text-zinc-500 group-hover:text-white transition">▾</span>
                </Link>

                <div
                    className="
                    absolute left-0 top-full mt-2 w-40
                    rounded-lg border border-zinc-800 bg-zinc-900 shadow-lg
                    opacity-0 invisible group-hover:opacity-100 group-hover:visible
                    transition-all duration-150
                    "
                >
                    <Link
                    href="/#features"
                    className="block px-4 py-2 text-sm text-zinc-300 hover:bg-zinc-800 hover:text-white transition"
                    >
                    Features
                    </Link>

                    <Link
                    href="/#pricing"
                    className="block px-4 py-2 text-sm text-zinc-300 hover:bg-zinc-800 hover:text-white transition"
                    >
                    Pricing
                    </Link>
                </div>
            </div>

            <Link href="/about" className="hover:text-white transition">
              About
            </Link>

            <Link href="/contact" className="hover:text-white transition">
              Contact
            </Link>
          </nav>
        </div>

        <div className="flex items-center gap-4">
          {!user ? 
          (<>
          <Link
            href="/login"
            className="text-sm text-zinc-400 hover:text-white transition"
          >
            Sign in
          </Link>

          <Link
            href="/register"
            className="rounded-lg bg-emerald-500  px-4 py-2 text-sm font-medium text-white hover:bg-emerald-600 transition"
          >
            Get Started
          </Link>
          </>):
          (<UserAvatar size={12} urlPath="/dashboard"/>)}
        </div>

      </div>
    </header>
  )
}
