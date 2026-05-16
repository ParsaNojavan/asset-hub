process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";


import { cookies } from "next/headers";
import { NextResponse } from "next/server";

export async function GET(
  req: Request,
  { params }: { params: Promise<{ id: string }> }
) {


  const { id } = await params;

  const cookieStore = await cookies();
  const token = cookieStore.get("accessToken")?.value;

  const response = await fetch(
    `https://localhost:7024/api/files/preview?FileId=${id}`,
    {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

  if (!response.ok) {
    const text = await response.text();
    console.error(text);
    return new NextResponse("Failed", { status: response.status });
  }

  return new Response(response.body, {
    headers: {
      "Content-Type": response.headers.get("Content-Type") ?? "application/octet-stream",
      "Content-Disposition": "inline",
    },
  });
}
