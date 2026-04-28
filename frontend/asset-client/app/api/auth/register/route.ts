import { NextResponse } from "next/server";

process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";


export async function POST(request: Request) {
  try {
    const body = await request.json();

    const res = await fetch("https://localhost:7187/api/auth/register", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(body),
    });

    const data = await res.json();
    

    if (!res.ok) {
      return NextResponse.json(
        { message: data.message || "Register failed" },
        { status: res.status }
      );
    }

    const userRes = await fetch("https://localhost:7187/api/user/me", {
      method: "GET",
      headers: { Authorization: `Bearer ${data.accessToken}`, }
    });

    if (!userRes.ok) {
      return NextResponse.json(
        { message: "Failed to fetch user info" },
        { status: userRes.status }
      );
    }

    const user = await userRes.json();

    const response = NextResponse.json(
      { success: true , user : user},
      { status: 200 }
    );


    response.cookies.set("accessToken", data.accessToken, {
      httpOnly: true,
      secure: process.env.NODE_ENV === "production",
      path: "/",
      sameSite: "lax",
      maxAge: 60 * 60 * 24 * 7,
    });

    return response;

  } catch (err) {
    return NextResponse.json(
      { message: "Server Error" },
      { status: 500 }
    );
  }
}
