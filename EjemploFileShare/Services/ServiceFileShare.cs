using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Files.Shares;


namespace EjemploFileShare.Services
{
    public class ServiceFileShare
    {
        private ShareDirectoryClient root;

        public ServiceFileShare(String con)
        {
            ShareClient client = 
                new ShareClient(con, "filesharegsr");
            this.root = client.GetRootDirectoryClient();
        }

        public async Task<List<String>> GetFilesAsync()
        {
            List<String> archivos = new List<string>();
            await foreach (var archivo in this.root.GetFilesAndDirectoriesAsync())
            {
                archivos.Add(archivo.Name);
            }
            return archivos;
        }

        public async Task<String> GetFileContentAsync(String nombre)
        {
            ShareFileClient archivo = this.root.GetFileClient(nombre);
            var data = await archivo.DownloadAsync();
            Stream stream = data.Value.Content;
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public async Task DeleteFileAsync(String nombre)
        {
            ShareFileClient archivo = this.root.GetFileClient(nombre);
            await archivo.DeleteAsync();
        }

        public async Task UploadFileAsync(String nombre
            , Stream stream)
        {
            ShareFileClient archivo = this.root.GetFileClient(nombre);
            await archivo.CreateAsync(stream.Length);
            await archivo.UploadAsync(stream);
        }
    }
}
