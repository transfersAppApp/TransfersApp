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
    public class RallyPointRepository : EntityRepository<RallyPoint, Guid>
    {
        public RallyPointRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public override async Task<ICollection<RallyPoint>> Get(bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                var all = (_dbContext.RallyPoints.AsNoTracking().ToList());
                if (!includeDeeleted)
                {
                    all = all.Where(v => !v.IsDeleted).ToList();
                }
                return all.ToList();
            }
        }

        public override async Task<ICollection<RallyPoint>> Get(Func<RallyPoint, bool> predicate, bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                var all = _dbContext.RallyPoints.AsNoTracking().Where(predicate).ToList();
                if (!includeDeeleted)
                {
                    all = all.Where(v => !v.IsDeleted).ToList();
                }
                return all.ToList();
            }
        }

    }
}