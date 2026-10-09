import type { NextConfig } from "next";
import withPWAInit from "@ducanh2912/next-pwa";

const withPWA = withPWAInit({
  dest: "public",
  disable: false,
  register: true,
  workboxOptions: {
    skipWaiting: true,
    clientsClaim: true,
  },
});


const nextConfig: NextConfig = {
  /* config options here */
};

export default withPWA(nextConfig);