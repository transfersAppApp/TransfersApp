using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TransfersApp.BL.Models;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;

namespace TransfersApp.Services
{
    public class TransportClassService : IServiceBase<TransportClassModel, int>
    {
        private IRepository<TransportClass, int> dbContext;

        public TransportClassService(IRepository<TransportClass, int> _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<int> Create(TransportClassModel model)
        {
            var tariffEntity = AutoMapper.Mapper.Map<TransportClass>(model);
            return await dbContext.Create(tariffEntity);
        }

        public async Task Delete(TransportClassModel model)
        {
            var entity = AutoMapper.Mapper.Map<TransportClass>(model);
            await dbContext.Delete(entity);
        }

        public async Task<IEnumerable<TransportClassModel>> Get()
        {
            var listEntities = await dbContext.Get();
            var models = AutoMapper.Mapper.Map<IEnumerable<TransportClassModel>>(listEntities);
            return models;
        }

        public async Task<IEnumerable<TransportClassModel>> Get(Func<TransportClassModel, bool> predicate, bool includeDeeleted = false)
        {
            var listEntities = await dbContext.Get(v => predicate(AutoMapper.Mapper.Map<TransportClassModel>(v)));
            var models = AutoMapper.Mapper.Map<IEnumerable<TransportClassModel>>(listEntities);
            return models;
        }
        public async Task<TransportClassModel> GetbyId(int Id)
        {
            var entity = (await dbContext.Get(v => v.Id == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<TransportClassModel>(entity);
                return model;
            }
            else
                return null;
        }


        public async Task Update(TransportClassModel model)
        {
            var entity = AutoMapper.Mapper.Map<TransportClass>(model);
            await dbContext.Update(entity);
        }
    }
}