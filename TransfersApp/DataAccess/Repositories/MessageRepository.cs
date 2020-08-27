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
    public class MessageRepository : EntityRepository<Message, Guid>
    {
        public MessageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public override async Task<ICollection<Message>> Get(bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                var all = (_dbContext.Messages.AsNoTracking().ToList());
                if (!includeDeeleted)
                {
                    all = all.Where(v => !v.IsDeleted).ToList();
                }
                return all.ToList();
            }
        }

        public override async Task<ICollection<Message>> Get(Func<Message, bool> predicate, bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                var all = (_dbContext.Messages.AsNoTracking()).Where(predicate).ToList();
                if (!includeDeeleted)
                {
                    all = all.Where(v => !v.IsDeleted).ToList();
                }
                return all.ToList();
            }
        }

    }
}