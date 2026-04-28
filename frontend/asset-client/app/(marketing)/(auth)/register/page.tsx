'use client'

import { useState } from "react";
import { useRouter } from "next/navigation";
import toast from "react-hot-toast";
import { useAuthStore } from "@/app/stores/auth-store";
import Link from "next/link";

export default function RegisterPage() {
  const router = useRouter();

  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const { setUser } = useAuthStore();

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();

    if (password !== confirmPassword) {
      toast.error("Passwords do not match");
      return;
    }

    setLoading(true);

    try {
      const res = await fetch("/api/auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          username,
          email,
          password,
        }),
      });

      const data = await res.json();

      if (!res.ok) {
        toast.error(data.message || "Registration failed");
        return;
      }

      setUser(data.user);

      toast.success("Account created successfully!");
      
      setTimeout(() => {
        router.push("/dashboard");
      }, 1000);

    } catch {
      toast.error("Something went wrong");
    } finally {
      setLoading(false);
    }
  }

  return (
    <section className="relative flex flex-col items-center justify-center px-6 py-32">

      <div className="absolute top-0 left-1/2 -translate-x-1/2 w-[800px] h-[600px] bg-emerald-500/10 blur-[140px] rounded-full pointer-events-none" />

      <div className="relative z-10 w-full max-w-lg bg-zinc-900/70 backdrop-blur-xl rounded-2xl border border-zinc-800 p-10 shadow-xl">
        
        <h1 className="text-4xl font-bold text-center">
          Join our <span className="text-emerald-500">family</span>
        </h1>

        <p className="text-zinc-400 text-center mt-3 mb-10">
          Create an account and join us
        </p>

        <form className="space-y-6" onSubmit={handleSubmit}>
          
          <div>
            <label className="text-sm text-zinc-400">User Name</label>
            <input
              type="text"
              placeholder="Username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
              className="mt-2 w-full bg-zinc-950 border border-zinc-800 hover:border-zinc-700 rounded-lg px-4 py-3 focus:outline-none focus:border-emerald-500 transition-colors"
            />
          </div>

          <div>
            <label className="text-sm text-zinc-400">Email Address</label>
            <input
              type="email"
              placeholder="johndoe@email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              className="mt-2 w-full bg-zinc-950 border border-zinc-800 hover:border-zinc-700 rounded-lg px-4 py-3 focus:outline-none focus:border-emerald-500 transition-colors"
            />
          </div>

          <div>
            <label className="text-sm text-zinc-400">Password</label>
            <input
              type="password"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              className="mt-2 w-full bg-zinc-950 border border-zinc-800 hover:border-zinc-700 rounded-lg px-4 py-3 focus:outline-none focus:border-emerald-500 transition-colors"
            />
          </div>

          <div>
            <label className="text-sm text-zinc-400">Confirm Password</label>
            <input
              type="password"
              placeholder="Confirm Password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
              className="mt-2 w-full bg-zinc-950 border border-zinc-800 hover:border-zinc-700 rounded-lg px-4 py-3 focus:outline-none focus:border-emerald-500 transition-colors"
            />
          </div>
        
          <p className="inline-flex gap-2 text-sm text-zinc-400">already have an account ? 
            <Link href='/login'
            className="text-emerald-500 underline">login</Link>
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
            {loading ? "Creating account..." : "Register"}
          </button>

        </form>
      </div>
    </section>
  );
}
