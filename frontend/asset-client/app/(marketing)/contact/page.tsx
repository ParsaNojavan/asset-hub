"use client";

import { useState } from "react";
import toast from "react-hot-toast";


export default function ContactPage() {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(false);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();

    setLoading(true);

    const res = await fetch("/api/contact", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ name, email, message }),
    });

    const data = await res.json();

    if (!res.ok) {
      toast.error(data.message || "Something went wrong");
      setLoading(false);
      return;
    }

    toast.success("Message received (Mock Mode)");

    setName("");
    setEmail("");
    setMessage("");
    setLoading(false);
  }

  return (
    <section className="relative flex flex-col items-center justify-center px-6 py-32">

      {/* emerald glow */}
      <div className="absolute top-0 left-1/2 w-[800px] h-[600px] bg-emerald-500/10 blur-[140px] rounded-full pointer-events-none" />

      <div className="relative z-10 w-full max-w-lg bg-zinc-900/70 backdrop-blur-xl rounded-2xl border border-zinc-800 p-10 shadow-xl">

        <h1 className="text-4xl font-bold text-center">
          Contact <span className="text-emerald-500">Us</span>
        </h1>

        <p className="text-zinc-400 text-center mt-3 mb-10">
          Send us a message and we’ll get back to you shortly.
        </p>

        <form className="space-y-6" onSubmit={handleSubmit}>

          <div>
            <label className="text-sm text-zinc-400">Your Name</label>
            <input
              type="text"
              placeholder="John Doe"
              value={name}
              onChange={(e) => setName(e.target.value)}
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
            <label className="text-sm text-zinc-400">Message</label>
            <textarea
              rows={5}
              placeholder="Write your message..."
              value={message}
              onChange={(e) => setMessage(e.target.value)}
              required
              className="mt-2 w-full bg-zinc-950 border border-zinc-800 hover:border-zinc-700 rounded-lg px-4 py-3 focus:outline-none focus:border-emerald-500 transition-colors"
            />
          </div>

          <button
            type="submit"
            disabled={loading}
            className="
              w-full 
              bg-emerald-500 
              hover:bg-emerald-600 
              disabled:bg-emerald-600/40
              disabled:cursor-not-allowed
              text-white 
              py-3 
              rounded-lg 
              transition 
              duration-200 
              cursor-pointer
            "
          >
            {loading ? "Sending..." : "Send Message"}
          </button>

        </form>

      </div>

    </section>
  );
}
