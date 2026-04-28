"use client";

import { X, Search, Plus } from "lucide-react";
import { useEffect, useState } from "react";
import { createPortal } from "react-dom";
import clsx from "clsx";
import { useRouter } from "next/navigation";
import toast from "react-hot-toast";

type Result = {
  userId: string;
  imgUrl: string;
  username: string;
  email?: string;
};

type Props = {
  open: boolean;
  onClose: () => void;
  assetId : string;
};

function UserSkeleton() {
  return (
    <div className="flex justify-between items-center px-4 py-3 animate-pulse">
      <div className="flex items-center gap-3">
        <div className="w-12 h-12 rounded-full bg-zinc-800"></div>

        <div className="flex flex-col gap-2">
          <div className="h-3 w-28 bg-zinc-800 rounded"></div>
          <div className="h-3 w-40 bg-zinc-800 rounded"></div>
        </div>
      </div>

      <div className="w-4 h-4 bg-zinc-800 rounded"></div>
    </div>
  );
}

export default function SearchModal({ open, onClose,assetId }: Props) {

  const router = useRouter();

  const [mounted, setMounted] = useState(false);
  const [query, setQuery] = useState("");

  const [loading, setLoading] = useState(false);
  const [results, setResults] = useState<Result[]>([]);
  const [selected, setSelected] = useState<string[]>([]);

  useEffect(() => setMounted(true), []);

  const AddId = (id: string) => {
    setSelected(prev => [...prev, id]);
  };

  const RemoveId = (id: string) => {
    setSelected(prev => prev.filter(i => i !== id));
  };

  useEffect(() => {
  if (open) {
    setQuery("");
    setResults([]);
    setSelected([]);
    setLoading(false);
  }
}, [open]);


  // SEARCH REQUEST
  useEffect(() => {

    if (!query.trim()) {
      setResults([]);
      return;
    }

    const controller = new AbortController();

    const timeout = setTimeout(async () => {

      try {

        setLoading(true);

        const res = await fetch(`http://localhost:3000/api/user/search-users?q=${encodeURIComponent(query)}`, {
          signal: controller.signal,
          credentials : 'include'
        });

        if (!res.ok) throw new Error("failed");

        const data = await res.json();

        setResults(data);

      } catch (err) {

        if (err instanceof DOMException) return;

        console.error(err);

      } finally {

        setLoading(false);

      }

    }, 400);

    return () => {
      clearTimeout(timeout);
      controller.abort();
    };

  }, [query]);

  if (!mounted) return null;
  if (!open) return null;

  return createPortal(
    <div className="fixed inset-0 z-[9999] flex items-start justify-center pt-[10vh]">

      <div
        className="absolute inset-0 bg-black/60 backdrop-blur-sm"
        onClick={onClose}
      />

      <div
        className={clsx(
          "relative w-full max-w-xl rounded-xl",
          "bg-zinc-900 border border-zinc-800 shadow-xl",
          "animate-[fadeInScale_0.18s_ease-out]"
        )}
      >

        <div className="flex items-center px-4 py-3 border-b border-zinc-800">
          <Search className="text-zinc-500" size={18} />

          <input
            autoFocus
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            placeholder="Search users..."
            className="flex-1 bg-transparent outline-none text-zinc-100 px-3 text-sm"
          />

          <button onClick={onClose}>
            <X className="text-zinc-500 cursor-pointer hover:text-white" size={18} />
          </button>
        </div>

        <div className="flex flex-col gap-2 max-h-80 overflow-y-auto py-2 no-scrollbar">

          {/* skeleton */}
          {loading && (
            <>
              <UserSkeleton />
              <UserSkeleton />
              <UserSkeleton />
              <UserSkeleton />
            </>
          )}

          {/* empty */}
          {!loading && results.length === 0 && query && (
            <div className="px-4 py-4 text-zinc-500 text-sm">
              No results found
            </div>
          )}

          {/* results */}
          {!loading &&
            results.map((item) => (
              <div
                key={item.userId}
                className="flex justify-between items-center"
              >

                <div className="flex items-center ml-2">
                  <img
                    src={`https://localhost:7024/${item.imgUrl}`}
                    alt={item.username}
                    className="w-12 h-12 rounded-full object-cover bg-zinc-800"
                  />

                  <div className="px-4 py-3 flex flex-col">
                    <span className="text-zinc-200 text-sm">
                      {item.username}
                    </span>

                    {item.email && (
                      <span className="text-zinc-500 text-xs mt-0.5">
                        {item.email}
                      </span>
                    )}
                  </div>
                </div>

                <input
                  type="checkbox"
                  className="appearance-none w-4 h-4 cursor-pointer my-auto mr-4 border border-zinc-500 bg-transparent rounded checked:bg-emerald-500"
                  onChange={(e) =>
                    e.target.checked
                      ? AddId(item.userId)
                      : RemoveId(item.userId)
                  }
                />

              </div>
            ))}

          {!loading && results.length > 0 && (
            <div className="flex justify-center">
              <button
                disabled={selected.length === 0}
                className="inline-flex justify-center text-white items-center w-[80%] bg-emerald-500 cursor-pointer py-2 rounded-lg my-4 hover:bg-emerald-700 transition gap-2 disabled:opacity-30 disabled:cursor-not-allowed"
                onClick={
                  async () => {
  try {
    const res = await fetch("http://localhost:3000/api/asset/share-asset", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "include",
      body: JSON.stringify({
        assetId : assetId,
        recipients: selected  
      })
    });

    if (!res.ok) {
          onClose();
          toast.error('error sharing');
    };

    console.log("Shared!", await res.json());
    onClose();
    setSelected([]);
    router.refresh()

  } catch (err) {
    onClose();
    toast.error('error sharing');
  }
                
                  }}
              >
                <Plus size={18} />
                Add
              </button>
            </div>
          )}

        </div>
      </div>
    </div>,
    document.body
  );
}
