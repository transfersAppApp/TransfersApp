using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TransfersApp.BL.Models;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;
using TransfersApp.DataAccess.Repositories;
using TransfersApp.Models;

namespace TransfersApp.Services
{
    public class TariffService : IServiceBase<TariffModel, int>
    {
        private IRepository<Tariff, int> dbContext;

        public TariffService(IRepository<Tariff, int> _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<int> Create(TariffModel model)
        {
            var tariffEntity = AutoMapper.Mapper.Map<Tariff>(model);
            return await dbContext.Create(tariffEntity);
        }

        public async Task Delete(TariffModel model)
        {
            var entity = AutoMapper.Mapper.Map<Tariff>(model);
            await dbContext.Delete(entity);
        }

        public async Task<IEnumerable<TariffModel>> Get()
        {
           var listEntities = await dbContext.Get();
            var models = AutoMapper.Mapper.Map<IEnumerable<TariffModel>>(listEntities);
            return models;
        }

        public async Task<IEnumerable<TariffModel>> Get(Func<TariffModel, bool> predicate, bool includeDeeleted = false)
        {
            var listEntities = await dbContext.Get(v => predicate(AutoMapper.Mapper.Map<TariffModel>(v)));
            var models = AutoMapper.Mapper.Map<IEnumerable<TariffModel>>(listEntities);
            return models;
        }
        public async Task<TariffModel> GetbyId(int Id)
        {
            var entity = (await dbContext.Get(v => v.Id == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<TariffModel>(entity);
                return model;
            }
            else
                return null;
        }


        public async Task Update(TariffModel model)
        {
            var entity = AutoMapper.Mapper.Map<Tariff>(model);
            await dbContext.Update(entity);
        }
    }
}