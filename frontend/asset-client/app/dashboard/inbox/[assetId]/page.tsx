import Image from "next/image";
import { ArrowLeft, Download } from "lucide-react";
import FileHeader from "@/app/components/file-header";
import FileMetaData from "@/app/components/metadata";
import { cookies } from "next/headers";
import apiProxy from "@/lib/apiProxy";

export default async function AssetDetailsPage({params} : {params : Promise<{assetId : string}>}) {

  const cookieStore = await cookies();
  const accessToken = cookieStore.get('accessToken')?.value;

  const {assetId} = await params;
  
  const res = await apiProxy(`http://localhost:5139/api/asset-details/${assetId}`,{
    headers : {
      Authorization : `Bearer ${accessToken}`
    }
  });

  const data = await res.json();

  return (
    <div className="w-full h-full p-6 md:p-10 overflow-y-auto">


      {/* MAIN INFO */}
      <div className="w-full max-w-5xl mx-auto">

        <FileHeader file={data}/>

        <div className="border-t border-zinc-800 my-10"></div>

        {/* DOWNLOAD */}
        <a 
        href={`/api/asset/download/${assetId}`}
        className="inline-flex px-6 py-3 bg-emerald-600 hover:bg-emerald-700 cursor-pointer gap-2 items-center rounded-lg text-white transition">
          <Download size={20} />
          Download File
        </a>

        <div className="border-t border-zinc-800 my-10"></div>

        {/* METADATA */}
        <FileMetaData file={data}/>
        
      </div>
    </div>
  );
}
