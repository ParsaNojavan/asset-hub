import Image from "next/image";
import { ArrowLeft, Download } from "lucide-react";
import { BreadcrumbBar } from "@/app/components/breadcrumb";
import FileHeader from "@/app/components/file-header";
import FileMetaData from "@/app/components/metadata";
import { cookies } from "next/headers";
import apiProxy from "@/lib/apiProxy";

export default async function AssetDetailsPage({params} : {params:Promise<{assetId:string}>}) {

  const cookieStore = await cookies();
  const token = await cookieStore.get("accessToken")?.value;

  const {assetId} = await params;

  console.log(assetId)
  console.log(token)

  const res = await apiProxy(
      `http://localhost:5139/api/asset-details/${assetId}`,
      {
        method: "GET",
        cache: "no-store",
      }
    );

    const data = await res.json();

    console.log(data);


  let rawPath  = data?.asset.storagePath;
  const segments = rawPath.split("/");
  const breadcrumbs = ["root", ...segments.slice(1)];


  return (
    <div className="w-full h-full p-6 md:p-10 overflow-y-auto">

      {/* Breadcrumb */}
      <div className="flex items-center gap-4 mb-8 text-sm text-zinc-400">
        
        <BreadcrumbBar breadcrumbs={breadcrumbs} parentId={data.asset.folderId}/>

      </div>

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
