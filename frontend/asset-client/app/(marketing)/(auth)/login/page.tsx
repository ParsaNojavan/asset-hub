'use client'

import { useState } from "react";
import { useAuthStore } from "@/app/stores/auth-store";
import toast from "react-hot-toast";
import Link from "next/link";

export default function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const { setUser } = useAuthStore();

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setLoading(true);

    try {
      const res = await fetch("/api/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });

      const data = await res.json();

      if (!res.ok) {
        toast.error(data.message || "Login failed");
        return;
      }

      setUser(data.user);

      toast.success("Login successful!");

      // Redirect after 1 sec
      setTimeout(() => {
        window.location.href = "/dashboard";
      }, 1000);

    } catch {
      toast.error("Server error");
    } finally {
      setLoading(false);
    }
  }

  return (
    <section className="relative flex flex-col items-center justify-center px-6 py-32">

      <div className="absolute top-0 left-1/2 -translate-x-1/2 w-[800px] h-[600px] bg-emerald-500/10 blur-[140px] rounded-full pointer-events-none" />

      <div className="relative z-10 w-full max-w-lg bg-zinc-900/70 backdrop-blur-xl rounded-2xl border border-zinc-800 p-10 shadow-xl">

        <h1 className="text-4xl font-bold text-center">
          Login to your <span className="text-emerald-500">account</span>
        </h1>
        <p className="text-zinc-400 text-center mt-3 mb-10">
          Login to your account to access your dashboard
        </p>

        <form className="space-y-6" onSubmit={handleSubmit}>

          <div>
            <label className="text-sm text-zinc-400">Email Address</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="johndoe@email"
              required
              className="mt-2 w-full bg-zinc-950 border border-zinc-800 hover:border-zinc-700 rounded-lg px-4 py-3 focus:outline-none focus:border-emerald-500 transition-colors"
            />
          </div>

          <div>
            <label className="text-sm text-zinc-400">Password</label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Password"
              required
              className="mt-2 w-full bg-zinc-950 border border-zinc-800 hover:border-zinc-700 rounded-lg px-4 py-3 focus:outline-none focus:border-emerald-500 transition-colors"
            />
          </div>

          <p className="inline-flex gap-2 text-sm text-zinc-400">don't have an account ? 
            <Link href='/register'
            className="text-emerald-500 underline">create</Link>
          </p>

          <button
            type="submit"
            disabled={loading}
            className="
              w-full 
              bg-emerald-500 
              hover:bg-emerald-600 
              disabled:bg-emerald-600/50
              disabled:cursor-not-allowed
              text-white 
              py-3 
              rounded-lg 
              transition 
              duration-200 
              cursor-pointer
            "
          >
            {loading ? "Loading..." : "Continue"}
          </button>

        </form>

      </div>
    </section>
  );
}
