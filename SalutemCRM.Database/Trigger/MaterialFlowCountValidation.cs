using EntityFrameworkCore.Triggered;
using SalutemCRM.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Database.Trigger;

public class MaterialFlowCountValidation : IBeforeSaveTrigger<MaterialFlow>
{
    public Task BeforeSave(ITriggerContext<MaterialFlow> context, CancellationToken cancellationToken)
    {
        if (context.ChangeType == ChangeType.Added)
        {
            using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
            {
                var whSup = db.WarehouseSupplying.Single(ws => ws.Id == context.Entity.WarehouseSupplyForeignKey);
                whSup.InStockCount += context.Entity.CountReturnedToStock; //???

                db.SaveChanges();
            }
        }

        return Task.CompletedTask;
    }
}
