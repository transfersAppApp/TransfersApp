using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
    public class TransferApiController : ApiController
    {
        private IClientTransferService _serviceClientTransfer;
        private ITravelService _serviceTravel;
        private IServiceBase<ConfirmationModel, Guid> _serviceTravelConfirmation;
        TravelsActualizationJob updateJob;
        
        private IClientService _serviceClient;
        private ITransferService _service;
        public TransferApiController()
        {
            _service = (ITransferService)DependencyResolver.Current.GetService(typeof(ITransferService));
            _serviceClientTransfer = (IClientTransferService)DependencyResolver.Current.GetService(typeof(IClientTransferService));
            _serviceClient = (IClientService)DependencyResolver.Current.GetService(typeof(IClientService));
            _serviceTravel = (ITravelService)DependencyResolver.Current.GetService(typeof(ITravelService));
            _serviceTravelConfirmation = (IServiceBase<ConfirmationModel, Guid>)DependencyResolver.Current.GetService(typeof(IServiceBase<ConfirmationModel, Guid>));

        }
        // GET: api/TransferApi
        public async Task<IEnumerable<TransferModel>> Get()
        {
            var responce = await _service.Get();
            return responce;
        }

        // GET: api/TransferApi/5
        public async Task<TransferModel> GetbyId(Guid id)
        {
            TransferModel model = await _service.GetbyId(id);

            return model;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Authorize]
        //[OPTIMIZE] - done
        public async Task<IEnumerable<TransferViewModel>> GetTransfers()
        {
            try
            {
                return await _serviceTravel.GetTransfersList(User.Identity.Name);
            }
            catch (Exception x)
            {
                Console.WriteLine("Exception GetTransfersModel: " + x.Message);

            }

            return null;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Authorize]
        //[OPTIMIZE] - done
        public async Task<ComplexTravelsModel> GetTransfersModel()
        {
            try
            {
                ComplexTravelsModel responce = new ComplexTravelsModel();


                responce = await _serviceTravel.GetComplexTravelsModel(User.Identity.Name);

                return responce;
            }
            catch (Exception x)
            {
                Console.WriteLine("Exception GetTransfersModel: " + x.Message);
                var test = JsonConvert.SerializeObject(x.InnerException);

            }

            return null;
        }


        [System.Web.Http.Authorize]
        // PUT: api/TransferApi/5
        public async Task<TransferModel> Update(TransferModel model)
        {

            try
            {
                await _service.Update(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await TransferModelExists(model.Id)))
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

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        // PUT: api/TransferApi/5
        public async Task<bool> UpdateUserTransfers(ClientTransfersUpdateModel model)
        {
            ClientModel client = await _serviceClient.GetClientByName(User.Identity.Name);

            client.WorkAdress = model.WorkAdress;
            client.HomeAddress = model.HomeAddress;
            client.WorkArrivingTime = model.WorkArrivingTime;
            client.WorkDepartureTime = model.WorkDepartureTime;
            client.WorkAdressLocaction = model.WorkAdressLocaction;
            client.HomeAddressLocaction = model.HomeAddressLocaction;

            await _serviceClient.Update(client);

            List<ClientTransferModel> clientTransfers = (await _serviceClientTransfer.Get()).Where(b => b.ClientId == client.Id).ToList();
            List<TransferModel> transfeers = clientTransfers.Select(g => g.Transfer).ToList();
            foreach (var t in clientTransfers)
            {
                await _serviceClientTransfer.Delete(t);
            }
            foreach (var t in transfeers)
            {
                await _service.Delete(t);
            }

            var _serviceTariff = (IServiceBase<TariffModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TariffModel, int>));
            var _serviceTransferStatus = (IServiceBase<TransferStatusModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TransferStatusModel, int>));
            try
            {
                var HomeWorkTransfer = new TransferModel();
                HomeWorkTransfer.MinimumClassId = (await _serviceTariff.Get()).FirstOrDefault().Id;
                HomeWorkTransfer.StatusId = (await _serviceTransferStatus.Get()).FirstOrDefault().Id;
                HomeWorkTransfer.Title = "На работу";
                HomeWorkTransfer.From = client.HomeAddress;
                HomeWorkTransfer.Destination = client.WorkAdress;
                var timeW = client.WorkArrivingTime.Split(':');
                int minutesW = 0;
                int.TryParse(timeW[1], out minutesW);
                int hoursW = 0;
                int.TryParse(timeW[0], out hoursW);
                HomeWorkTransfer.ArrivalTime = DateTime.Today.AddHours(hoursW).AddMinutes(minutesW);
                
                var WorkHomeTransfer = new TransferModel();
                WorkHomeTransfer.MinimumClassId = (await _serviceTariff.Get()).FirstOrDefault().Id;
                WorkHomeTransfer.StatusId = (await _serviceTransferStatus.Get()).FirstOrDefault().Id;
                WorkHomeTransfer.Title = "Домой";
                WorkHomeTransfer.From = client.WorkAdress;
                WorkHomeTransfer.Destination = client.HomeAddress;

                var timeH = client.WorkDepartureTime.Split(':');
                int minutesH = 0;
                int.TryParse(timeH[1], out minutesH);
                int hoursH = 0;
                int.TryParse(timeH[0], out hoursH);
                WorkHomeTransfer.DepartureTime = DateTime.Today.AddHours(hoursH).AddMinutes(minutesH);

                WorkHomeTransfer.Id = await _service.Create(WorkHomeTransfer);
                HomeWorkTransfer.Id = await _service.Create(HomeWorkTransfer);

                var HomeTransferWorkTransfer = new ClientTransferModel();
                HomeTransferWorkTransfer.ClientId = client.Id;
                HomeTransferWorkTransfer.TransferId = HomeWorkTransfer.Id;
                HomeTransferWorkTransfer.IsActive = true;

                var HomeWorkTransferTransfer = new ClientTransferModel();
                HomeWorkTransferTransfer.ClientId = client.Id;
                HomeWorkTransferTransfer.TransferId = WorkHomeTransfer.Id;
                HomeWorkTransferTransfer.IsActive = true;

                await _serviceClientTransfer.Create(HomeTransferWorkTransfer);
                await _serviceClientTransfer.Create(HomeWorkTransferTransfer);

                return true;
            }
            catch (Exception x)
            {
                
            }
            return false;
        }

        // POST: api/TransferApi
        [ResponseType(typeof(TransferModel))]
        [System.Web.Http.Authorize]
        public async Task<TransferModel> Create(TransferModel model)
        {
            try
            {
                ClientModel client = await _serviceClient.GetClientByName(User.Identity.Name);

                model.Id = await _service.Create(model);
                TransferModel newmodel = await _service.GetbyId(model.Id);

                var clientTransfer = new ClientTransferModel();
                clientTransfer.ClientId = client.Id;
                clientTransfer.IsActive = true;
                clientTransfer.TransferId = model.Id;
                await _serviceClientTransfer.Create(clientTransfer);

                return newmodel;
            }
            catch (Exception x)
            { 
            
            }
            return null;
        }

        // DELETE: api/TransferApi/5
        public async Task<bool> Delete(Guid id)
        {
            TransferModel model = await _service.GetbyId(id);
            if (model == null)
            {
                return false;
            }
            await _service.Delete(model);

            return true;
        }
        private async Task<bool> TransferModelExists(Guid id)
        {
            return (await _service.GetbyId(id)) != null;
        }
    }
}