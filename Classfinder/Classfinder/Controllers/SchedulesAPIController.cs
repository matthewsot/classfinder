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
        [Route("API/Classes/{school}/{period}/{searchTerm}")]
        [AllowAnonymous]
        public IHttpActionResult SearchClasses(string school, int period, string searchTerm)
        {
            var classes = db.Classes.Where(@class => @class.Name.Contains(searchTerm) && @class.Period == period && (@class.School == school || @class.School == null));

            return Ok(classes.Select(@class => new
            {
                name = @class.Name,
                id = @class.Id,
            }));
        }

        [HttpGet]
        [Route("API/Classes/{period}")]
        [AllowAnonymous]
        public IHttpActionResult SearchClasses(int period)
        {
            var classes = db.Classes.Where(@class => @class.Period == period).Take(10);

            return Ok(classes.Select(@class => new
            {
                name = @class.Name,
                id = @class.Id,
            }));
        }

        [HttpGet]
        [Route("API/Schedule/{userId}/{semester}/{period}")]
        [AllowAnonymous]
        public IHttpActionResult GetClassForPeriod(string userId, int semester, int period)
        {
            var user = db.Users.Find(userId);
            var schedule = semester == 2 ? user.SecondSemester : user.FirstSemester;
            var classInPeriod = schedule.FirstOrDefault(@class => @class.Period == period) ?? new Class { Name = "No Class" };

            return Ok(new ClassModel
            {
                name = classInPeriod.Name
            });
        }

        [HttpGet]
        [Route("API/Classmates/{userId}/{semester}/{period}")]
        [AllowAnonymous]
        public IHttpActionResult GetClassmatesForPeriod(string userId, int semester, int period)
        {
            var user = db.Users.Find(userId);
            var schedule = semester == 2 ? user.SecondSemester : user.FirstSemester;
            var classInPeriod = schedule.FirstOrDefault(@class => @class.Period == period) ?? new Class { Name = "No Class" };

            var classmates = semester == 2
                ? classInPeriod.StudentsInClassSecondSemester
                : classInPeriod.StudentsInClassFirstSemester;

            return Ok(new
            {
                name = classInPeriod.Name,
                classmates = classmates.Select(student => new
                {
                    realName = student.RealName,
                    userName = student.UserName,
                    grade = 12 - (student.GradYear - 2015)
                })
            });
        }

        public class ClassModel
        {
            public string name { get; set; }
        }

        [HttpGet]
        [Route("Admin/AddNoClasses")]
        [AllowAnonymous]
        public IHttpActionResult AddNoClasses()
        {
            for (var i = 1; i <= 7; i++)
            {
                int period = i;

                if (!db.Classes.Any(@class => @class.Period == period && @class.Name == "No Class"))
                {
                    db.Classes.Add(new Class
                    {
                        Name = "No Class",
                        Period = period
                    });
                }
            }
            db.SaveChanges();
            return Ok("Great!");
        }

        [HttpPost]
        [Route("API/Schedule/{userId}/{semester}/{period}")]
        public IHttpActionResult SetClassForPeriod(ClassModel model, string userId, int semester, int period)
        {
            var user = db.Users.Find(userId);
            var schedule = semester == 2 ? user.SecondSemester : user.FirstSemester;

            var currClassInPeriod = schedule.FirstOrDefault(@class => @class.Period == period);
            if (currClassInPeriod != null)
            {
                schedule.Remove(currClassInPeriod);
            }

            var classToAdd = db.Classes.FirstOrDefault(@class => @class.Name == model.name && @class.Period == period && (@class.School == user.School || @class.School == null));
            if (classToAdd != null)
            {
                schedule.Add(classToAdd);
            }
            else
            {
                var newClass = new Class {Name = model.name, Period = period, School = user.School};
                db.Classes.Add(newClass);
                schedule.Add(newClass);
            }

            if (currClassInPeriod != null &&
                currClassInPeriod.Name != "No Class" &&
                currClassInPeriod != classToAdd &&
                !currClassInPeriod.StudentsInClassFirstSemester.Any() &&
                !currClassInPeriod.StudentsInClassSecondSemester.Any())
            {
                db.Classes.Remove(currClassInPeriod);
            }

            if (semester == 1 && user.SignUpLevel < (int) SignUpLevel.FirstSemesterScheduleEntered)
            {
                user.SignUpLevel = (int) SignUpLevel.FirstSemesterScheduleEntered;
            }
            if (semester == 2 && user.SignUpLevel < (int)SignUpLevel.SecondSemesterScheduleEntered)
            {
                user.SignUpLevel = (int)SignUpLevel.SecondSemesterScheduleEntered;
            }

            db.SaveChanges();

            return Ok("GOOD");
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
