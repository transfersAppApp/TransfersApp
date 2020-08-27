using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TransfersApp.DataAccess.Entities;
using TransfersApp.Models;
using System.Data;
using System.Data.Entity;
using TransfersApp.Entities;

namespace TransfersApp.DataAccess.Repositories
{
    public class TransferRallyPointRepository : EntityRepository<TransferRallyPoint, Guid>
    {
        public TransferRallyPointRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }


        public override async Task<ICollection<TransferRallyPoint>> Get(bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                try
                {
                    var all = (_dbContext.TransferRallyPoints.Include(c => c.Transfer).AsNoTracking().ToList());
                    if (!includeDeeleted)
                    {
                        all = all.Where(v => !v.IsDeleted).ToList();
                    }
                    return all.ToList();
                }
                catch (Exception x)
                { 
                }
                return null;
            }
        }

        public override async Task<ICollection<TransferRallyPoint>> Get(Func<TransferRallyPoint, bool> predicate, bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                try
                {
                    var all = (_dbContext.TransferRallyPoints.Include(c => c.Transfer).AsNoTracking()).Where(predicate).ToList();
                    if (!includeDeeleted)
                    {
                        all = all.Where(v => !v.IsDeleted).ToList();
                    }
                    return all.ToList();
                }
                catch (Exception x)
                { 
                
                }
                return null;
            }
        }

    }
}