import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export function middleware(request: NextRequest) {
  const token = request.cookies.get("accessToken")?.value;
  const { pathname } = request.nextUrl;

  const isPublicApi = pathname.startsWith("/api/auth/login") 
  || pathname.startsWith("/api/auth/register")
  || pathname.startsWith("/api/auth/refresh")

  if (isPublicApi) {
    return NextResponse.next();
  }

  if (!token) {
    return NextResponse.redirect(new URL("/login", request.url));
  }

  return NextResponse.next();
}

export const config = {
  matcher: [
    "/dashboard/:path*",
    "/api/:path*"
  ],
  
};
