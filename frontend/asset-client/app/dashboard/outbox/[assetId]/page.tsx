import { BreadcrumbBar } from "@/app/components/breadcrumb";
import FileHeader from "@/app/components/file-header";
import ActionButtons from "@/app/dashboard/outbox/components/outbox-actions";
import SharedUsersTable from "../components/shared-user-table";
import { cookies } from "next/headers";
import apiProxy from "@/lib/apiProxy";


export default async function AssetDetailsPage({params} : {params : {assetId : string}}) {

  const cookieStore = await cookies();
  const accessToken = cookieStore.get('accessToken')?.value;

  const {assetId} = await params;
  console.log(assetId);
  const result = await apiProxy(`http://localhost:5139/api/share-details/${assetId}`,{
    method : 'GET',
    headers : {
      Authorization : `Bearer ${accessToken}`
    }
  })

  var outboxDetails = await result.json();
  console.log(outboxDetails)

  return (
    <div className="w-full h-full p-6 md:p-10 overflow-y-auto">

      {/* MAIN INFO */}
      <div className="w-full max-w-5xl mx-auto">

        
        <FileHeader file={outboxDetails.asset}/>


        <div className="border-t border-zinc-800 my-10"></div>

       
      </div>

       <div className="flex flex-col w-[80%] mx-auto gap-4">
          
          <div className="flex-col sm:flex-row flex sm:gap-4">
            <ActionButtons assetId={assetId}/>
          </div>

        {/* Table */}
          <SharedUsersTable Recivers={outboxDetails.recivers} shareId={assetId}/>
        </div>
      </div>
  );
}
