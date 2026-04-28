import AssetGrid from "@/app/dashboard/assets/components/asset-grid";
import { BreadcrumbBar } from "@/app/components/breadcrumb";
import { FolderGrid } from "@/app/dashboard/assets/components/folder-grid";
import Upload from "./components/upload";
import { cookies } from "next/headers";
import AddFolder from "./components/add-folder";
import apiProxy from "@/lib/apiProxy";

type Props = {
  searchParams: {
    folderId?: string;
  };
};

type folder = {
  id: string;
  name: string;
  ParentFolderId: string;
  path: string;
};

type asset = {
  fileName: string;
  id: string;
  contentType: string;
};

export default async function MyAssetsPage({ searchParams }: Props) {

  

  const params = await searchParams;
  const folderId = params?.folderId ?? null;

  const cookieStore = await cookies();
  const accessToken = cookieStore.get("accessToken")?.value;

  let folders: folder[] = [];
  let assets: asset[] = [];

  // Shared variable for response
  let data: any = null;

  // ------------------------------
  // 1) Fetch folders & files
  // ------------------------------
  if (folderId) {

    const res = await apiProxy(
      `http://localhost:5139/api/folder/${folderId}/contents`,
      {
        method: "GET",
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
        cache: "no-store",
      }
    );

    data = await res.json();

  } else {

    const res = await apiProxy(
      "http://localhost:5139/api/folder/me",
      {
        method: "GET",
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
        cache: "no-store",
      }
    );

    data = await res.json();
  }

  // ------------------------------
  // 2) Assign folders/files
  // ------------------------------
  folders = data?.subFolders ?? [];
  assets = data?.subFiles ?? [];


  let rawPath  = data?.path;
  const segments = rawPath.split("/");
  const breadcrumbs = ["root", ...segments.slice(1)];

  // ------------------------------
  // 3) Render UI
  // ------------------------------
  return (
    <div className="p-10 min-h-screen text-[text-color]">

      <div className="flex flex-col md:flex-row md:justify-between md:items-center mb-8 gap-4">
        <BreadcrumbBar breadcrumbs={breadcrumbs} parentId={data?.parentFolderId} />
        <div className="flex gap-2">
          <AddFolder folderId={data?.folderId}/>
          <Upload 
            folderId={folderId} 
            folderPath={segments.slice(1).join("/")} 
        />
        </div>

      </div>

      <h1 className="text-xl font-medium mb-8">My Assets</h1>

      {/* Subfolders */}
      <FolderGrid folders={folders} />

      {/* Files */}
      <AssetGrid assets={assets} />
    </div>
  );
}
