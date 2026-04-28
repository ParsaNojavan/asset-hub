import { NextResponse } from "next/server";

export async function POST(req: Request) {
  try {
    const { name, email, message } = await req.json();

    // Validation 
    if (!name || name.trim().length < 2) {
      return NextResponse.json(
        { message: "Name is required and must be at least 2 characters." },
        { status: 400 }
      );
    }

    if (!email || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      return NextResponse.json(
        { message: "A valid email is required." },
        { status: 400 }
      );
    }

    if (!message || message.trim().length < 10) {
      return NextResponse.json(
        { message: "Message must be at least 10 characters long." },
        { status: 400 }
      );
    }

    // Mock logic for now
    console.log("Contact Request Received:");
    console.log({
      name,
      email,
      message,
      time: new Date().toISOString(),
    });

    return NextResponse.json({
      success: true,
      message: "Mock: Message received successfully.",
    });

  } catch (error) {
    console.error("Contact API Error:", error);
    return NextResponse.json(
      { message: "Server error" },
      { status: 500 }
    );
  }
}
