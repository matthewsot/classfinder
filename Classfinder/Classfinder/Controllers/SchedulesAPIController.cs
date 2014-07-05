using System.Linq;
using System.Web.Http;
using Classfinder.Models;

namespace Classfinder.Controllers
{
    [Authorize]
    public class SchedulesAPIController : ApiController
    {
        private CfDb db = new CfDb();

        [HttpGet]
        [Route("API/Schedule/{userId}/{semester}")]
        [AllowAnonymous]
        public IHttpActionResult GetSchedule(string userId, int semester)
        {
            var user = db.Users.Find(userId);
            var schedule = semester == 2 ? user.SecondSemester : user.FirstSemester;

            return Ok(schedule.Select(@class => new
            {
                name = @class.Name,
                period = @class.Period,
                id = @class.Id,
            }));
        }

        [HttpGet]
        [Route("API/Classes/{period}/{searchTerm}")]
        [AllowAnonymous]
        public IHttpActionResult SearchClasses(int period, string searchTerm)
        {
            var classes = db.Classes.Where(@class => @class.Name.Contains(searchTerm));

            return Ok(classes.Select(@class => new
            {
                name = @class.Name,
                id = @class.Id,
            }));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
