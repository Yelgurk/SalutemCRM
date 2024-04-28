using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Domain.Model;

public partial class FileAttach
{
    [NotMapped]
    public string FileLocalPath { get; private set; } = "";

    [NotMapped]
    public byte[]? Bytes { get; set; }

    public FileAttach SetLocalFileNamePath(string localPath) =>
        (localPath = localPath.Replace("file:///", "").Replace(@"/", @"\"))
        .Do(x => FileLocalPath = x)
        .Do(x => FileName = x.Split("\\").Reverse().First())
        .Do(x => this);

    [NotMapped]
    public bool FileFounded { get; set; } = true;
}