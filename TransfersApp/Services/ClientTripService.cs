using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TransfersApp.BL.Models;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;
using TransfersApp.DataAccess.Repositories;
using TransfersApp.Services.Interfaces;

namespace TransfersApp.Services
{
    public class ClientTransferService : IClientTransferService
    {
        private IRepository<ClientTransfer, Guid> dbContext;
        private ITransferRepository dbContextTransfer;

        public ClientTransferService(IRepository<ClientTransfer, Guid> _dbContext, ITransferRepository _dbContextTransfer)
        {
            dbContext = _dbContext;
            dbContextTransfer = _dbContextTransfer;
        }
        public async Task<Guid> Create(ClientTransferModel model)
        {
            var entity = AutoMapper.Mapper.Map<ClientTransfer>(model);
            var checkTransfer = (await dbContext.Get(b=>b.TransferId == model.TransferId)).Any(r=>r.ClientId == model.ClientId);
            if (!checkTransfer)
            {
                return await dbContext.Create(entity);
            }
            else
            {
                return Guid.Empty;
            }
        }

        public async Task Delete(ClientTransferModel model)
        {
           // var entity = AutoMapper.Mapper.Map<ClientTransfer>(model);
            await dbContext.DeleteById(model.Id);
            if (model.TransferId.HasValue)
            {
                var anyActiveClientForTransfer = (await dbContext.Get(v => v.TransferId == model.TransferId)).Any();
                if (!anyActiveClientForTransfer)
                {
                    var inativeTransfer = (await dbContextTransfer.Get(f => f.Id == model.TransferId))?.FirstOrDefault();
                    if (inativeTransfer != null)
                    {
                        await dbContextTransfer.DeleteById(model.TransferId.Value);
                    }
                }
            }
        }

        public async Task<IEnumerable<ClientTransferModel>> Get()
        {
            var listEntities = await dbContext.Get();
            var models = listEntities.Select(v=> new ClientTransferModel(v));
            return models;
        }
        public async Task<IEnumerable<ClientTransferModel>> GetAllForView()
        {
            var listEntities = await dbContext.Get();
            var models = listEntities.Select(v => new ClientTransferModel(v, true,false));
            return models;
        }
        public async Task<IEnumerable<ClientTransferModel>> Get(Func<ClientTransferModel, bool> predicate, bool includeDeeleted = false)
        {
            var listEntities = await dbContext.Get(v => predicate(AutoMapper.Mapper.Map<ClientTransferModel>(v)));
            var models = AutoMapper.Mapper.Map<IEnumerable<ClientTransferModel>>(listEntities);
            return models;
        }
        public async Task<ClientTransferModel> GetbyId(Guid Id)
        {
            var entity = (await dbContext.Get(v => v.Id == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<ClientTransferModel>(entity);
                return model;
            }
            else
                return null;
        }

        public async Task<ClientTransferModel> GetByTransferId(Guid Id)
        {
            var entity = (await dbContext.Get(v => v.TransferId.HasValue && v.TransferId.Value == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<ClientTransferModel>(entity);
                return model;
            }
            else
                return null;
        }

        public async Task Update(ClientTransferModel model)
        {
            var entity = AutoMapper.Mapper.Map<ClientTransfer>(model);
            await dbContext.Update(entity);
        }
    }
}