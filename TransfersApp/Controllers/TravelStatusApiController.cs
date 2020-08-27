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

namespace TransfersApp.Controllers
{
    public class TravelStatusApiController : ApiController
    {

        private IServiceBase<TravelStatusModel, int> _service;
        public TravelStatusApiController()
        {
            _service = (IServiceBase<TravelStatusModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TravelStatusModel, int>));
        }
        // GET: api/TravelStatusModels
        public async Task<IEnumerable<TravelStatusModel>> Get()
        {
            var responce = await _service.Get();
            return responce;
        }

        // GET: api/TravelStatusModels/5
        public async Task<TravelStatusModel> GetById(int id)
        {
            TravelStatusModel model = await _service.GetbyId(id);

            return model;
        }

        // PUT: api/TravelStatusModels/5
        public async Task<TravelStatusModel> Update(TravelStatusModel model)
        {

            try
            {
                await _service.Update(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await TravelStatusModelExists(model.Id)))
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

        // POST: api/TravelStatusModels
        public async Task<TravelStatusModel> Create(TravelStatusModel model)
        {
            model.Id = await _service.Create(model);
            TravelStatusModel newmodel = await _service.GetbyId(model.Id);

            return newmodel;
        }

        // DELETE: api/TravelStatusModels/5
        public async Task<bool> Delete(int id)
        {
            TravelStatusModel model = await _service.GetbyId(id);
            if (model == null)
            {
                return false;
            }
            await _service.Delete(model);

            return true;
        }
        private async Task<bool> TravelStatusModelExists(int id)
        {
            return (await _service.GetbyId(id)) != null;
        }
    }
}