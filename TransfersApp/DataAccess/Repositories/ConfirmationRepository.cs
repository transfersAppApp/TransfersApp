using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TransfersApp.DataAccess.Entities;
using TransfersApp.Models;
using System.Data;
using System.Data.Entity;

namespace TransfersApp.DataAccess.Repositories
{
    public class ConfirmationRepository : EntityRepository<Confirmation, Guid>
    {
        public ConfirmationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public override async Task<ICollection<Confirmation>> Get(bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                var all = (_dbContext.Confirmations.Include(c => c.Client).Include(c => c.Travel).AsNoTracking().ToList());
                if (!includeDeeleted)
                {
                    all = all.Where(v => !v.IsDeleted).ToList();
                }
                return all.ToList();
            }
        }

        public override async Task<ICollection<Confirmation>> Get(Func<Confirmation, bool> predicate, bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                var all = (_dbContext.Confirmations.Include(c => c.Client).Include(c => c.Travel).AsNoTracking()).Where(predicate).ToList();
                if (!includeDeeleted)
                {
                    all = all.Where(v => !v.IsDeleted).ToList();
                }
                return all.ToList();
            }
        }

    }
}