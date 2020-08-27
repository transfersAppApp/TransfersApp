using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TransfersApp.DataAccess.Entities;
using TransfersApp.Models;
using System.Data;
using System.Data.Entity;
using TransfersApp.BL.Models;

namespace TransfersApp.DataAccess.Repositories
{
    public class TransferRepository : EntityRepository<Transfer, Guid>, ITransferRepository
    {
        public TransferRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public override async Task<ICollection<Transfer>> Get(bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                var all = (_dbContext.Transfers.Include(t => t.Status).AsNoTracking().ToList());
                if (!includeDeeleted)
                {
                    all = all.Where(v => !v.IsDeleted).ToList();
                }
                return all.ToList();
            }
        }

        public override async Task<ICollection<Transfer>> Get(Func<Transfer, bool> predicate, bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                var all = ((_dbContext.Transfers.Include(t => t.Status).AsNoTracking())).Where(predicate).ToList();
                if (!includeDeeleted)
                {
                    all = all.Where(v => !v.IsDeleted).ToList();
                }
                return all.ToList();
            }
        }



    }
}