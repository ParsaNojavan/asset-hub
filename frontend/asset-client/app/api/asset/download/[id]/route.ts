import apiProxy from "@/lib/apiProxy";
import { access } from "fs";
import { cookies } from "next/headers";
import { NextResponse } from "next/server";

export async function GET(req: Request, { params }: any) {

  const cookie = await cookies()
  const token = cookie.get("accessToken")?.value;

  console.log(token)

  const query = await params;

  const fileId = query.id;

  const response = await apiProxy(
    `http://localhost:5139/api/files/download?FileId=${fileId}`
  );

  if (!response.ok) {
    return new NextResponse("Failed", { status: 400 });
  }

  const blob = await response.blob();

  return new Response(blob, {
    headers: {
      "Content-Type": response.headers.get("Content-Type")!,
      "Content-Disposition": response.headers.get("Content-Disposition")!,
    },
  });
}
