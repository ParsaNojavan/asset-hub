import apiProxy from "@/lib/apiProxy";
import { cookies } from "next/headers";
import { NextResponse } from "next/server";

export async function GET(req: Request) {

  try {
    // read URL
    const { searchParams } = new URL(req.url);
    const q = searchParams.get("q");

    console.log("Received query:", q);

    // no query? return empty list
    if (!q || q.trim() === "") {
      return NextResponse.json([]);
    }

    // get cookie
    const cookieStore = await cookies();
    const accessToken = cookieStore.get("accessToken")?.value;

    console.log(accessToken)

    // call backend
    const backend = await apiProxy(
      `http://localhost:5139/api/user/search?query=${encodeURIComponent(q)}`,
      {
        method: "GET",
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      }
    );

    const text = await backend.text();
    console.log("Backend status:", backend.status);
    console.log("Backend raw:", text);

    if (!backend.ok) {
      return new NextResponse(text, { status: backend.status });
    }

    return NextResponse.json(JSON.parse(text));

  } catch (err) {
    console.error("Route error:", err);
    return new NextResponse("Internal Error", { status: 500 });
  }
}
