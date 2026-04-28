import apiProxy from "@/lib/apiProxy";
import { cookies } from "next/headers";
import { NextResponse } from "next/server";

export async function DELETE(req: Request) {
  try {
    const { shareId, reciverId } = await req.json();

    const cookieStore = await cookies();
    const accessToken = cookieStore.get("accessToken")?.value;

    if (!accessToken) {
      return NextResponse.json({ error: "Unauthorized" }, { status: 401 });
    }

    const backendRes = await apiProxy("http://localhost:5139/api/shares/unshare", {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${accessToken}`,
      },
      body: JSON.stringify({ shareId, reciverId }),
    });

    if (backendRes.status === 204) {
      return new Response(null, { status: 204 });
    }

    const errorData = await backendRes.text();
    return NextResponse.json(
      { error: "Failed to unshare", detail: errorData },
      { status: backendRes.status }
    );
  } catch (error: any) {
    return NextResponse.json({ error: "Server error", detail: error.message }, { status: 500 });
  }
}
