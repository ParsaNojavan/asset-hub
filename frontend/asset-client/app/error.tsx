"use client";

export default function GlobalError({
  error,
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {
  return (
    <div className="flex min-h-screen flex-col items-center justify-center gap-4">
      <h2 className="text-2xl font-bold">Something went wrong</h2>
      <p className="text-sm text-gray-500">{error.message}</p>
      <button
        onClick={() => reset()}
        className="rounded bg-black px-4 py-2 text-white cursor-pointer"
      >
        Try again
      </button>
    </div>
  );
}
