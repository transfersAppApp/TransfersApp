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
using TransfersApp.Services.Interfaces;

namespace TransfersApp.Controllers
{
    public class ConfirmationController : Controller
    {
        
        private IServiceBase<ConfirmationModel, Guid> _service;
        public ConfirmationController(IServiceBase<ConfirmationModel, Guid> service)
        {
            _service = service;
        }
        // GET: Confirmation
        public async Task<ActionResult> Index()
        {
            // var confirmationModels = _service.ConfirmationModels.Include(c => c.Client).Include(c => c.Travel);
            return View(await _service.Get());

        }

        // GET: Confirmation/Details/5
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

        // GET: Confirmation/Create
        public async Task<ActionResult> Create()
        {
            var _serviceClient = (IServiceBase<ClientTransferModel, Guid>)DependencyResolver.Current.GetService(typeof(IServiceBase<ClientTransferModel, Guid>));
            var ClientModels = await _serviceClient.Get();
            ViewBag.ClientId = new SelectList(ClientModels, "Id", "FirstName");

            var _serviceTravel = (ITravelService)DependencyResolver.Current.GetService(typeof(ITravelService));
            var TravelModels = await _serviceTravel.Get();
            ViewBag.TravelId = new SelectList(TravelModels, "Id", "DisplayName");

            return View();
        }

        // POST: Confirmation/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ClientId,Text,IsPositive,TravelId")] ConfirmationModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.Create(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Confirmation/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConfirmationModel confirmationModel = await _service.GetbyId(id.Value);
            if (confirmationModel == null)
            {
                return HttpNotFound();
            }
            var _serviceClient = (IServiceBase<ClientTransferModel, Guid>)DependencyResolver.Current.GetService(typeof(IServiceBase<ClientTransferModel, Guid>));
            var ClientModels = await _serviceClient.Get();

            ViewBag.ClientId = new SelectList(ClientModels, "Id", "FirstName");

            var _serviceTravel = (ITravelService)DependencyResolver.Current.GetService(typeof(ITravelService));
            var TravelModels = await _serviceTravel.Get();
            ViewBag.TravelId = new SelectList(TravelModels, "Id", "DisplayName");
            return View(confirmationModel);
        }

        // POST: Confirmation/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ClientId,Text,IsPositive,TravelId")] ConfirmationModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.Update(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Confirmation/Delete/5
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

        // POST: Confirmation/Delete/5
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
