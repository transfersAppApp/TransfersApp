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
    public class WishService : IServiceBase<WishModel, Guid>
    {
        private IRepository<Wish, Guid> dbContext;

        public WishService(IRepository<Wish, Guid> _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<Guid> Create(WishModel model)
        {
            var tariffEntity = AutoMapper.Mapper.Map<Wish>(model);
            return await dbContext.Create(tariffEntity);
        }

        public async Task Delete(WishModel model)
        {
            var entity = AutoMapper.Mapper.Map<Wish>(model);
            await dbContext.Delete(entity);
        }

        public async Task<IEnumerable<WishModel>> Get()
        {
            var listEntities = await dbContext.Get();
            var models = AutoMapper.Mapper.Map<IEnumerable<WishModel>>(listEntities);
            return models;
        }

        public async Task<IEnumerable<WishModel>> Get(Func<WishModel, bool> predicate, bool includeDeeleted = false)
        {
            var listEntities = await dbContext.Get(v => predicate(AutoMapper.Mapper.Map<WishModel>(v)));
            var models = AutoMapper.Mapper.Map<IEnumerable<WishModel>>(listEntities);
            return models;
        }
        public async Task<WishModel> GetbyId(Guid Id)
        {
            var entity = (await dbContext.Get(v => v.Id == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<WishModel>(entity);
                return model;
            }
            else
                return null;
        }


        public async Task Update(WishModel model)
        {
            var entity = AutoMapper.Mapper.Map<Wish>(model);
            await dbContext.Update(entity);
        }
    }
}