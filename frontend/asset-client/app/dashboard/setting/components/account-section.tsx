'use client'

import { useState } from "react";
import ChangePassword from "./change-password";

export default function AccountSection () {

    const [passwordOpen, setPasswordOpen] = useState(false);

    return (
        <><section className="border-t border-zinc-800 pt-12 grid grid-cols-1 md:grid-cols-3 gap-8">

            <div className="space-y-1">
                <h2 className="text-lg font-medium text-foreground">Account</h2>
                <p className="text-sm text-zinc-500">
                    Manage security and credentials for your account.
                </p>
            </div>

            <div className="md:col-span-2">
                <button
                    onClick={() => setPasswordOpen(true)}
                    className="px-4 cursor-pointer py-2 hover:text-white border border-zinc-700 rounded-md hover:bg-zinc-800 transition w-full sm:w-auto"
                >
                    Change Password
                </button>
            </div>

        </section>
        <ChangePassword open={passwordOpen} onClose={()=>setPasswordOpen(false)}/>
        </>
                
    );
}