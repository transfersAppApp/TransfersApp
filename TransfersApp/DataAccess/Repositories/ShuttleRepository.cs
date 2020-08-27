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
using System.Data.SqlClient;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace TransfersApp.DataAccess.Repositories
{
    public class ShuttleRepository : EntityRepository<Shuttle, Guid>, IShuttleRepository
    {
        public ShuttleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public override async Task<ICollection<Shuttle>> Get(bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                var all = (_dbContext.Shuttles.Include(c => c.Route).Include(c => c.TimeSlot).Include(c => c.StartRallyPoint).AsNoTracking().ToList());
                if (!includeDeeleted)
                {
                    all = all.Where(v => !v.IsDeleted).ToList();
                }
                return all.ToList();
            }
        }

        public override async Task<ICollection<Shuttle>> Get(Func<Shuttle, bool> predicate, bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                var all = (_dbContext.Shuttles.Include(c => c.Route).Include(c => c.TimeSlot).Include(c => c.StartRallyPoint).AsNoTracking()).Where(predicate).ToList();
                if (!includeDeeleted)
                {
                    all = all.Where(v => !v.IsDeleted).ToList();
                }
                return all.ToList();
            }
        }


        public async Task<List<ShuttleViewModel>> GetEntities(Func<Shuttle, bool> predicate, bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                var all = (_dbContext.Shuttles.Include(c => c.Route).Include(c => c.TimeSlot).Include(c => c.StartRallyPoint).AsNoTracking()).Where(predicate).ToList();

                var Shuttles = _dbContext.Transfers.GroupBy(v => v.ShuttleId).Select(t => new { Key = t.Key, Count = t.Count() }).ToList();

                if (!includeDeeleted)
                {
                    all = all.Where(v => !v.IsDeleted).ToList();
                }
                List<ShuttleViewModel> result = new List<ShuttleViewModel>();

                all.ForEach(v=> {
                    var model = Mapper.Map< ShuttleViewModel>(v);
                    model.Passengers = Shuttles.FirstOrDefault(r => r.Key.HasValue && r.Key.Value != null && r.Key.Value == model.Id)?.Count ?? 0;
                    result.Add(model);
                });
                return result;
            }
        }
    }
}