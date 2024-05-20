using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SalutemCRM.Domain.Model;

namespace SalutemCRM.Database;

public class Program
{
    static void Main(string[] args)
    {
        DatabaseContext.ReCreateDatabase();
    }
}