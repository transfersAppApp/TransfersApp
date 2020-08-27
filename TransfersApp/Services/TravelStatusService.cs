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
    public class TravelStatusService : IServiceBase<TravelStatusModel, int>
    {
        private IRepository<TravelStatus, int> dbContext;

        public TravelStatusService(IRepository<TravelStatus, int> _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<int> Create(TravelStatusModel model)
        {
            var tariffEntity = AutoMapper.Mapper.Map<TravelStatus>(model);
            return await dbContext.Create(tariffEntity);
        }

        public async Task Delete(TravelStatusModel model)
        {
            var entity = AutoMapper.Mapper.Map<TravelStatus>(model);
            await dbContext.Delete(entity);
        }

        public async Task<IEnumerable<TravelStatusModel>> Get()
        {
            var listEntities = await dbContext.Get();
            var models = AutoMapper.Mapper.Map<IEnumerable<TravelStatusModel>>(listEntities);
            return models;
        }

        public async Task<IEnumerable<TravelStatusModel>> Get(Func<TravelStatusModel, bool> predicate, bool includeDeeleted = false)
        {
            var listEntities = await dbContext.Get(v => predicate(AutoMapper.Mapper.Map<TravelStatusModel>(v)));
            var models = AutoMapper.Mapper.Map<IEnumerable<TravelStatusModel>>(listEntities);
            return models;
        }
        public async Task<TravelStatusModel> GetbyId(int Id)
        {
            var entity = (await dbContext.Get(v => v.Id == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<TravelStatusModel>(entity);
                return model;
            }
            else
                return null;
        }


        public async Task Update(TravelStatusModel model)
        {
            var entity = AutoMapper.Mapper.Map<TravelStatus>(model);
            await dbContext.Update(entity);
        }
    }
}