using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using TransfersApp.BL.Models;
using TransfersApp.Jobs;
using TransfersApp.Models;
using TransfersApp.Services;
using TransfersApp.Services.Interfaces;

namespace TransfersApp.Controllers
{
    public class ClientTransferApiController : ApiController
    {
        private IClientService _serviceClient;
        TravelsActualizationJob updateJob;
        private IClientTransferService _service;
        private ITransferService _serviceTransfer;
        public ClientTransferApiController()
        {
            _service = (IClientTransferService)DependencyResolver.Current.GetService(typeof(IClientTransferService));
            _serviceClient = (IClientService)DependencyResolver.Current.GetService(typeof(IClientService));
            _serviceTransfer = (ITransferService)DependencyResolver.Current.GetService(typeof(ITransferService));
            updateJob = new TravelsActualizationJob();

        }
        // GET: api/ClientTransferApi
        public async Task<IEnumerable<ClientTransferModel>> Get()
        {
            var responce = await _service.Get();
            return responce;
        }

        // GET: api/ClientTransferApi/5
        public async Task<ClientTransferModel> GetById(Guid id)
        {
            ClientTransferModel model = await _service.GetbyId(id);

            return model;
        }

        // PUT: api/ClientTransferApi/5
        public async Task<ClientTransferModel> Update(ClientTransferModel model)
        {

            try
            {
                ClientModel client = await _serviceClient.GetClientByName(User.Identity.Name);
                model.ClientId = client.Id;
                await _service.Update(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await ClientTransferModelExists(model.Id)))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            var newmodel = await _service.GetbyId(model.Id);
            return newmodel;
        }

        // POST: api/ClientTransferApi
        public async Task<ClientTransferModel> Create(ClientTransferModel model)
        {
            ClientModel client = await _serviceClient.GetClientByName(User.Identity.Name);
            model.ClientId = client.Id;
            model.IsActive = true;
            model.Id = await _service.Create(model);
            ClientTransferModel newmodel = await _service.GetbyId(model.Id);

            return newmodel;
        }

        [System.Web.Http.HttpGet]
        public async Task<bool> Activate(Guid id)
        {
            try
            {
                ClientTransferModel model = await _service.GetByTransferId(id);
                if (model == null)
                {
                    return false;
                }
                model.IsActive = true;
                await _service.Update(model);
                if(updateJob == null)
                    updateJob = new TravelsActualizationJob();

                    await updateJob.ExecuteAddForTransfer(model.Transfer);
                
                
                return true;
            }
            catch (Exception x)
            {

            }
            return false;
        }
        [System.Web.Http.HttpGet]
        public async Task<bool> Deactivate(Guid id)
        {
            try
                {
                ClientTransferModel model = await _service.GetByTransferId(id);
                if (model == null)
                {
                    return false;
                }
                model.IsActive = false;
                await _service.Update(model);

                if (updateJob == null)
                    updateJob = new TravelsActualizationJob();
                 await updateJob.ExecuteRemoveForTransfer(model.Transfer);

                return true;
            }
            catch (Exception x)
            {

            }
            return false;
        }
        // DELETE: api/ClientTransferApi/5
        [System.Web.Http.HttpDelete]
        public async Task<bool> Delete(Guid id)
        {
            ClientTransferModel model = await _service.GetbyId(id);
            if (model == null)
            {
                return false;
            }
            await _service.Delete(model);

            return true;
        }

        [System.Web.Http.HttpDelete]
        public async Task<bool> DeleteTransfer(Guid id)
        {
            ClientTransferModel model = await _service.GetByTransferId(id);
            if (model == null)
            {
                return false;
            }
            await _service.Delete(model);

            return true;
        }

        private async Task<bool> ClientTransferModelExists(Guid id)
        {
            return (await _service.GetbyId(id)) != null;
        }
    }
}