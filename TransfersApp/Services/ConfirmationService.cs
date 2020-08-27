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
    public class ConfirmationService : IServiceBase<ConfirmationModel, Guid>
    {
        private IRepository<Confirmation, Guid> dbContext;

        public ConfirmationService(IRepository<Confirmation, Guid> _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<Guid> Create(ConfirmationModel model)
        {
            var ent = (await dbContext.Get(m => m.TravelId == model.TravelId && m.ClientId == model.ClientId)).AsEnumerable();
            if (!ent?.Any() ?? false)
            {
                var tariffEntity = AutoMapper.Mapper.Map<Confirmation>(model);
                return await dbContext.Create(tariffEntity);
            }
            else
            {
                return ent?.FirstOrDefault()?.Id ?? Guid.Empty;
            }
        }

        public async Task Delete(ConfirmationModel model)
        {
            var entity = AutoMapper.Mapper.Map<Confirmation>(model);
            await dbContext.Delete(entity);
        }

        public async Task<IEnumerable<ConfirmationModel>> Get()
        {
            var listEntities = (await dbContext.Get()).AsEnumerable();
            var models = AutoMapper.Mapper.Map<IEnumerable<ConfirmationModel>>(listEntities);
            return models;
        }

        public async Task<IEnumerable<ConfirmationModel>> Get(Func<ConfirmationModel, bool> predicate, bool includeDeeleted = false)
        {
            var listEntities = (await dbContext.Get(v => predicate(AutoMapper.Mapper.Map<ConfirmationModel>(v)))).AsEnumerable();
            var models = AutoMapper.Mapper.Map<IEnumerable<ConfirmationModel>>(listEntities);
            return models;
        }
        public async Task<ConfirmationModel> GetbyId(Guid Id)
        {
            var entity = (await dbContext.Get(v => v.Id == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<ConfirmationModel>(entity);
                return model;
            }
            else
                return null;
        }


        public async Task Update(ConfirmationModel model)
        {
            var entity = AutoMapper.Mapper.Map<Confirmation>(model);
            await dbContext.Update(entity);
        }
    }
}