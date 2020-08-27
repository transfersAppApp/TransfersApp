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
using TransfersApp.Models;
using TransfersApp.Services;
using TransfersApp.Services.Interfaces;

namespace TransfersApp.Controllers
{
    public class ClientApiController : ApiController
    {
        private IClientService _service;
        public ClientApiController()
        {
            _service = (IClientService)DependencyResolver.Current.GetService(typeof(IClientService));
        }
        // GET: api/ClientApi
        public async Task<IEnumerable<ClientModel>> Get()
        {
            var responce = await _service.Get();
            return responce;
        }

        // GET: api/ClientApi/5
        public async Task<ClientModel> GetById(Guid id)
        {
            ClientModel model = await _service.GetbyId(id);

            return model;
        }
        [System.Web.Http.HttpGet]
        public async Task<ClientModel> GetUser()
        {
            ClientModel model = await _service.GetClientByName(User.Identity.Name);

            return model;
        }
        [System.Web.Http.HttpPut]
        // PUT: api/ClientApi/
        public async Task<ClientModel> Update(ClientModel model)
        {
            try
            {
                await _service.Update(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await ClientModelExists(model.Id)))
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

        [System.Web.Http.HttpPut]
        // PUT: api/ClientApi/
        public async Task<ClientModel> UpdatePreferences(PreferencesModel model)
        {
                ClientModel client = await _service.GetClientByName(User.Identity.Name);
            try
            {
                client.MinPassengers = model.MinPassengers;
                await _service.Update(client);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await ClientModelExists(client.Id)))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return client;
        }

        // POST: api/ClientApi
        public async Task<ClientModel> Create(ClientModel model)
        {
            model.Id = await _service.Create(model);
            ClientModel newmodel = await _service.GetbyId(model.Id);

            return newmodel;
        }

        // DELETE: api/ClientApi/5
        public async Task<bool> Delete(Guid id)
        {
            ClientModel model = await _service.GetbyId(id);
            if (model == null)
            {
                return false;
            }
            await _service.Delete(model);

            return true;
        }

        private async Task<bool> ClientModelExists(Guid id)
        {
            return (await _service.GetbyId(id)) != null;
        }
    }
}