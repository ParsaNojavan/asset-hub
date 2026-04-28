using Storage.Application.Services;
using Storage.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Infrastructure.Services.Storage.Local
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _rootPath;
        private readonly IAssetRepository _assetRepository;
        public LocalFileStorageService(IAssetRepository assetRepository)
        {
            _rootPath = Path.Combine(Directory.GetCurrentDirectory(), "storage-data");
            _assetRepository = assetRepository;
            Directory.CreateDirectory(_rootPath);
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string relativePath, CancellationToken cancellationToken = default)
        {
            var fullPath = Path.Combine(_rootPath, relativePath);

            var directory = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory!);

            await using var file = new FileStream(fullPath, FileMode.Create
                , FileAccess.Write, FileShare.None);

            await fileStream.CopyToAsync(file, cancellationToken);

            return relativePath;
        }


        public async Task<Stream> DownloadAsync(string relativePath, CancellationToken cancellationToken = default)
        {
            var fullPath = Path.Combine(_rootPath, relativePath);
            fullPath = Path.GetFullPath(fullPath);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("File not found", fullPath);

            return new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }



        public async Task DeleteAsync(string relativePath, CancellationToken cancellationToken = default)
        {
            var file = await _assetRepository.GetByPathAsync(relativePath);
            if (file != null)
                await _assetRepository.DeleteAsync(file.Id);

                var fullPath = Path.Combine(_rootPath, relativePath);

            if (File.Exists(fullPath)) 
                    File.Delete(fullPath);

            await Task.CompletedTask;
        }

        public async Task DeleteDirectoryAsync(string relativePath, CancellationToken cancellationToken = default)
        {
            var normalizedPath = relativePath.Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(_rootPath, normalizedPath);

            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, recursive: true);
            }

            await Task.CompletedTask;
        }

        public async Task MoveDirectoryAsync(string sourceRelativePath, string destinationRelativePath, CancellationToken cancellationToken = default)
        {
            var source = Path.Combine(_rootPath, sourceRelativePath);
            var destination = Path.Combine(_rootPath, destinationRelativePath);

            if (!Directory.Exists(source))
                throw new DirectoryNotFoundException(source);

            Directory.Move(source, destination);

            await Task.CompletedTask;
        }

        public async Task MoveFileAsync(
    string sourceRelativePath,
    string destinationRelativePath,
    CancellationToken cancellationToken = default)
        {
            var source = Path.Combine(_rootPath, sourceRelativePath);
            var destination = Path.Combine(_rootPath, destinationRelativePath);

            if (!File.Exists(source))
                throw new FileNotFoundException(source);

            var destinationDirectory = Path.GetDirectoryName(destination);
            if (!Directory.Exists(destinationDirectory))
                Directory.CreateDirectory(destinationDirectory!);

            File.Move(source, destination);

            await Task.CompletedTask;
        }

        public Task CreateDirectoryAsync(string path, CancellationToken cancellationToken)
        {
            var fullPath = Path.Combine(_rootPath, path.TrimStart('/'));

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            return Task.CompletedTask;
        }
    }
}
