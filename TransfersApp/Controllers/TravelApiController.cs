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
    public class TravelApiController : ApiController
    {
        
        private ITravelService _service;
        public TravelApiController()
        {
            _service = (ITravelService)DependencyResolver.Current.GetService(typeof(ITravelService));
        }
        // GET: api/TravelApi
        public async Task<IEnumerable<TravelModel>> Get()
        {
            var responce = await _service.Get();

            foreach (var tr in responce)
            {
                tr.TravelStatusId = 1;
                await _service.Update(tr);
            }
            responce = await _service.Get();
            return responce;
        }

        // GET: api/TravelApi/5
        public async Task<TravelModel> GetById(Guid id)
        {
            TravelModel model = await _service.GetbyId(id);

            return model;
        }

        // PUT: api/TravelApi/5
        public async Task<TravelModel> Update(TravelModel model)
        {
           
            try
            {
                await _service.Update(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await TravelModelExists(model.Id)))
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

        // POST: api/TravelApi
        public async Task<TravelModel> Create(TravelModel model)
        {

            model.Id = await _service.Create(model);
            TravelModel newmodel = await _service.GetbyId(model.Id);

            return newmodel;
        }

        // DELETE: api/TravelApi/5
        public async Task<bool> Delete(Guid id)
        {
            TravelModel model = await _service.GetbyId(id);
            if (model == null)
            {
                return false;
            }
            await _service.Delete(model);

            return true;
        }
        private async Task<bool> TravelModelExists(Guid id)
        {
            return (await _service.GetbyId(id)) != null;
        }
    }
}