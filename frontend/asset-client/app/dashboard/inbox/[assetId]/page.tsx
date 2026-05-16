import Image from "next/image";
import { ArrowLeft, Download, Eye } from "lucide-react";
import FileHeader from "@/app/components/file-header";
import FileMetaData from "@/app/components/metadata";
import { cookies } from "next/headers";
import apiProxy from "@/lib/apiProxy";
import { canPreview } from "@/utils/previewableTypes";

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

         <div className="flex flex-col sm:flex-row gap-2 justify-center sm:justify-start items-center">
          {/* DOWNLOAD */}
          <a
            href={`/api/asset/download/${assetId}`}
            className="inline-flex justify-center w-48 px-2 py-3 bg-emerald-600 hover:bg-emerald-700 cursor-pointer gap-2 items-center rounded-lg text-white transition">
            <Download size={20} />
            Download File
          </a>

          {canPreview(data.asset.contentType) && (

            <a
              href={`/api/asset/preview/${assetId}`} target="_blank"
              className="inline-flex justify-center w-48 px-2 py-3 bg-amber-500 hover:bg-amber-600 cursor-pointer gap-2 items-center rounded-lg text-white transition">
              <Eye size={20} />
              Preview File
            </a>
          )}
        </div>

        <div className="border-t border-zinc-800 my-10"></div>

        {/* METADATA */}
        <FileMetaData file={data}/>
        
      </div>
    </div>
  );
}
