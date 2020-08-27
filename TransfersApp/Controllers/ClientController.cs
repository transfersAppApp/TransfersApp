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
using TransfersApp.Models.SearchModels;

namespace TransfersApp.Controllers
{
    public class ClientController : Controller
    {

        private IServiceBase<ClientModel, Guid> _service;
        public ClientController(IServiceBase<ClientModel, Guid> service)
        {
            _service = service;
        }
        // GET: Client
        public async Task<ActionResult> Index()
        {
            ViewBag.Items = await _service.Get();
            return View(new ClientSearchModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index([Bind(Include = "SearchText")] ClientSearchModel model)
        {

            Func<ClientModel, bool> search = (client) => {
                bool result = false;
                if (!string.IsNullOrEmpty(client.FirstName))
                {
                    result = result || client.FirstName.ToLower().Contains(model.SearchText.ToLower());
                }
                if (!string.IsNullOrEmpty(client.LastName))
                {
                    result = result || client.LastName.ToLower().Contains(model.SearchText.ToLower());
                }
                if (!string.IsNullOrEmpty(client.HomeAddress))
                {
                    result = result || client.HomeAddress.ToLower().Contains(model.SearchText.ToLower());
                }
                if (!string.IsNullOrEmpty(client.WorkAdress))
                {
                    result = result || client.WorkAdress.ToLower().Contains(model.SearchText.ToLower());
                }
                return result; };
            if (model != null && !string.IsNullOrEmpty(model.SearchText))
                ViewBag.Items = await _service.Get(search);
            else
                ViewBag.Items = await _service.Get();
            if (model == null)
                model = new ClientSearchModel();

            return View(model);
        }

        // GET: Client/Details/5
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

        // GET: Client/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Client/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserId,Id,FirstName,LastName,FullName,Birthday,Email,PhoneNumber,InsuranceSum,MinPassengers,HomeAddressLocaction,WorkAdressLocaction,HomeAddress,WorkAdress,WorkArrivingTime,WorkDepartureTime,Balancce")] ClientModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.Create(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Client/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientModel clientModel = await _service.GetbyId(id.Value);
            if (clientModel == null)
            {
                return HttpNotFound();
            }
            return View(clientModel);
        }

        // POST: Client/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserId,Id,FirstName,LastName,FullName,Email,PhoneNumber,Birthday,InsuranceSum,MinPassengers,HomeAddressLocaction,WorkAdressLocaction,HomeAddress,WorkAdress,WorkArrivingTime,WorkDepartureTime,Balancce")] ClientModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.Update(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Client/Delete/5
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

        // POST: Client/Delete/5
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
