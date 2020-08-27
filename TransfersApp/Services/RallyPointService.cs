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
    public class RallyPointService : IServiceBase<RallyPointModel, Guid>
    {
        private IRepository<RallyPoint, Guid> dbContext;

        public RallyPointService(IRepository<RallyPoint, Guid> _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<Guid> Create(RallyPointModel model)
        {
            var tariffEntity = AutoMapper.Mapper.Map<RallyPoint>(model);
            return await dbContext.Create(tariffEntity);
        }

        public async Task<IEnumerable<RallyPointModel>> Get(Func<RallyPointModel, bool> predicate, bool includeDeeleted = false)
        {
            var listEntities = await dbContext.Get(v => predicate(AutoMapper.Mapper.Map<RallyPointModel>(v)));
            var models = AutoMapper.Mapper.Map<IEnumerable<RallyPointModel>>(listEntities);
            return models;
        }
        public async Task Delete(RallyPointModel model)
        {
            var entity = AutoMapper.Mapper.Map<RallyPoint>(model);
            await dbContext.DeleteById(entity.Id);
        }

        public async Task<IEnumerable<RallyPointModel>> Get()
        {
            var listEntities = await dbContext.Get();
            var models = AutoMapper.Mapper.Map<IEnumerable<RallyPointModel>>(listEntities);
            return models;
        }

        public async Task<RallyPointModel> GetbyId(Guid Id)
        {
            var entity = (await dbContext.Get(v => v.Id == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<RallyPointModel>(entity);
                return model;
            }
            else
                return null;
        }


        public async Task Update(RallyPointModel model)
        {
            var entity = AutoMapper.Mapper.Map<RallyPoint>(model);
            await dbContext.Update(entity);
        }
    }
}