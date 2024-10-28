using PinataNET.Models;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HeyRed.Mime;
using System.Xml.Linq;
using Newtonsoft.Json;
using JsonException = Newtonsoft.Json.JsonException;

namespace PinataNET
{
    public class PinataClient
    {
        private readonly string _jwt;
        private readonly HttpClient _httpClient;

        public PinataClient(string jwt)
        {
            _httpClient = new HttpClient();
            _jwt = jwt ?? throw new NullReferenceException("JWT cannot be null/empty");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwt);
        }

        /// <summary>
        /// Pins file to ipfs
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task PinFileToIPFSAsync(string filePath)
        {
            try
            {
                // Read the file contents
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var mimeType = MimeTypesMap.GetMimeType(filePath);
                    var fileContent = new StreamContent(stream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                    var formData = new MultipartFormDataContent();
                    formData.Add(fileContent, "file", Path.GetFileName(filePath));

                    // Prepare the request
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://api.pinata.cloud/pinning/pinFileToIPFS")
                    {
                        Headers = { Authorization = new AuthenticationHeaderValue("Bearer", _jwt) },
                        Content = formData
                    };

                    // Send the request
                    var response = await _httpClient.SendAsync(request);

                    // Check for success and read response
                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseData); // Output the response
                    }
                    else
                    {
                        throw new Exception($"Failed to pin file: {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Uploads a file to Pinata.
        /// </summary>
        /// <param name="fileStream">File stream of the file to be uploaded</param>
        /// <param name="fileName">Name of the file (including extension)</param>
        /// <param name="groupId">Optional group ID for the file</param>
        /// <returns>FileResponse containing upload details</returns>
        /// <exception cref="Exception">Thrown if the upload fails</exception>
        public async Task<FileResponse> UploadFileAsync(Stream fileStream, string fileName, string groupId = "")
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://uploads.pinata.cloud/v3/files");

                var mimeType = MimeTypesMap.GetMimeType(fileName);
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(fileName), "name");

                    if (!string.IsNullOrEmpty(groupId))
                        content.Add(new StringContent(groupId), "group_id");

                    content.Add(fileContent, "file", fileName);

                    var response = await _httpClient.PostAsync("https://uploads.pinata.cloud/v3/files", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<FileResponse>(responseContent);
                    }
                    else
                    {
                        throw new JsonException($"Failed to upload file: {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new JsonException($"Failed to upload file: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets list of files
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<GetFilesListResponse> GetFilesListAsync()
        {
            var response = await _httpClient.GetAsync("https://api.pinata.cloud/v3/files");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GetFilesListResponse>(responseContent);
            }
            else
            {
                throw new Exception($"Failed to retrieve files: {response.ReasonPhrase}");
            }
        }

        /// <summary>
        /// Gets file by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<FileResponse> GetFileById(string id)
        {
            var response = await _httpClient.GetAsync("https://api.pinata.cloud/v3/files");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FileResponse>(responseContent);
            }
            else
            {
                throw new Exception($"Failed to retrieve file: {response.ReasonPhrase}");
            }
        }

        /// <summary>
        /// Updates file specified
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<FileResponse> UpdateFileNameAsync(string id, string newName)
        {
            var url = $"https://api.pinata.cloud/v3/files/{id}";
            var content = new StringContent("{\"name\": \"" + newName + "\"}", Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FileResponse>(responseContent);
            }
            else
            {
                throw new Exception($"Failed to update file name: {response.ReasonPhrase}");
            }
        }

        /// <summary>
        /// Deletes file specified
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<FileResponse> DeleteFileAsync(string id)
        {
            var url = $"https://api.pinata.cloud/v3/files/{id}";

            var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FileResponse>(responseContent);
            }
            else
            {
                throw new Exception($"Failed to delete file: {response.ReasonPhrase}");
            }
        }
    }
}
