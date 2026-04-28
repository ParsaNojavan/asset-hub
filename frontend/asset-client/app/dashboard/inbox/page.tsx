import InboxTable from "@/app/dashboard/inbox/components/inbox-table";
import Pagination from "@/app/components/pagination";
import { cookies } from "next/headers";
import apiProxy from "@/lib/apiProxy";
import { redirect } from "next/navigation";

export default async function InboxPage({searchParams} : {searchParams : Promise<{page : string}>}) {

  const cookieStore = await cookies();
  const accessToken = cookieStore.get('accessToken')?.value;

  const page = Number((await searchParams).page ?? "1") || 1;

  const res = await apiProxy(`http://localhost:5139/api/share-details/inbox?page=${page}&pageSize=5`,{
    headers : {
      Authorization : `Bearer ${accessToken}`
    }
  });

  const data = await res.json();
  console.log(data);
  

  return (
    <div className="p-10 min-h-screen text-[text-color]">

      <h1 className="text-xl font-medium mb-8">Inbox</h1>

      {/* Table */}
      <InboxTable inboxItems={data.items}/>


      {/* Pagination */}
      <Pagination total={data.total} currentPage={page}/>
    </div>
  );
}
