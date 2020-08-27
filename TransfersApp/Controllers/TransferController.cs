using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TransfersApp.BL.Models;
using TransfersApp.Models;
using TransfersApp.Services;
using System.IO;
using TransfersApp.Services.Interfaces;
using TransfersApp.Jobs;
using TransfersApp.Models.SearchModels;

namespace TransfersApp.Controllers
{
    public class TransferController : Controller
    {

        private ITransferService _service;
        private ITravelService _serviceTravel;
        private IServiceBase<RallyPointModel, Guid> _serviceRallyPoint;
        private IClientTransferService _serviceClientTransfer;
        private IClientService _serviceClient;

        public TransferController(ITransferService service, ITravelService serviceTravel, IServiceBase<RallyPointModel, Guid> serviceRallyPoint,IClientTransferService serviceClientTransfer, IClientService serviceClient)
        {
            _service = service;
            _serviceClient = serviceClient;
            _serviceRallyPoint = serviceRallyPoint;
            _serviceTravel = serviceTravel;
            _serviceClientTransfer = serviceClientTransfer;
        }
        // GET: Transfer
        public async Task<ActionResult> Index()
        {

            List<TransferViewModel> transfers = (await _service.GetAllForView()).ToList();
            List<ClientTransferModel> clientTransfers = (await _serviceClientTransfer.GetAllForView()).ToList();

            foreach (var clientTr in clientTransfers)
            {
                var tr = transfers.FirstOrDefault(v => v.Id == clientTr.TransferId);
                if (tr != null)
                {
                    tr.IsActive = clientTr.IsActive;
                    tr.Client = clientTr.Client;
                }
            }
            ViewBag.Items = transfers;
            var _serviceTransferStatusModel = (IServiceBase<TransferStatusModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TransferStatusModel, int>));
            var TransferStatuses = (await _serviceTransferStatusModel.Get()).Select(v => new { Id = v.Id.ToString(), Name = v.Name }).ToList();
            TransferStatuses.Add(new { Id = "All", Name = "All" });
            ViewBag.StatusId = new SelectList(TransferStatuses, "Id", "Name", "All");


            var _serviceShuttle = (IShuttleService)DependencyResolver.Current.GetService(typeof(IShuttleService));
            var Shuttles = (await _serviceShuttle.Get()).Select(v => new { Id = v.Id.ToString(), Name = v.Name }).ToList();
            Shuttles.Add(new { Id = "All", Name = "All" });
            Shuttles.Add(new { Id = "ShuttleNotAssigned", Name = "Shuttle Not Assigned" });
            ViewBag.ShuttleId = new SelectList(Shuttles, "Id", "Name", "All");

