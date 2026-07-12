import Image from "next/image";
import { cookies } from "next/headers";
import { createIconGetter } from "@/utils/getIcon";

interface File {
    asset : {
      id : string,
      fileName: string,
      size: number,
      contentType: string,
      updatedAt : Date,
      createdAt : Date,
    },
    user : {
      username : string,
      imgUrl : string
      email : string
    }
}

export default async function FileHeader({file} : {file : File}) {

    const cookieStore = await cookies();
    const iconPack = cookieStore.get("iconPack")?.value || "default";

      const getIcon = createIconGetter({
        server: () => iconPack
      });

    return (
        
        <div className="flex flex-col md:flex-row md:items-center gap-6">

          {/* Icon */}
          <div>
            <Image
              src={getIcon(file.asset.contentType)}
              alt={file.asset.fileName}
              width={90}
              height={90}
              className="drop-shadow-lg"
            />
          </div>

          {/* File info */}
          <div className="flex flex-col">
            <h1 className="text-2xl md:text-3xl font-semibold text-[header-text-color]">
              {file.asset.fileName}
            </h1>

            <p className="text-zinc-400 mt-1">
              {file.asset.contentType} • {(file.asset.size / (1024 * 1024)).toFixed(2)} Mb
            </p>

            {/* Owner */}
            <div className="flex items-center gap-3 mt-4">
              <img
                src={`https://localhost:7024/${file.user.imgUrl}`}
                alt={file.user.username}
                className="rounded-full w-8 h-8 object-cover"
              />
              <span className="text-[small-text-color]">{file.user.username}</span>
            </div>
          </div>
        </div>
    );
}