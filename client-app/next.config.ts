import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  env: {
    SOCKET_CONNECTION: process.env.SOCKET_CONNECTION,
    PAYMENT_ROUTE: process.env.PAYMENT_ROUTE,
  },
};

export default nextConfig;
