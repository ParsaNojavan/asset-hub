import { NextRequest } from "next/server";
import { cookies } from "next/headers";
import apiProxy from "@/lib/apiProxy";

export async function POST(req: NextRequest) {
  try {
    const body = await req.json();

    const cookieStore = await cookies();
    const token = cookieStore.get("accessToken")?.value;

    if (!token) {
      return new Response("Unauthorized", { status: 401 });
    }

    console.log("Body reached route handler:", body);


    const res = await apiProxy("http://localhost:5139/api/folder/create", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`
      },
      body: JSON.stringify(body)
    });

    if (!res.ok) {
      const error = await res.text();
      return new Response(error, { status: res.status });
    }

    return Response.json(await res.json());

  } catch (err) {
    console.error("Handler error:", err);
    return new Response("Server error", { status: 500 });
  }
}
