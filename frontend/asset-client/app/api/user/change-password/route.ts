import { NextResponse } from "next/server";
import { cookies } from "next/headers";
import apiProxy from "@/lib/apiProxy";

export async function PUT(req: Request) {
  try {
    const cookieStore = await cookies();
    const accessToken = cookieStore.get("accessToken")?.value;

    if (!accessToken) {
      return NextResponse.json(
        { error: "Unauthorized" },
        { status: 401 }
      );
    }

    const body = await req.json();

    const backendRes = await apiProxy(
      "http://localhost:5139/api/user/change-password",
      {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          "Authorization": `Bearer ${accessToken}`,
        },
        body: JSON.stringify({
          currentPassword: body.currentPassword,
          newPassword: body.newPassword,
        }),
      }
    );

    if (!backendRes.ok) {
      const errorText = await backendRes.text();
      return NextResponse.json(
        { error: errorText || "Failed to change password" },
        { status: backendRes.status }
      );
    }

    // Your backend returns:
    // return NoContent();
    return new Response(null, { status: 204 });

  } catch (error) {
    console.error("Change password error:", error);
    return NextResponse.json(
      { error: "Internal Server Error" },
      { status: 500 }
    );
  }
}
