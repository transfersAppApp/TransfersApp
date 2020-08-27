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
using TransfersApp.Services.Interfaces;

namespace TransfersApp.Services
{
    public class TravelService : ITravelService
    {
        private ITravelRepository dbContext;
        public TravelService(ITravelRepository _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<Guid> Create(TravelModel model)
        {
            var tariffEntity = AutoMapper.Mapper.Map<Travel>(model);
            return await dbContext.Create(tariffEntity);
        }

        public async Task Delete(TravelModel model)
        {
            var entity = AutoMapper.Mapper.Map<Travel>(model);
            await dbContext.DeleteById(entity.Id);
        }

        public async Task<IEnumerable<TravelModel>> Get()
        {
            var listEntities = await dbContext.Get();
            var models = AutoMapper.Mapper.Map<IEnumerable<TravelModel>>(listEntities);
            return models;
        }
        public async Task<bool> ChechIfAlreadyCreated(TransferModel model, DateTime potentialNextDate)
        {
            return await dbContext.ChechIfAlreadyCreated(model, potentialNextDate);
        }

        public async Task CleanDuplicates(TransferModel model, DateTime potentialNextDate)
        {
            await dbContext.CleanDuplicates(model, potentialNextDate);
        }

        public async Task CleanAll(TransferModel model, DateTime potentialNextDate)
        {
            await dbContext.CleanAll(model, potentialNextDate);
        }
        public async Task<IEnumerable<TravelModel>> Get(Func<TravelModel, bool> predicate, bool includeDeeleted = false)
        {
            var listEntities = (await dbContext.Get()).ToList().Where(v => predicate(AutoMapper.Mapper.Map<TravelModel>(v)));
            var models = AutoMapper.Mapper.Map<IEnumerable<TravelModel>>(listEntities);
            return models;
        }

        public async Task UpdateBackBalanceForShuttleClients(Guid ShuttleId)
        {
            await dbContext.UpdateBackBalanceForShuttleClients(ShuttleId);
        }
        public async Task UpdateBalanceForShuttleClients(Guid ShuttleId)
        {
            await dbContext.UpdateBalanceForShuttleClients(ShuttleId);
        }

        public async Task<TravelModel> GetbyId(Guid Id)
        {
            var entity = (await dbContext.Get(v => v.Id == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<TravelModel>(entity);
                return model;
            }
            else
                return null;
        }


        public async Task Update(TravelModel model)
        {
            var entity = AutoMapper.Mapper.Map<Travel>(model);
            await dbContext.Update(entity);
        }

        public async Task ExecuteAddForTransfers(List<Guid> transfersIds)
        {
            await dbContext.ExecuteAddForTransfers(transfersIds);

        }

        public async Task ExecuteAddForTransfer(TransferModel transfer)
        {
            await dbContext.ExecuteAddForTransfer(transfer);
        }

        public async Task ExecuteRemoveForTransfer(TransferModel model)
        {
            await dbContext.ExecuteRemoveForTransfer(model);
        }

        public async Task UpdatePricesForShuttle(Guid ShuttleId)
        {
            await dbContext.UpdatePricesForShuttle(ShuttleId);
        }

        public async Task<ComplexTravelsModel> GetComplexTravelsModel(string userName)
        {
            return await dbContext.GetComplexTravelsModel(userName);
        }

        public async Task<IEnumerable<TransferViewModel>> GetTransfersList(string userName)
        {
            return await dbContext.GetTransfersList(userName);

        }
    }
}