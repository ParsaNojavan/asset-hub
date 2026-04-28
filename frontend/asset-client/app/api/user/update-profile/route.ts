import { NextResponse } from "next/server";
import { cookies } from "next/headers";
import apiProxy from "@/lib/apiProxy";

export async function PATCH(req: Request) {
  try {
    const cookieStore = await cookies();
    const accessToken = cookieStore.get("accessToken")?.value;
    if (!accessToken) return NextResponse.json({ error: "Unauthorized" }, { status: 401 });

    const formData = await req.formData();

    const backendRes = await apiProxy("http://localhost:5139/api/user/update-profile", {
      method: "PATCH",
      headers: {
        "Authorization": `Bearer ${accessToken}`,
      },
      body: formData, 
    });

    if (!backendRes.ok) {
      const errorText = await backendRes.text();
      return NextResponse.json({ message: errorText || "Backend error" }, { status: backendRes.status });
    }

    const result = await backendRes.json();
    return NextResponse.json(result);

  } catch (error) {
    console.error("Route Handler Error:", error);
    return NextResponse.json({ message: "Internal Server Error" }, { status: 500 });
  }
}
