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
      email : string
    }
}

export default function FileMetaData ({file} : {file : File}) {
    return (
        <div className="space-y-3 text-sm text-[small-text-color]">
          <div className="flex justify-between">
            <span className="text-[icon-color]">Uploaded on</span><span>{new Date(file.asset.createdAt).toLocaleString()}</span>
          </div>

          <div className="flex justify-between">
            <span className="text-[icon-color]">Last Modified</span>
            <span>{new Date(file.asset.updatedAt).toLocaleString()}</span>
          </div>

          <div className="flex justify-between">
            <span className="text-[icon-color]">Content Type</span>
            <span>{file.asset.contentType}</span>
          </div>

          <div className="flex justify-between">
            <span className="text-[icon-color]">Owner</span>
            <span>{file.user.email}</span>
          </div>

          <div className="flex justify-between">
            <span className="text-[icon-color]">File ID</span>
            <span>{file.asset.id}</span>
          </div>
        </div>
    )
}