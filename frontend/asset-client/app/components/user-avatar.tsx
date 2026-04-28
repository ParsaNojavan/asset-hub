"use client";
import { useAuthStore } from "@/app/stores/auth-store";
import Link from "next/link";

export default function UserAvatar({size , urlPath}:{size:number , urlPath : string}) {
  const user = useAuthStore((s) => s.user);

  if (!user) return null;

  const cacheBypassUrl = `${user.imgUrl}?refresh=${Date.now()}`;

  const Img = (
    <img
      src={`https://localhost:7024/${cacheBypassUrl}`}
      className={`h-${size} w-${size} rounded-full object-cover`}
    />
  );


  if (!urlPath) return Img;

  return <Link href={urlPath}>{Img}</Link>;
}
