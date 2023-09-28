using BookingEntries.Models;
using BookingEntries.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookingEntries.Controllers
{
    public class ClientsController : Controller
    {
        private BookingDbContext db = new BookingDbContext();
        private readonly HttpClient client = new HttpClient();

        private const string BASE_URL = "http://localhost:54621/api/ClientsApi/";

        // GET: Clients
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(BASE_URL);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var clients = JsonConvert.DeserializeObject<List<Client>>(responseBody);

            return View(clients);
        }

        public ActionResult AddNewSpot(int? id)
        {
            HttpResponseMessage response = client.GetAsync(BASE_URL + "spot-list").Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;

            var spots = JsonConvert.DeserializeObject<List<Spot>>(responseBody);

            ViewBag.spots = new SelectList(spots, "SpotId", "SpotName", (id != null) ? id.ToString() : "");
            return PartialView("_addNewSpot");
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ClientVM clientVM, int[] SpotId)
        {
            if (ModelState.IsValid)
            {
                var formData = new MultipartFormDataContent();

                string clientJson = JsonConvert.SerializeObject(clientVM);
                formData.Add(new StringContent(clientJson, Encoding.UTF8, "application/json"), "clientVM");

                string spotJson = JsonConvert.SerializeObject(SpotId);
                formData.Add(new StringContent(spotJson, Encoding.UTF8, "application/json"), "spotId");

                StreamContent imageContent = new StreamContent(clientVM.PictureFile.InputStream);
                imageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "PictureFile",
                    FileName = clientVM.PictureFile.FileName
                };
                formData.Add(imageContent);

                HttpResponseMessage response = client.PostAsync(BASE_URL + "create-booking", formData).Result;
                response.EnsureSuccessStatusCode();

                string responseBody = response.Content.ReadAsStringAsync().Result;

                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Edit(int? id)
        {
            HttpResponseMessage response = client.GetAsync(BASE_URL + "client-info/" + id).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;

            var clientData = JsonConvert.DeserializeObject<ClientVM>(responseBody);
            return View(clientData);
        }

        [HttpPost]
        public ActionResult Edit(ClientVM clientVM, int[] SpotId)
        {
            if (ModelState.IsValid)
            {
                var formData = new MultipartFormDataContent();

                string clientJson = JsonConvert.SerializeObject(clientVM);
                formData.Add(new StringContent(clientJson, Encoding.UTF8, "application/json"), "clientVM");

                string spotJson = JsonConvert.SerializeObject(SpotId);
                formData.Add(new StringContent(spotJson, Encoding.UTF8, "application/json"), "spotId");

                StreamContent imageContent = new StreamContent(clientVM.PictureFile.InputStream);
                imageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "PictureFile",
                    FileName = clientVM.PictureFile.FileName
                };
                formData.Add(imageContent);

                HttpResponseMessage response = client.PostAsync(BASE_URL + "update-booking", formData).Result;
                response.EnsureSuccessStatusCode();

                string responseBody = response.Content.ReadAsStringAsync().Result;

                return RedirectToAction("Index");
            }

            HttpResponseMessage editResponse = client.GetAsync(BASE_URL + "client-info/" + clientVM.ClientId).Result;
            editResponse.EnsureSuccessStatusCode();
            string editResponseBody = editResponse.Content.ReadAsStringAsync().Result;

            var clientData = JsonConvert.DeserializeObject<ClientVM>(editResponseBody);
            return View(clientData);
        }

        public ActionResult Delete(int? id)
        {
            HttpResponseMessage response = client.DeleteAsync(BASE_URL + "delete/" + id).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;

            return RedirectToAction("Index");
        }
    }
}