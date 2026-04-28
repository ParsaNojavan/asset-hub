import { cookies } from "next/headers";
import { redirect } from "next/navigation";

export default async function apiProxy(
  url: string,
  options: RequestInit = {}
) {

  const cookieStore = await cookies();
  const accessToken = cookieStore.get("accessToken")?.value;

  let res = await fetch(url, {
    ...options,
    headers: {
      ...(options.headers || {}),
      Authorization: accessToken ? `Bearer ${accessToken}` : "",
    },
    cache: "no-store",
  });

  if (res.status === 401) {
    redirect("/login");
  }

  return res;
}
