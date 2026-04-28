import { NextResponse } from "next/server";
import { cookies } from "next/headers";
import apiProxy from "@/lib/apiProxy";

export async function PUT(req: Request) {
  try {

    const cookieStore = await cookies()
    const token = cookieStore.get("accessToken")?.value;

    console.log(token)

    const body = await req.json();
    
    const res = await apiProxy('http://localhost:5139/api/files/rename', {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(body),
    });

    const data = await res.json();

    if (!res.ok) {
      return NextResponse.json(data, { status: res.status });
    }

    return NextResponse.json(data);

  } catch (error) {
    return NextResponse.json(
      { error: "Backend request failed" },
      { status: 500 }
    );
  }
}
