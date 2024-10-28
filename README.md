# PinataNET Documentation

## Overview

`PinataClient` is a class for interacting with the Pinata API, allowing you to upload, pin, retrieve, update, and delete files on IPFS (InterPlanetary File System). The client requires a JWT (JSON Web Token) for authorization.

### Installation

To use the `PinataClient`, ensure you have the following NuGet packages installed:

- **Newtonsoft.Json**
- **HeyRed.Mime**

## Usage

### Creating an Instance

To create an instance of `PinataClient`, you need to provide a JWT:

```csharp
var pinataClient = new PinataClient("your_jwt_here");
```

## Methods

### PinFileToIPFSAsync

```csharp
public async Task<PinFileResponse> PinFileToIPFSAsync(string filePath)
```

Pins a file to IPFS.

#### Parameters

- `filePath` (string): The path of the file to be pinned.

#### Returns

- `PinFileResponse`: A response containing the details of the pinned file.

#### Exceptions

- Throws `JsonException` if the file pinning fails.

#### Example

```csharp
var pinFileResponse = await pinataClient.PinFileToIPFSAsync("path/to/your/file.txt");
```

---

### UploadFileAsync

```csharp
public async Task<FileResponse> UploadFileAsync(Stream fileStream, string fileName, string groupId = "")
```

Uploads a file to Pinata.

#### Parameters

- `fileStream` (Stream): The stream of the file to be uploaded.
- `fileName` (string): The name of the file (including extension).
- `groupId` (string, optional): An optional group ID for the file.

#### Returns

- `FileResponse`: A response containing the upload details.

#### Exceptions

- Throws `JsonException` if the upload fails.

#### Example

```csharp
using (var fileStream = File.OpenRead("path/to/your/file.txt"))
{
    var fileResponse = await pinataClient.UploadFileAsync(fileStream, "file.txt");
}
```

---

### GetFilesListAsync

```csharp
public async Task<GetFilesListResponse> GetFilesListAsync()
```

Retrieves a list of files from Pinata.

#### Returns

- `GetFilesListResponse`: A response containing a list of files.

#### Exceptions

- Throws `Exception` if the retrieval fails.

#### Example

```csharp
var filesListResponse = await pinataClient.GetFilesListAsync();
```

---

### GetFileById

```csharp
public async Task<FileResponse> GetFileById(string id)
```

Gets a file by its ID.

#### Parameters

- `id` (string): The ID of the file to retrieve.

#### Returns

- `FileResponse`: A response containing the file details.

#### Exceptions

- Throws `Exception` if the retrieval fails.

#### Example

```csharp
var fileResponse = await pinataClient.GetFileById("your_file_id");
```

---

### UpdateFileNameAsync

```csharp
public async Task<FileResponse> UpdateFileNameAsync(string id, string newName)
```

Updates the name of a specified file.

#### Parameters

- `id` (string): The ID of the file to update.
- `newName` (string): The new name for the file.

#### Returns

- `FileResponse`: A response containing the updated file details.

#### Exceptions

- Throws `Exception` if the update fails.

#### Example

```csharp
var updatedFileResponse = await pinataClient.UpdateFileNameAsync("your_file_id", "new_file_name.txt");
```

---

### DeleteFileAsync

```csharp
public async Task<FileResponse> DeleteFileAsync(string id)
```

Deletes a specified file.

#### Parameters

- `id` (string): The ID of the file to delete.

#### Returns

- `FileResponse`: A response confirming the deletion of the file.

#### Exceptions

- Throws `Exception` if the deletion fails.

#### Example

```csharp
var deleteFileResponse = await pinataClient.DeleteFileAsync("your_file_id");
```

## Error Handling

The `PinataClient` methods throw exceptions on failure, allowing you to handle errors gracefully in your application. Ensure that you wrap calls in try-catch blocks as needed.

```csharp
try
{
    var fileResponse = await pinataClient.UploadFileAsync(fileStream, "file.txt");
}
catch (JsonException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
}
```

## Conclusion

The `PinataClient` class provides a simple interface for interacting with the Pinata API, enabling file management on IPFS. Use the methods provided to easily pin, upload, retrieve, update, and delete files.
