using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SalutemCRM.Domain.Model;

namespace SalutemCRM.Database;

public class Program
{
    static void Main(string[] args)
    {
        int x = 3;

        if (x == 0)
            using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
            {
                WarehouseItem item1 = new WarehouseItem() { Name = "Спираль", Code = "a123" };
                WarehouseItem item2 = new WarehouseItem() { Name = "Нагреватель", Code = "б456" };
                WarehouseItem item3 = new WarehouseItem() { Name = "Рычаг", Code = "в789" };

                WarehouseSupply sup1 = new WarehouseSupply() {
                    WarehouseItem = item1,
                    Code = "z000123",
                    PriceSingle = 0.15,
                    DeliveryStatus = Delivery_Status.NotShipped,
                    PaymentStatus = Payment_Status.Unpaid,
                    CountOrdered = 20,
                    CountAvailable = 20
                };

                WarehouseSupply sup2 = new WarehouseSupply() {
                    WarehouseItem = item2,
                    Code = "w000234",
                    PriceSingle = 0.20,
                    DeliveryStatus = Delivery_Status.NotShipped,
                    PaymentStatus = Payment_Status.Unpaid,
                    CountOrdered = 18,
                    CountAvailable = 18
                };

                WarehouseSupply sup3 = new WarehouseSupply() {
                    WarehouseItem = item3,
                    Code = "x000345",
                    PriceSingle = 0.25,
                    DeliveryStatus = Delivery_Status.NotShipped,
                    PaymentStatus = Payment_Status.Unpaid,
                    CountOrdered = 16,
                    CountAvailable = 16
                };

                WarehouseSupply sup4 = new WarehouseSupply() {
                    WarehouseItem = item1,
                    Code = "q000456",
                    PriceSingle = 0.30,
                    DeliveryStatus = Delivery_Status.NotShipped,
                    PaymentStatus = Payment_Status.Unpaid,
                    CountOrdered = 14,
                    CountAvailable = 14
                };

                WarehouseSupply sup5 = new WarehouseSupply() {
                    WarehouseItem = item2,
                    Code = "r000567",
                    PriceSingle = 0.35,
                    DeliveryStatus = Delivery_Status.NotShipped,
                    PaymentStatus = Payment_Status.Unpaid,
                    CountOrdered = 12,
                    CountAvailable = 12
                };

                WarehouseSupply sup6 = new WarehouseSupply() {
                    WarehouseItem = item3,
                    Code = "s000678",
                    PriceSingle = 0.40,
                    DeliveryStatus = Delivery_Status.NotShipped,
                    PaymentStatus = Payment_Status.Unpaid,
                    CountOrdered = 10,
                    CountAvailable = 10
                };

                db.WarehouseItems.AddRange(item1, item2, item3);
                db.WarehouseSupplying.AddRange(sup1, sup2, sup3, sup4, sup5, sup6);
                db.SaveChanges();

                foreach (var wi in db.WarehouseItems)
                    foreach (var ws in wi.WarehouseSupplying)
                        Console.WriteLine($"{wi.Name} code: (local = {wi.Code}, shop = {ws.Code}), count: {ws.CountAvailable}");
            }

        else if (x == 1)
            using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
            {
                db.WarehouseItems.Load();
                db.WarehouseSupplying.Load();

                foreach (var wi in db.WarehouseItems)
                    foreach (var ws in wi.WarehouseSupplying)
                        Console.WriteLine($"{wi.Name} code: (local = {wi.Code}, shop = {ws.Code}), count: {ws.CountAvailable}");
            }

        else if (x == 2)
            using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
            {
                db.WarehouseItems.Load();
                db.WarehouseSupplying.Load();

                foreach (var wi in db.WarehouseItems)
                    foreach (var ws in wi.WarehouseSupplying)
                        Console.WriteLine($"{wi.Name} code: (local = {wi.Code}, shop = {ws.Code}), count: {ws.CountAvailable}, payment: {ws.PaymentStatus}, delivery: {ws.DeliveryStatus}");

                Console.WriteLine();

                var item = db.WarehouseSupplying.First(ws => ws.WarehouseItem!.Name == "рычаг");
                item.PaymentStatus = Payment_Status.HalfPaid;
                item.DeliveryStatus = Delivery_Status.FullyDelivered;
                db.SaveChanges();

                foreach (var wi in db.WarehouseItems)
                    foreach (var ws in wi.WarehouseSupplying)
                        Console.WriteLine($"{wi.Name} code: (local = {wi.Code}, shop = {ws.Code}), count: {ws.CountAvailable}, payment: {ws.PaymentStatus}, delivery: {ws.DeliveryStatus}");
            }

        else if (x == 3)
            using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
            {
                db.WarehouseItems.Load();
                db.WarehouseSupplying.Load();

                foreach (var wi in db.WarehouseItems)
                    foreach (var ws in wi.WarehouseSupplying.Where(ws => ws.DeliveryStatus > Delivery_Status.NotShipped))
                        Console.WriteLine($"{wi.Name} code: (local = {wi.Code}, shop = {ws.Code}), count: {ws.CountAvailable}, payment: {ws.PaymentStatus}, delivery: {ws.DeliveryStatus}");
            }
    }
}
