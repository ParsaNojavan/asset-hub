import apiProxy from "@/lib/apiProxy";
import { cookies } from "next/headers";

export async function DELETE() {
  try {
    const cookieStore = await cookies();
    const accessToken = cookieStore.get("accessToken")?.value;

    if (!accessToken) {
      return Response.json(
        { message: "Unauthorized" },
        { status: 401 }
      );
    }

    const backendRes = await apiProxy(
      `http://localhost:5139/api/user/delete`,
      {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      }
    );

    if (!backendRes.ok) {
      const text = await backendRes.text();

      return Response.json(
        {
          message: text || "Failed to delete account",
        },
        { status: backendRes.status }
      );
    }

    cookieStore.delete("accessToken");
    cookieStore.delete("refreshToken");

    return new Response(null, { status: 204 });

  } catch (err) {
    console.error("Delete error:", err);

    return Response.json(
      { message: "Internal server error" },
      { status: 500 }
    );
  }
}
