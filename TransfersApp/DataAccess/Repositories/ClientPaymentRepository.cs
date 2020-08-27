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
    public class ClientPaymentRepository : EntityRepository<ClientPayment, Guid>
    {
        public ClientPaymentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }


        public override async Task<ICollection<ClientPayment>> Get(bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                try
                {

                    var all = (_dbContext.ClientPayments.AsNoTracking().ToList());
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

        public override async Task<ICollection<ClientPayment>> Get(Func<ClientPayment, bool> predicate, bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                try
                {
                    var all = (_dbContext.ClientPayments.AsNoTracking()).Where(predicate).ToList();
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