            return View(new TransferSearchModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index([Bind(Include = "NameSearchText,ShuttleId,StatusId")] TransferSearchModel model)
        {
            List<TransferViewModel> transfers = (await _service.GetAllForView()).ToList();
            List<ClientTransferModel> clientTransfers = (await _serviceClientTransfer.GetAllForView()).ToList();

            foreach (var clientTr in clientTransfers)
            {
                var tr = transfers.FirstOrDefault(v => v.Id == clientTr.TransferId);
                if (tr != null)
                {
                    tr.IsActive = clientTr.IsActive;
                    tr.Client = clientTr.Client;
                }
            }
            if (model.StatusId == null) model.StatusId = "All";
            if (model.ShuttleId == null) model.ShuttleId = "All";


            Func<TransferViewModel, bool> search = (transfer) =>
            {
                bool result = false;
                if (!string.IsNullOrEmpty(model.NameSearchText))
                {
                    if (!string.IsNullOrEmpty(transfer.From))
                    {
                        result = result || transfer.From.ToLower().Contains(model.NameSearchText.ToLower());
                    }
                    if (!string.IsNullOrEmpty(transfer.Destination))
                    {
                        result = result || transfer.Destination.ToLower().Contains(model.NameSearchText.ToLower());
                    }
                    if (!string.IsNullOrEmpty(transfer.Title))
                    {
                        result = result || transfer.Title.ToLower().Contains(model.NameSearchText.ToLower());
                    }
                    if (!string.IsNullOrEmpty(transfer.Client?.FirstName))
                    {
                        result = result || transfer.Client.FirstName.ToLower().Contains(model.NameSearchText.ToLower());
                    }
                    if (!string.IsNullOrEmpty(transfer.Client?.LastName))
                    {
                        result = result || transfer.Client.LastName.ToLower().Contains(model.NameSearchText.ToLower());
                    }
                }
                else
                    result = true;

                if (model.StatusId != "All")
                {
                    if (transfer.StatusId.HasValue)
                        result = result && transfer.StatusId == int.Parse(model.StatusId);
                    else
                        result = false;
                }
                if (model.ShuttleId != "All")
                {
                    if (model.ShuttleId == "ShuttleNotAssigned")
                        result = result && !transfer.ShuttleId.HasValue;
                    else
                    if (transfer.ShuttleId.HasValue)
                    {
                        result = result && transfer.ShuttleId == Guid.Parse(model.ShuttleId);
                    }
                    else
                    {
                        return false;
                    }
                }
                return result;
            };

            if (model != null)
                ViewBag.Items = transfers.Where(search).ToList();
            else
                ViewBag.Items = transfers;

            if (model == null)
                model = new TransferSearchModel();

            var _serviceTransferStatusModel = (IServiceBase<TransferStatusModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TransferStatusModel, int>));
            var TransferStatuses = (await _serviceTransferStatusModel.Get()).Select(v => new { Id = v.Id.ToString(), Name = v.Name }).ToList();
            TransferStatuses.Add(new { Id = "All", Name = "All" });
            ViewBag.StatusId = new SelectList(TransferStatuses, "Id", "Name", "All");


            var _serviceShuttle = (IShuttleService)DependencyResolver.Current.GetService(typeof(IShuttleService));
            var Shuttles = (await _serviceShuttle.Get()).Select(v => new { Id = v.Id.ToString(), Name = v.Name }).ToList();
            Shuttles.Add(new { Id = "All", Name = "All" });
            Shuttles.Add(new { Id = "ShuttleNotAssigned", Name = "Shuttle Not Assigned" });
            ViewBag.ShuttleId = new SelectList(Shuttles, "Id", "Name", "All");

            return View(model);
        }

        public async Task<ActionResult> GetFile(Guid transferId)
        {

            List<ClientTransferModel> clientTransfers = (await _serviceClientTransfer.Get()).Where(b => b.TransferId == transferId).ToList();
            if (clientTransfers == null || !clientTransfers.Any())
            {
                return RedirectToAction("Index");

            }

            var client = clientTransfers.First().Client;
            if (client == null)
            {
                client = await _serviceClient.GetbyId(clientTransfers.First().ClientId.Value);
            }
            var homeCooddrs = client.HomeAddressLocaction.Replace(':', ',').Split(',');
            var workCooddrs = client.WorkAdressLocaction.Replace(':', ',').Split(',');
            if (workCooddrs.Length == 2 && homeCooddrs.Length == 2)
            {
                var clientHomeLat = homeCooddrs[0];
                var clientHomeLng = homeCooddrs[1];
                var clientWorkLat = workCooddrs[0];
                var clientWorkLng = workCooddrs[1];
                int i = 1;
                string name = Server.MapPath("~/Content/" + string.Format("RallyPoints-{0}.csv", DateTime.Now.ToString("yyyyMMdd-HHmmss")));
                var data = (await _serviceRallyPoint.Get()).Where(y => y.Coordinates.Contains(',')).Select(v =>
                {
                    var coords = v.Coordinates.Replace(':', ',').Split(',');
                    return new { Latitude = coords[0], Longtitude = coords[1], Description = v.Description, Label = v.Id, PlacemarkNumber = i++.ToString() };
                });

                FileInfo info = new FileInfo(name);

                if (!info.Exists)
                {
                    using (StreamWriter writer = info.CreateText())
                    {
                        writer.WriteLine("Latitude,Longtitude,Description,Label,Placemark number");

                        foreach (var pint in data)
                        {
                            writer.WriteLine($"{pint.Latitude},{pint.Longtitude}, \"{pint.Description}\", \"{pint.Label}\" , \"{pint.PlacemarkNumber}\"");

                        }

                        writer.WriteLine($"{clientHomeLat},{clientHomeLng}, \"{client.HomeAddress}\", \"Home\" , \"{i++}\"");
                        writer.WriteLine($"{clientWorkLat},{clientWorkLng}, \"{client.WorkAdress}\", \"Work\" , \"{i++}\"");

                    }

                }
                return File(name, "text/csv", string.Format("RallyPoints-{0}->{1}-{2}.csv", client.HomeAddress.Replace(' ', '-').Replace('.', '-').Replace(',', '-'), client.WorkAdress.Replace(' ', '-').Replace('.', '-').Replace(',', '-'), DateTime.Now.ToString("yyyyMMdd-HHmmss")));
            }
            else
                return RedirectToAction("Index");
        }
        // GET: Transfer/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = await _service.GetbyId(id.Value);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Transfer/Create
        public async Task<ActionResult> Create()
        {
            var _serviceTransferStatusModel = (IServiceBase<TransferStatusModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TransferStatusModel, int>));
            var TransferStatuses = await _serviceTransferStatusModel.Get();
            ViewBag.StatusId = new SelectList(TransferStatuses, "Id", "Name");


