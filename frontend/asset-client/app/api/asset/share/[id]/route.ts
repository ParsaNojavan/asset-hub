import apiProxy from "@/lib/apiProxy";
import { cookies } from "next/headers";
import { NextRequest } from "next/server";

export async function POST(request:NextRequest, {params}: { params: Promise<{ id: string }> }) {
    
    const cookie = await cookies();
    const token = cookie.get("accessToken")?.value;
    console.log(token)

    const {id} = await params;


  const res = await apiProxy(`http://localhost:5139/api/shares/share/${id}`, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${token}`
    }
  });

  return new Response(null, { status: res.status });
}
