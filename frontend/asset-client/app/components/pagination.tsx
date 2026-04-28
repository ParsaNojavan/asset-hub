"use client";

import { useRouter, useSearchParams } from "next/navigation";

export default function Pagination({
  total,
  currentPage,
}: {
  total: number;
  currentPage: number;
}) {
  const router = useRouter();
  const searchParams = useSearchParams();

  const pages = Array.from({ length: total }, (_, i) => i + 1);

  const goToPage = (page: number) => {
    const params = new URLSearchParams(searchParams);

    params.set("page", String(page));

    router.push(`?${params.toString()}`);
  };

  const goPrev = () => {
    if (currentPage > 1) goToPage(currentPage - 1);
  };

  const goNext = () => {
    if (currentPage < total) goToPage(currentPage + 1);
  };

  return (
    <div className="fixed bottom-2 inset-x-0 flex text-white justify-center items-center gap-2 text-sm">

      {/* Prev Button */}
      <button
        onClick={goPrev}
        disabled={currentPage === 1}
        className={`px-3 py-1 rounded cursor-pointer 
          ${currentPage === 1 ? "bg-zinc-900 opacity-40 cursor-not-allowed" : "bg-zinc-800 hover:bg-zinc-700"}
        `}
      >
        Prev
      </button>

      {/* Page Numbers */}
      {pages.map((page) => (
        <button
          key={page}
          onClick={() => goToPage(page)}
          className={`px-3 py-1 rounded cursor-pointer 
            ${
              page === currentPage
                ? "bg-emerald-600 text-white"
                : "bg-zinc-700 text-white hover:bg-zinc-600"
            }
          `}
        >
          {page}
        </button>
      ))}

      {/* Next Button */}
      <button
        onClick={goNext}
        disabled={currentPage === total}
        className={`px-3 py-1 rounded cursor-pointer 
          ${currentPage === total ? "bg-zinc-900 opacity-40 cursor-not-allowed" : "bg-zinc-800 hover:bg-zinc-700"}
        `}
      >
        Next
      </button>
    </div>
  );
}
