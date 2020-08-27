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
using TransfersApp.Services;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using Newtonsoft.Json;
using TransfersApp.Models.SearchModels;

namespace TransfersApp.Controllers
{
    public class RallyPointController : Controller
    {
        private IServiceBase<RallyPointModel, Guid> _service;
        public RallyPointController(IServiceBase<RallyPointModel, Guid> service)
        {
            _service = service;
        }
        // GET: RallyPoint
        public async Task<ActionResult> Index()
        {
            ViewBag.Items = await _service.Get();
            return View(new RallyPointSearchModel());

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index([Bind(Include = "NameSearchText")] RallyPointSearchModel model)
        {
            Func<RallyPointModel, bool> search = (RallyPoint) => {
                bool result = false;
                if (!string.IsNullOrEmpty(RallyPoint.Description))
                {
                    result = result || RallyPoint.Description.ToLower().Contains(model.NameSearchText.ToLower());
                }
                if (!string.IsNullOrEmpty(RallyPoint.Coordinates))
                {
                    result = result || RallyPoint.Coordinates.ToLower().Contains(model.NameSearchText.ToLower());
                }
                return result;
            };
            if (model != null && !string.IsNullOrEmpty(model.NameSearchText))
                ViewBag.Items = await _service.Get(search);
            else
                ViewBag.Items = await _service.Get();
            if (model == null)
                model = new RallyPointSearchModel();

            return View(model);
        }
        // GET: RallyPoint/Details/5
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

        // GET: RallyPoint/Create
        public ActionResult Create()
        {
            return View();
        }


        public ActionResult ViewMap()
        {
            return View();
        }

        public string GetFormattedAdddress(string country, string city, string street, string streettype, string number)
        {
            var address = country + ", " + city + ", " + street;
            if (!(string.IsNullOrEmpty(streettype) || string.IsNullOrWhiteSpace(streettype)))
            {
                address += " " + streettype;
            }
            if (!(string.IsNullOrEmpty(number) || string.IsNullOrWhiteSpace(number)))
            {
                address += ", " + number;
            }
            return address;
        }



        public async Task<Locations> GetCoordinates(string address)
        {
            Locations coordsForAddress = null;
            var AddressEncoded = HttpUtility.UrlEncode(address);
            var Url = "geocode/v1/json?q=" + AddressEncoded + "&key=1378cc8710124cfb92411c7ff2c9e9b1";
            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.BaseAddress = new Uri("https://api.opencagedata.com/");
                var responce = await httpclient.GetAsync(Url);
                var dataLocations = await responce.Content.ReadAsStringAsync();
                if (responce.IsSuccessStatusCode)
                {
                    coordsForAddress = JsonConvert.DeserializeObject<Locations>(dataLocations);
                }
            }
            return coordsForAddress;
        }
        public async Task<ActionResult> GetFile()
        {
            //var Address = GetFormattedAdddress("Україна", "Львів", "Кульпарківська", "", "222");
            var Address = GetFormattedAdddress("Russia", "Санкт-Петербург", " Колпино, Финляндская", "улица  ", "22ж");
            Locations coordsForAddress = await GetCoordinates(Address);

            int i = 1;
            string name = Server.MapPath("~/Content/" + string.Format("RallyPoints-{0}.csv", DateTime.Now.ToString("yyyyMMdd-HHmmss")));
            var data = (await _service.Get()).Where(y => y.Coordinates.Contains(',')).Select(v => {
                var coords = v.Coordinates.Split(',');
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
                    foreach (var pint in coordsForAddress.Results.Where(v => v.components.country_code.ToLower().Contains("ru")))
                    {
                        writer.WriteLine($"{pint.geometry.lat},{pint.geometry.lng}, \"{Address}\", \"Address from geocoding\" , \"{i++}\"");
                    }
                }
            }
            return File(name, "text/csv", string.Format("RallyPoints-{0}.csv", DateTime.Now.ToString("yyyyMMdd-HHmmss")));
            
        }
        public async Task<ActionResult> ExportToExcel()
        {/*
            var gv = new GridView();
            int i = 1;
           
            gv.DataSource = data;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xlsx");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();*/
            return RedirectToAction("Index");
        }
        // POST: RallyPoint/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Coordinates,Description,IsProxy,IsDeleted")] RallyPointModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.Create(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: RallyPoint/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
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

        // POST: RallyPoint/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Coordinates,Description,IsProxy,IsDeleted")] RallyPointModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.Update(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: RallyPoint/Delete/5
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

        // POST: RallyPoint/Delete/5
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
