using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TransfersApp.Models;


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
using TransfersApp.Jobs;
using TransfersApp.Services.Interfaces;

namespace TransfersApp.Controllers
{
    public class ClientPaymentController : Controller
    {
        private IServiceBase<ClientPaymentModel, Guid> _service;
        TravelsActualizationJob updateJob;

        private IClientTransferService _serviceClientTransfers;
        private ITravelService _serviceTravel;

        public ClientPaymentController(IServiceBase<ClientPaymentModel, Guid> service, IClientTransferService serviceClientTransfers, ITravelService serviceTravel)
        {
            _service = service;
            updateJob = new TravelsActualizationJob();
            _serviceTravel = serviceTravel;
            _serviceClientTransfers = serviceClientTransfers;
        }
        // GET: ClientPayment
        public async Task<ActionResult> Index()
        {
            return View(await _service.Get());
        }

        // GET: ClientPayment/Details/5
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

        // GET: ClientPayment/Create
        public async Task<ActionResult> Create()
        {
            var _serviceClient = (IServiceBase<ClientPaymentModel, Guid>)DependencyResolver.Current.GetService(typeof(IServiceBase<ClientPaymentModel, Guid>));
            var ClientModels = await _serviceClient.Get();
            ViewBag.ClientId = new SelectList(ClientModels, "Id", "FirstName");

            var _servicePayment = (ITravelService)DependencyResolver.Current.GetService(typeof(ITravelService));
            var TravelModels = await _servicePayment.Get();
            ViewBag.TravelId = new SelectList(TravelModels, "Id", "Title");
            return View();
        }

        // POST: ClientPayment/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,PaymentId,ClientId")] ClientPaymentModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.Create(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: ClientPayment/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientPaymentModel clientPaymentModel = await _service.GetbyId(id.Value);
            if (clientPaymentModel == null)
            {
                return HttpNotFound();
            }
            var _serviceClient = (IServiceBase<ClientPaymentModel, Guid>)DependencyResolver.Current.GetService(typeof(IServiceBase<ClientPaymentModel, Guid>));
            var ClientModels = await _serviceClient.Get();
            ViewBag.ClientId = new SelectList(ClientModels, "Id", "FirstName");

            var _servicePayment = (ITravelService)DependencyResolver.Current.GetService(typeof(ITravelService));
            var TravelModels = await _servicePayment.Get();
            ViewBag.TravelId = new SelectList(TravelModels, "Id", "Title");
            return View(clientPaymentModel);
        }

        // POST: ClientPayment/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TravelId,ClientId")] ClientPaymentModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.Update(model);
                var updatedModel = await _serviceTravel.GetbyId(model.Id);


                var transfer = (await _serviceClientTransfers.Get())?.Where(v => v.ClientId == model.Id && v.IsActive && v.TransferId == updatedModel.TransferId)?.Select(n=>n.Transfer)?.FirstOrDefault();
                if (updateJob == null)
                    updateJob = new TravelsActualizationJob();
                if(transfer != null)
                await updateJob.ExecuteAddForTransfer(transfer);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: ClientPayment/Delete/5
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

        // POST: ClientPayment/Delete/5
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
