import OutboxGrid from "@/app/dashboard/outbox/components/outbox-grid";
import Pagination from "@/app/components/pagination";
import { cookies } from "next/headers";
import apiProxy from "@/lib/apiProxy";

export default async function OutboxPage({searchParams} : {searchParams : Promise<{page : string}>}) {

    const cookieStore = await cookies();
    const accessToken = cookieStore.get('accessToken')?.value;

    const page = Number((await searchParams).page ?? "1") || 1;
    console.log(page);

    const result = await apiProxy(`http://localhost:5139/api/share-details/outbox?page=${page}`,{
      method : 'GET',
      headers : {
        Authorization : `Bearer ${accessToken}`
      }
    })

    const outbox = await result.json();

    console.log(outbox)

  return (
    <div className="p-10 min-h-screen text-[text-color]">

      <h1 className="text-xl font-medium mb-8">Outbox</h1>

      {/* Files */}
      <OutboxGrid outboxFiles={outbox.items}/>

      {/* Pagination */}
      <Pagination total={outbox.total} currentPage={page}/>
    </div>
  );
}
