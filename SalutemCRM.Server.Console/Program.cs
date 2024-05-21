using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using SalutemCRM.TCP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SalutemCRM.Server.Console;

class Program
{
    public static IHost? Host { get; private set; }

    private static string FilesContainerPath => $"{Directory.GetCurrentDirectory()}\\Uploaded_files";

    static void Main(string[] args)
    {
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureServices(services => {
                services.AddSingleton<TCPServer>();
            })
            .Build();

        Host!.Services.GetService<TCPServer>()!
            .DoInst(x => x.LoggingAction = (sysMessage) =>
            {
                System.Console.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} {sysMessage}");
            })
            .Do(x => x.DataReceived += (o, e) =>
            {
                switch (e.MessageType)
                {
                    case MBEnums.STRING: { x.Logging($"New message [{e.ThisChannel.Id}]: {e.Message}"); }; break;

                    case MBEnums.FILE_JSON:
                        {
                            List<FileAttach>? receivedFiles = JsonSerializer.Deserialize<List<FileAttach>>(e.Message);

                            using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                            {
                                receivedFiles?.DoForEach(file =>
                                {
                                    Directory.CreateDirectory(FilesContainerPath);

                                    string refFileName = file!.FileName;
                                    string filePath = $"{FilesContainerPath}\\{file!.FileName}";

                                    for (int i = 0; File.Exists(filePath); ++i)
                                       filePath = $"{FilesContainerPath}\\{file!.FileName = $"{i}_{refFileName}"}";

                                    using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                                        fs.Write(file!.Bytes!, 0, file.Bytes!.Length);

                                    file.RecordDT = DateTime.Now;
                                    db.FileAttachs.Add(file);

                                    x.Logging($"New file received [{e.ThisChannel.Id}]: {file!.FileName}\nFile path: {filePath}");
                                });

                                db.SaveChanges();
                            }

                        }; break;

                    case MBEnums.GET_FILE_JSON:
                        {
                            List<FileAttach> response = new() { new FileAttach() { FileName = e.Message } };
                            
                            if (File.Exists($"{FilesContainerPath}\\{e.Message}"))
                                new FileStream($"{FilesContainerPath}\\{e.Message}", FileMode.Open, FileAccess.Read)
                                .DoInst(x => x.Read(response[0].Bytes = new byte[x.Length], 0, Convert.ToInt32(x.Length)))
                                .Do(x => x.Close());
                            else
                                response[0].FileFounded = false;
                            
                            e.ThisChannel.Send(JsonSerializer.Serialize(response), MBEnums.FILE_JSON);

                            x.Logging($"Client request for file [{e.ThisChannel.Id}]: {e.Message}\nDoes file founded: {response[0].FileFounded}");
                        }; break;

                    case MBEnums.USER_JSON:
                        {
                            User? match = JsonSerializer.Deserialize<User>(e.Message),
                                  result = match?.Clone() ?? null;

                            Debug.WriteLine($"{match!.Login} {match!.PasswordMD5}");

                            using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                                e.ThisChannel.Send(JsonSerializer.Serialize(
                                    result = db.Users
                                    .Where(x => x.Login == match!.Login && x.PasswordMD5 == match.PasswordMD5)
                                    .FirstOrDefault()),
                                    MBEnums.USER_JSON
                                );

                            x.Logging($"Authorization attempt [{e.ThisChannel.Id}]: {match?.Login ?? "null"} | {match?.PasswordMD5 ?? "null"}, result = {result != null}, {result?.UserRole?.Name ?? ""}");
                        }; break;

                    default: break;
                }
            })
            .Do(x => x.Start());

        while (true) { }
    }
}
