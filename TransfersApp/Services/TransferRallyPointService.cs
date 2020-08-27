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
    public class TransferRallyPointService : ITransferRallyPointService
    {
        private IRepository<TransferRallyPoint, Guid> dbContext;
        private ITransferRepository dbContextTransfer;

        public TransferRallyPointService(IRepository<TransferRallyPoint, Guid> _dbContext, ITransferRepository _dbContextTransfer)
        {
            dbContext = _dbContext;
            dbContextTransfer = _dbContextTransfer;
        }
        public async Task<Guid> Create(TransferRallyPointModel model)
        {
            var entity = AutoMapper.Mapper.Map<TransferRallyPoint>(model);
            
                return await dbContext.Create(entity);
        }

        public async Task Delete(TransferRallyPointModel model)
        {
           // var entity = AutoMapper.Mapper.Map<TransferRallyPoint>(model);
            await dbContext.DeleteById(model.Id);

        }

        public async Task<IEnumerable<TransferRallyPointModel>> Get()
        {
            var listEntities = await dbContext.Get();
            var models = AutoMapper.Mapper.Map<IEnumerable<TransferRallyPointModel>>(listEntities);
            return models;
        }

        public async Task<IEnumerable<TransferRallyPointModel>> Get(Func<TransferRallyPointModel, bool> predicate, bool includeDeeleted = false)
        {
            var listEntities = await dbContext.Get(v => predicate(AutoMapper.Mapper.Map<TransferRallyPointModel>(v)));
            var models = AutoMapper.Mapper.Map<IEnumerable<TransferRallyPointModel>>(listEntities);
            return models;
        }
        public async Task<TransferRallyPointModel> GetbyId(Guid Id)
        {
            var entity = (await dbContext.Get(v => v.Id == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<TransferRallyPointModel>(entity);
                return model;
            }
            else
                return null;
        }

        public async Task<List<TransferRallyPointModel>> GetByTransferId(Guid Id)
        {
            var entity = (await dbContext.Get(v => v.TransferId.HasValue && v.TransferId.Value == Id));
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<List<TransferRallyPointModel>>(entity);
                return model;
            }
            else
                return null;
        }

        public async Task Update(TransferRallyPointModel model)
        {
            var entity = AutoMapper.Mapper.Map<TransferRallyPoint>(model);
            await dbContext.Update(entity);
        }
    }
}