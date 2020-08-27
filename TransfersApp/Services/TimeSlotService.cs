using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TransfersApp.BL.Models;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;
using TransfersApp.Entities;
using TransfersApp.Models;

namespace TransfersApp.Services
{
    public class TimeSlotService : IServiceBase<TimeSlotModel, int>
    {
        private IRepository<TimeSlot, int> dbContext;

        public TimeSlotService(IRepository<TimeSlot, int> _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<int> Create(TimeSlotModel model)
        {
            var tariffEntity = AutoMapper.Mapper.Map<TimeSlot>(model);
            return await dbContext.Create(tariffEntity);
        }

        public async Task Delete(TimeSlotModel model)
        {
            var entity = AutoMapper.Mapper.Map<TimeSlot>(model);
            await dbContext.Delete(entity);
        }

        public async Task<IEnumerable<TimeSlotModel>> Get()
        {
            var listEntities = await dbContext.Get();
            var models = AutoMapper.Mapper.Map<IEnumerable<TimeSlotModel>>(listEntities);
            return models;
        }

        public async Task<IEnumerable<TimeSlotModel>> Get(Func<TimeSlotModel, bool> predicate, bool includeDeeleted = false)
        {
            var listEntities = await dbContext.Get(v => predicate(AutoMapper.Mapper.Map<TimeSlotModel>(v)));
            var models = AutoMapper.Mapper.Map<IEnumerable<TimeSlotModel>>(listEntities);
            return models;
        }
        public async Task<TimeSlotModel> GetbyId(int Id)
        {
            var entity = (await dbContext.Get(v => v.Id == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<TimeSlotModel>(entity);
                return model;
            }
            else
                return null;
        }


        public async Task Update(TimeSlotModel model)
        {
            var entity = AutoMapper.Mapper.Map<TimeSlot>(model);
            await dbContext.Update(entity);
        }
    }
}