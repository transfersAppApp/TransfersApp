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
    public class ClientTransferRepository : EntityRepository<ClientTransfer, Guid>
    {
        public ClientTransferRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }


        public override async Task<ICollection<ClientTransfer>> Get(bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                try
                {
                    
                    var all = (_dbContext.ClientTransfers.Include(c => c.Transfer).AsNoTracking().ToList());
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

        public override async Task<ICollection<ClientTransfer>> Get(Func<ClientTransfer, bool> predicate, bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                try
                {
                    var all = (_dbContext.ClientTransfers.Include(c => c.Transfer).AsNoTracking()).Where(predicate).ToList();
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