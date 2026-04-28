import { NextResponse } from "next/server";
import { cookies } from "next/headers";
import apiProxy from "@/lib/apiProxy";

export async function POST(req: Request) {
  try {
    const body = await req.json();

    const { assetId, recipients } = body;

    console.log(body)

    if (!assetId || !Array.isArray(recipients)) {
      return NextResponse.json(
        { error: "Invalid body" },
        { status: 400 }
      );
    }

    
    console.log(recipients);

    const cookieStore = await cookies();
    const accessToken = cookieStore.get("accessToken")?.value;

    if (!accessToken) {
      return NextResponse.json(
        { error: "Unauthorized" },
        { status: 401 }
      );
    }

    const payload = {
      shareId: assetId,
      reciverIds: recipients
    };

    console.log(payload)

    const response = await apiProxy('http://localhost:5139/api/shares/share', {
      method: "POST",
      headers: {
        "Authorization": `Bearer ${accessToken}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify(payload),
    });

    if (!response.ok) {
      const errorData = await response.text();
      console.error("Backend error:", errorData);

      return NextResponse.json(
        { error: "Backend failed", detail: errorData },
        { status: response.status }
      );
    }

    let json;
    try {
      json = await response.json();
    } catch {
      json = { ok: true };
    }

    return NextResponse.json(json, { status: 200 });

  } catch (err: any) {
    console.error("Route error:", err);
    return NextResponse.json(
      { error: "Server error" },
      { status: 500 }
    );
  }
}
