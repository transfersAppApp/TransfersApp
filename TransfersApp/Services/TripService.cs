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
    public class TransferService : ITransferService
    {
        private ITransferRepository dbContext;

        public TransferService(ITransferRepository _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<Guid> Create(TransferModel model)
        {
            var tariffEntity = AutoMapper.Mapper.Map<Transfer>(model);
            return await dbContext.Create(tariffEntity);
        }

        public async Task Delete(TransferModel model)
        {
            await dbContext.DeleteById(model.Id);
        }

        public async Task<IEnumerable<TransferModel>> Get()
        {
            var listEntities = await dbContext.Get();
            var models = AutoMapper.Mapper.Map<IEnumerable<TransferModel>>(listEntities);
            return models;
        }

        public async Task<IEnumerable<TransferModel>> Get(Func<TransferModel, bool> predicate, bool includeDeeleted = false)
        {
            
            var listEntities = (await dbContext.Get()).ToList().Where(v => predicate(AutoMapper.Mapper.Map<TransferModel>(v)));
            var models = AutoMapper.Mapper.Map<IEnumerable<TransferModel>>(listEntities);
            return models;


        }


        public async Task<IEnumerable<TransferViewModel>> GetAllForView()
        {
            var listEntities = await dbContext.Get();

            var models = listEntities.Select(v=> {
                return new TransferViewModel(v); 
            });
            //var models = AutoMapper.Mapper.Map<IEnumerable<TransferModel>>(listEntities);
            return models;
        }
    public async Task<TransferModel> GetbyId(Guid Id)
        {
            var entity = (await dbContext.Get(v => v.Id == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<TransferModel>(entity);
                return model;
            }
            else
                return null;
        }


        public async Task Update(TransferModel model)
        {
            var entity = AutoMapper.Mapper.Map<Transfer>(model);
            await dbContext.Update(entity);
        }
    }
}