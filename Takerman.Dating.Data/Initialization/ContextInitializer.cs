using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Takerman.Dating.Data.Initialization
{
    public class ContextInitializer(ILogger<ContextInitializer> _logger, DefaultContext _context) : IContextInitializer
    {
        private readonly string[] Scopes = { DriveService.Scope.DriveFile };
        private const string ApplicationName = "dating";

        private const string FolderId = "1v5TJPmqwYwLfDkvzv_mueLP6C1FVGwUv";

        public async Task InitializeAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
                await BackupDatabase(_context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }

        private async Task BackupDatabase(DbContext context)
        {
            using var connection = new SqlConnection(context.Database.GetConnectionString());
            var builder = new SqlConnectionStringBuilder(context.Database.GetConnectionString());
            var backupName = $"{builder.InitialCatalog}_{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}.bak";
            var backupPath = $"/home/takerman/volumes/mssql/backups/{backupName}";
            var query = $"BACKUP DATABASE {builder.InitialCatalog} TO DISK = '/var/opt/mssql/backups/{backupName}'";

            using var command = new SqlCommand(query, connection);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            UploadFile(backupPath);

            File.Delete(backupPath);
        }

        public async void UploadFile(string filePath)
        {
            using var stream = new FileStream("/home/takerman/Documents/Auth/google-service.json", FileMode.Open, FileAccess.Read);
            var credential = GoogleCredential.FromStream(stream).CreateScoped(DriveService.Scope.DriveFile);
            // var secrets = await GoogleClientSecrets.FromStreamAsync(stream);
            // var credPath = "/home/takerman/Documents/Auth/token.json";
            // var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(secrets.Secrets, Scopes, "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;

            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = Path.GetFileName(filePath)
                // Parents = [FolderId]
            };

            using var fsStream = new FileStream(filePath, FileMode.Open);
            var request = service.Files.Create(fileMetadata, fsStream, "application/octet-stream");
            request.Fields = "id";
            request.Upload();

            var file = request.ResponseBody;

            var cleanupRequest = service.Files.List();
            cleanupRequest.Q = $"'{FolderId}' in parents";
            cleanupRequest.Fields = "nextPageToken, files(id, name, createdTime)";
            var result = cleanupRequest.Execute();

            var files = result.Files.OrderByDescending(f => f.CreatedTimeDateTimeOffset).ToList();

            for (int i = 10; i < files.Count; i++)
            {
                var deleteRequest = service.Files.Delete(files[i].Id);
                deleteRequest.Execute();
            }
        }
    }
}