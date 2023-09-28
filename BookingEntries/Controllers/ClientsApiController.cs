using BookingEntries.Models;
using BookingEntries.ViewModels;
using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BookingEntries.Controllers
{
    [RoutePrefix("api/clientsApi")]
    public class ClientsApiController : ApiController
    {
        private BookingDbContext db = new BookingDbContext();

        // Get all Clients
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetClients()
        {
            var clients = await db.Clients
                .Include(c => c.bookingEntries.Select(b => b.Spot))
                .OrderByDescending(x => x.ClientId)
                .ToListAsync();

            return Ok(clients);
        }

        // Get Client By Id
        [HttpGet]
        [Route("client-info/{id}")]
        public async Task<IHttpActionResult> GetClientInfo(int id)
        {
            Client client = await db.Clients.FirstAsync(x => x.ClientId == id);
            var clientSpots = await db.BookingEntries.Where(x => x.ClientId == id).ToListAsync();

            ClientVM clientVM = new ClientVM()
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                Age = client.Age,
                BirthDate = client.BirthDate,
                Picture = client.Picture,
                MaritalStatus = client.MaritalStatus
            };
            if (clientSpots.Count() > 0)
            {
                foreach (var item in clientSpots)
                {
                    clientVM.SpotList.Add(item.SpotId);
                }
            }
            return Ok(clientVM);
        }

        // Get all Spots
        [HttpGet]
        [Route("spot-list")]
        public async Task<IHttpActionResult> GetSpots()
        {
            var spotList = await db.Spots.ToListAsync();

            return Ok(spotList);
        }


        // Get all Spots
        [HttpPost]
        [Route("create-booking")]
        public async Task<IHttpActionResult> CreateBooking()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("Unsupported media type.");
            }

            var uploadPath = HttpContext.Current.Server.MapPath("~/Images/");
            var provider = new MultipartFormDataStreamProvider(uploadPath);

            try
            {
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                await Request.Content.ReadAsMultipartAsync(provider);

                var viewModelJson = provider.FormData["clientVM"];
                var spotJson = provider.FormData["spotId"];

                var client = JsonConvert.DeserializeObject<Client>(viewModelJson);
                var spotId = JsonConvert.DeserializeObject<int[]>(spotJson);

                var imageFile = provider.FileData.FirstOrDefault();
                if (imageFile != null)
                {
                    var fileName = imageFile.Headers.ContentDisposition.FileName.Trim('\"');
                    string filePath = Path.Combine("/Images/", Guid.NewGuid() + Path.GetExtension(fileName));

                    var fileBytes = File.ReadAllBytes(imageFile.LocalFileName);
                    File.WriteAllBytes(HttpContext.Current.Server.MapPath(filePath), fileBytes);
                    client.Picture = filePath;
                }

                //Save all spot from SpotId[]
                foreach (var item in spotId)
                {
                    BookingEntry bookingEntry = new BookingEntry()
                    {
                        Client = client,
                        ClientId = client.ClientId, //client will be created and refer the ClientId
                        SpotId = item
                    };
                    db.BookingEntries.Add(bookingEntry);
                }

                await db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // Update Booking
        [HttpPost]
        [Route("update-booking")]
        public async Task<IHttpActionResult> UpdateBooking()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("Unsupported media type.");
            }

            var uploadPath = HttpContext.Current.Server.MapPath("~/Images/");
            var provider = new MultipartFormDataStreamProvider(uploadPath);

            try
            {
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                await Request.Content.ReadAsMultipartAsync(provider);

                var viewModelJson = provider.FormData["clientVM"];
                var spotJson = provider.FormData["spotId"];

                var client = JsonConvert.DeserializeObject<Client>(viewModelJson);
                var spotId = JsonConvert.DeserializeObject<int[]>(spotJson);

                var imageFile = provider.FileData.FirstOrDefault();
                if (imageFile != null)
                {
                    var fileName = imageFile.Headers.ContentDisposition.FileName.Trim('\"');
                    string filePath = Path.Combine("/Images/", Guid.NewGuid() + Path.GetExtension(fileName));

                    var fileBytes = File.ReadAllBytes(imageFile.LocalFileName);
                    File.WriteAllBytes(HttpContext.Current.Server.MapPath(filePath), fileBytes);
                    client.Picture = filePath;
                }

                var existsSpotEntry = await db.BookingEntries
                    .Where(x => x.ClientId == client.ClientId)
                    .ToListAsync();

                //Delete all spot from exist booking
                foreach (var bookingEntry in existsSpotEntry)
                {
                    db.BookingEntries.Remove(bookingEntry);
                }

                //Save all spot from SpotId[]
                foreach (var item in spotId)
                {
                    BookingEntry bookingEntry = new BookingEntry()
                    {
                        Client = client,
                        ClientId = client.ClientId, //client will be created and refer the ClientId
                        SpotId = item
                    };
                    db.BookingEntries.Add(bookingEntry);
                }

                db.Entry(client).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // Delete Booking
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IHttpActionResult> DeleteBooking(int id)
        {
            var client = db.Clients.Find(id);
            var existsSpotEntry = await db.BookingEntries
                .Where(x => x.ClientId == id)
                .ToListAsync();

            foreach (var bookingEntry in existsSpotEntry)
            {
                db.BookingEntries.Remove(bookingEntry);
            }

            db.Entry(client).State = EntityState.Deleted;
            await db.SaveChangesAsync();

            return Ok();
        }

    }
}