            var _serviceShuttle = (IShuttleService)DependencyResolver.Current.GetService(typeof(IShuttleService));
            var Shuttles = await _serviceShuttle.Get();
            ViewBag.ShuttleId = new SelectList(Shuttles, "Id", "Name");

            return View();
        }

        // POST: Transfer/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,From,Destination,DepartureTime,ArrivalTime,StatusId,MinimumClassId,ShuttleId,Length,LengthFromLastRallyPoint,ShuttleStopOrder")] TransferModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.Create(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Transfer/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransferModel TransferModel = await _service.GetbyId(id.Value);
            if (TransferModel == null)
            {
                return HttpNotFound();
            }
            var _serviceTransferStatusModel = (IServiceBase<TransferStatusModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TransferStatusModel, int>));
            var TransferStatuses = await _serviceTransferStatusModel.Get();
            ViewBag.StatusId = new SelectList(TransferStatuses, "Id", "Name");

            var _serviceShuttle = (IShuttleService)DependencyResolver.Current.GetService(typeof(IShuttleService));
            var Shuttles = (await _serviceShuttle.Get()).Select(v => new { Id = v.Id.ToString(), Name = v.Name }).ToList();
            Shuttles.Add(new { Id = Guid.Empty.ToString(), Name = " " });
            ViewBag.ShuttleId = new SelectList(Shuttles, "Id", "Name", " ");

            return View(TransferModel);
        }

        // POST: Transfer/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,From,Destination,DepartureTime,ArrivalTime,StatusId,MinimumClassId,ShuttleId,Length,LengthFromLastRallyPoint,ShuttleStopOrder")] TransferModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ShuttleId.HasValue && model.ShuttleId.Value == Guid.Empty)
                    model.ShuttleId = null;
                await _service.Update(model);
                if (model.ShuttleId.HasValue)
                {
                    var travelsToUpdate = (await _serviceTravel.Get(v => v.TransferId == model.Id)).ToList();
                    foreach (var travel in travelsToUpdate)
                    {
                        if (model.ShuttleId.HasValue && model.ShuttleId.Value == Guid.Empty)
                        {
                            if (travel.ShuttleId != model.ShuttleId.Value)
                            {
                                travel.ShuttleId = model.ShuttleId.Value;
                                await _serviceTravel.Update(travel);
                            }
                        }
                    }


                    TravelsActualizationJob updateJob = new TravelsActualizationJob();
                    await updateJob.UpdatePricesForShuttle(model.ShuttleId.Value);
                }
                return RedirectToAction("Index");
            }
            var _serviceTransferStatusModel = (IServiceBase<TransferStatusModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TransferStatusModel, int>));
            var TransferStatuses = await _serviceTransferStatusModel.Get();
            ViewBag.StatusId = new SelectList(TransferStatuses, "Id", "Name");

            var _serviceShuttle = (IShuttleService)DependencyResolver.Current.GetService(typeof(IShuttleService));
            var Shuttles = (await _serviceShuttle.Get()).Select(v => new { Id = v.Id.ToString(), Name = v.Name }).ToList();
            Shuttles.Add(new { Id = Guid.Empty.ToString(), Name = " " });
            ViewBag.ShuttleId = new SelectList(Shuttles, "Id", "Name", " ");
            return View(model);
        }

        // GET: Transfer/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = await _service.GetbyId(id.Value);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Transfer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var model = await _service.GetbyId(id);
            await _service.Delete(model);
            return RedirectToAction("Index");
        }
    }
}
