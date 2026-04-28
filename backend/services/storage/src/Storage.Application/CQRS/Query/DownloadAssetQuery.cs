using MediatR;
using Storage.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Query
{
    public record DownloadAssetQuery(Guid AssetId, Guid UserId): IRequest<DownloadAssetResult> { }

}
