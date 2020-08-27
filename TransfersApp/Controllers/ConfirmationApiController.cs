using DocumentFormat.OpenXml.Presentation;
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
    public class ConfirmationApiController : ApiController
    {
        private IServiceBase<ConfirmationModel, Guid> _service;
        private IClientService _serviceClient;
        private ITravelService _serviceTravel;
        private ITransferService _serviceTransfer;
        public ConfirmationApiController()
        {
            _serviceClient = (IClientService)DependencyResolver.Current.GetService(typeof(IClientService));
            _service = (IServiceBase<ConfirmationModel, Guid>)DependencyResolver.Current.GetService(typeof(IServiceBase<ConfirmationModel, Guid>));
            _serviceTravel = (ITravelService)DependencyResolver.Current.GetService(typeof(ITravelService));
            _serviceTransfer = (ITransferService)DependencyResolver.Current.GetService(typeof(ITransferService));
        }

        // GET: api/ConfirmationModels
        public async Task<IEnumerable<ConfirmationModel>> Get()
        {
            var responce = await _service.Get();
            return responce;
        }

        // GET: api/ConfirmationModels/5
        public async Task<ConfirmationModel> GetById(Guid id)
        {
            ConfirmationModel model = await _service.GetbyId(id);

            return model;
        }

        [System.Web.Http.HttpPost]//[OPTIMIZE]
        public async Task<bool> WeeklyTravelsUpdate(WeeklyTravelsModel model)
        {
            try
            {
                ClientModel client = await _serviceClient.GetClientByName(User.Identity.Name);

                var confirmationsToRemove = (await _service.Get()).Where(v => v.TravelId.HasValue && model.RejectedTravels.Contains(v.TravelId.Value) && v.ClientId == client.Id);
                foreach (var item in confirmationsToRemove)
                {
                    try
                    {
                        await _service.Delete(item);
                    }
                    catch (Exception x)
                    { 
                    }
                }

                foreach (var item in model.ConfirmedTravels)
                {
                    ConfirmationModel newModel = new ConfirmationModel();
                    newModel.ClientId = client.Id;
                    newModel.TravelId = item;
                    newModel.IsPositive = true;
                    newModel.Id = await _service.Create(newModel);
                }


                return true;
            }
            catch (Exception x)
            {
            }
            return false;
        }

        public int FreeCancelationMinutes = 120;
        [System.Web.Http.HttpPost]//[OPTIMIZE]
        public async Task<bool> WeeklyTravelsPreferencesUpdate(WeeklyTravelsPreferencesModel model)
        {
            try
            {
                ClientModel client = await _serviceClient.GetClientByName(User.Identity.Name);

                client.MinPassengers = model.MinPassengers;
                await _serviceClient.Update(client);

                var confirmationsToRemove = (await _service.Get()).Where(v => v.TravelId.HasValue && model.RejectedTravels.Contains(v.TravelId.Value) && v.ClientId == client.Id);
                var canceledTravels = (await _serviceTravel.Get(v => model.RejectedTravels.Contains(v.Id))).ToList();
                var NOW = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));


                foreach (var tr in canceledTravels)
                {
                    if (tr.DateTime >= NOW.AddMinutes(FreeCancelationMinutes))
                    {
                        try
                        {
                            await _service.Delete(tr.Confirmations.First(v => v.ClientId == client.Id));
                            /*client.Balancce += tr.Price;
                            await _serviceClient.Update(client);*/
                        }
                        catch (Exception x)
                        {
                        }
                    }

                }

                foreach (var item in model.ConfirmedTravels)
                {
                    ConfirmationModel newModel = new ConfirmationModel();
                    newModel.ClientId = client.Id;
                    newModel.TravelId = item;
                    newModel.IsPositive = true;
                    newModel.Id = await _service.Create(newModel);
                }

                TravelsActualizationJob job = new TravelsActualizationJob();
                
                var ConfirmedTravels = (await _serviceTravel.Get(v => model.ConfirmedTravels.Contains(v.Id))).ToList().Select(v=>v.ShuttleId).Distinct();
                
                foreach (var shttle in ConfirmedTravels.Where(m=>m.HasValue))
                {
                     var travelInShuttle = (await _serviceTravel.Get(v => v.ShuttleId == shttle && v.DateTime > NOW));
                     var clientsInShuttle = travelInShuttle.Select(v=>v.Confirmations.FirstOrDefault()).Where(b=>b != null).Select(b=>b.Client).ToList();

                     var dayShuttle = travelInShuttle.GroupBy(b=>b.DateTime.Value.Date);
                    
                    await job.UpdateBackBalanceForShuttleClients(shttle.Value);
                    await job.UpdatePricesForShuttle(shttle.Value);
                    await job.UpdateBalanceForShuttleClients(shttle.Value);
                }
                return true;
            }
            catch (Exception x)
            {
            }
            return false;
        }

        // PUT: api/ConfirmationModels/5
        public async Task<ConfirmationModel> Update(ConfirmationModel model)
        {
            try
            {
                ClientModel client = await _serviceClient.GetClientByName(User.Identity.Name);
                model.ClientId = client.Id;
                await _service.Update(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await ConfirmationModelExists(model.Id)))
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

        // POST: api/ConfirmationModels
        public async Task<ConfirmationModel> Create(ConfirmationModel model)
        {

            ClientModel client = await _serviceClient.GetClientByName(User.Identity.Name);
            model.ClientId = client.Id;
            model.Id = await _service.Create(model);
            ConfirmationModel newmodel = await _service.GetbyId(model.Id);

            return newmodel;
        }

        // DELETE: api/ConfirmationModels/5
        public async Task<bool> Delete(Guid id)
        {
            ConfirmationModel model = await _service.GetbyId(id);
            if (model == null)
            {
                return false;
            }
            await _service.Delete(model);

            return true;
        }

        private async Task<bool> ConfirmationModelExists(Guid id)
        {
            return (await _service.GetbyId(id)) != null;
        }
    }
}