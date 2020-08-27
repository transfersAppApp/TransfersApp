using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TransfersApp.BL.Models;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;
using TransfersApp.DataAccess.Repositories;
using TransfersApp.Entities;
using TransfersApp.Models;
using TransfersApp.Services.Interfaces;

namespace TransfersApp.Services
{
    public class ShuttleService : IShuttleService
    {
        private IShuttleRepository dbContext;

        public ShuttleService(IShuttleRepository _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<Guid> Create(ShuttleModel model)
        {
            var tariffEntity = AutoMapper.Mapper.Map<Shuttle>(model);
            return await dbContext.Create(tariffEntity);
        }

        public async Task Delete(ShuttleModel model)
        {
            var entity = AutoMapper.Mapper.Map<Shuttle>(model);
            await dbContext.Delete(entity);
        }

        public async Task<IEnumerable<ShuttleModel>> Get()
        {
            var listEntities = await dbContext.Get();
            var models = AutoMapper.Mapper.Map<IEnumerable<ShuttleModel>>(listEntities);
            return models;
        }

        public async Task<IEnumerable<ShuttleModel>> Get(Func<ShuttleModel, bool> predicate, bool includeDeeleted = false)
        {
            var listEntities = await dbContext.Get(v => predicate(AutoMapper.Mapper.Map<ShuttleModel>(v)));
            var models = AutoMapper.Mapper.Map<IEnumerable<ShuttleModel>>(listEntities);
            return models;
        }
        public async Task<ShuttleModel> GetbyId(Guid Id)
        {
            var entity = (await dbContext.Get(v => v.Id == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<ShuttleModel>(entity);
                return model;
            }
            else
                return null;
        }

        public async Task<List<ShuttleViewModel>> GetEntities(Func<ShuttleModel, bool> predicate, bool includeDeeleted = false)
        {
            var listEntities = await dbContext.GetEntities(v => predicate(AutoMapper.Mapper.Map<ShuttleModel>(v)));
            return listEntities;
        }

        public async Task Update(ShuttleModel model)
        {
            var entity = AutoMapper.Mapper.Map<Shuttle>(model);
            await dbContext.Update(entity);
        }
    }
}