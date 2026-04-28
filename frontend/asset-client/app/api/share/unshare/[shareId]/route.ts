import { NextRequest, NextResponse } from "next/server";
import { cookies } from "next/headers";
import apiProxy from "@/lib/apiProxy";

export async function DELETE(
  req: NextRequest,
  { params }: { params: { shareId: string } }
) {
  try {
    const cookieStore = await cookies();
    const accessToken = cookieStore.get("accessToken")?.value;

    if (!accessToken) {
      return NextResponse.json(
        { error: "Unauthorized" },
        { status: 401 }
      );
    }

    const { shareId } = await params;

    if (!shareId) {
      return NextResponse.json(
        { error: "shareId is required" },
        { status: 400 }
      );
    }

    const backendResponse = await apiProxy(
      `http://localhost:5139/api/shares/unshare/${shareId}`,
      {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      }
    );

    if (!backendResponse.ok) {
      const text = await backendResponse.text();
      return NextResponse.json(
        { error: "Failed to unshare asset", backend: text },
        { status: backendResponse.status }
      );
    }

    const result = await backendResponse.json();

    return NextResponse.json(
      { message: "Unshared successfully", data: result },
      { status: 200 }
    );

  } catch (err: any) {
    return NextResponse.json(
      { error: "Server error", detail: err.message },
      { status: 500 }
    );
  }
}
