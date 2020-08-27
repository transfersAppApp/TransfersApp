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
    public class ClientTransferController : Controller
    {
        private IClientTransferService _service;

        public ClientTransferController(IClientTransferService service)
        {
            _service = service;
        }
        // GET: ClientTransfer
        public async Task<ActionResult> Index()
        {
            return View(await _service.Get());
        }

        // GET: ClientTransfer/Details/5
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

        // GET: ClientTransfer/Create
        public async Task<ActionResult> Create()
        {
            var _serviceClient = (IServiceBase<ClientTransferModel, Guid>)DependencyResolver.Current.GetService(typeof(IServiceBase<ClientTransferModel, Guid>));
            var ClientModels = await _serviceClient.Get();
             ViewBag.ClientId = new SelectList(ClientModels, "Id", "FirstName");

            var _serviceTransfer = (ITransferService)DependencyResolver.Current.GetService(typeof(ITransferService));
            var TransferModels = await _serviceTransfer.Get();
            ViewBag.TransferId = new SelectList(TransferModels, "Id", "Title");
            return View();
        }

        // POST: ClientTransfer/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TransferId,ClientId")] ClientTransferModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.Create(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: ClientTransfer/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientTransferModel clientTransferModel = await _service.GetbyId(id.Value);
            if (clientTransferModel == null)
            {
                return HttpNotFound();
            }
            var _serviceClient = (IServiceBase<ClientTransferModel, Guid>)DependencyResolver.Current.GetService(typeof(IServiceBase<ClientTransferModel, Guid>));
            var ClientModels = await _serviceClient.Get();
            ViewBag.ClientId = new SelectList(ClientModels, "Id", "FirstName");

            var _serviceTransfer = (ITransferService)DependencyResolver.Current.GetService(typeof(ITransferService));
            var TransferModels = await _serviceTransfer.Get();
            ViewBag.TransferId = new SelectList(TransferModels, "Id", "Title");
            return View(clientTransferModel);
        }

        // POST: ClientTransfer/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TransferId,ClientId")] ClientTransferModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.Update(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: ClientTransfer/Delete/5
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

        // POST: ClientTransfer/Delete/5
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
