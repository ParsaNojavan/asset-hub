// app/api/upload/route.ts

export const runtime = "nodejs";


import { NextRequest, NextResponse } from "next/server";
import { cookies } from "next/headers";
import apiProxy from "@/lib/apiProxy";

export async function POST(req: NextRequest) {
  try {
    const cookieStore = await cookies();
    const accessToken = cookieStore.get("accessToken")?.value;

    if (!accessToken) {
      return NextResponse.json(
        { message: "Unauthorized" },
        { status: 401 }
      );
    }

    const formData = await req.formData();
    const file = formData.get("file") as File | null;
    const folderId = formData.get("folderId")?.toString() ?? null;
    const path = formData.get("path")?.toString() ?? null;

    console.log("RH received:", { path, folderId, fileName: file?.name });

    if (!file) {
      return NextResponse.json(
        { message: "No file uploaded" },
        { status: 400 }
      );
    }

    if (!path) {
      return NextResponse.json(
        { message: "Path is required" },
        { status: 400 }
      );
    }

    const backendFormData = new FormData();
    backendFormData.append("file", file);
    backendFormData.append("path", path); 
    if (folderId) backendFormData.append("folderId", folderId);

    console.log("sent to backend:", {
      file: backendFormData.get("file"),
      path: backendFormData.get("path")
    });

    const res = await apiProxy("http://localhost:5139/api/files/upload", {
      method: "POST",
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
      body: backendFormData
    });

    if (!res.ok) {
      const txt = await res.text();
      return NextResponse.json(
        { message: "Upload failed", details: txt },
        { status: res.status }
      );
    }

    const json = await res.json();
    return NextResponse.json(json);

  } catch (err) {
    console.error("Upload error:", err);
    return NextResponse.json(
      { message: "Internal Server Error" },
      { status: 500 }
    );
  }
}
