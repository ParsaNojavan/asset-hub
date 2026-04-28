import Modal from "@/app/components/modal";
import { AppleIcon, AppWindow, Monitor, Moon, Sun } from "lucide-react";
import EditProfile from "./components/profile";
import ChangePassword from "./components/change-password";
import AppearanceSection from "@/app/dashboard/setting/components/appearance";
import IconSection from "@/app/dashboard/setting/components/icon-section";
import AccountSection from "./components/account-section";
import DangerZone from "@/app/dashboard/setting/components/danger-zone";

export default function SettingsPage() {

  return (
    <div className="px-6 md:px-8 py-10 flex justify-center">
      <div className="w-full max-w-5xl space-y-16">

        {/* PAGE HEADER */}
        <div className="space-y-1">
          <h1 className="text-2xl font-semibold text-foreground">Settings</h1>
          <p className="text-zinc-500 text-sm">
            Manage your account settings, preferences, and profile configuration.
          </p>
        </div>

        <EditProfile/>

        <AppearanceSection/>

        <IconSection/>

        <AccountSection/>

        <DangerZone/>

      </div>
    </div>
  );
}
