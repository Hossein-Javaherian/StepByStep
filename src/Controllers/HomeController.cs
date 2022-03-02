using System.Web.Mvc;
using System.Web.Routing;
using StepByStep.Infrastructure;
using StepByStep.Models;

namespace StepByStep.Controllers
{
    public class HomeController : Controller
    {
        private SessionManager sessionManager;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            sessionManager = new SessionManager(Session);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Bio()
        {
            var bio = sessionManager.Get<Bio>();
            var model = new Bio
            {
                Name = bio.Name,
                Family = bio.Family
            };

            ViewBag.Bio = Session.Serialize<Bio>();
            ViewBag.Old = Session.Serialize<Old>();
            ViewBag.Contact = Session.Serialize<Contact>();

            return View(model);
        }

        [HttpPost]
        public ActionResult Bio(Bio model)
        {
            if (ModelState.IsValid)
            {
                var bio = sessionManager.Get<Bio>();
                bio.Name = model.Name;
                bio.Family = model.Family;

                return RedirectToAction("Old");
            }

            return View(model);
        }

        public ActionResult Old()
        {
            var old = sessionManager.Get<Old>();
            var model = new Old
            {
                Age = old.Age
            };

            ViewBag.Old = Session.Serialize<Old>();

            return View(model);
        }

        [HttpPost]
        public ActionResult Old(Old model, string prev, string next)
        {
            var old = sessionManager.Get<Old>();

            if (prev != null)
            {
                if (ModelState.IsValid)
                {
                    old.Age = model.Age;

                    return RedirectToAction("Bio");
                }
            }

            if (next != null)
            {
                if (ModelState.IsValid)
                {
                    old.Age = model.Age;

                    return RedirectToAction("Contact");
                }
            }

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Success()
        {
            ViewBag.Message = "Success add ali.";

            return View();
        }

        public ActionResult Contact()
        {
            var bio = sessionManager.Get<Bio>();
            if (string.IsNullOrEmpty(bio.Name) || string.IsNullOrEmpty(bio.Family))
                return RedirectToAction("Bio");

            var contact = sessionManager.Get<Contact>();
            var model = new Contact
            {
                Phone = contact.Phone,
                Fax = contact.Fax
            };

            ViewBag.Bio = Session.Serialize<Bio>();
            ViewBag.Contact = Session.Serialize<Contact>();
            ViewBag.Old = Session.Serialize<Old>();

            return View(model);
        }

        [HttpPost]
        public ActionResult Contact(Contact model, string prev)
        {
            var old = sessionManager.Get<Old>();
            var bio = sessionManager.Get<Bio>();
            var contact = sessionManager.Get<Contact>();

            if (prev != null)
            {
                if (ModelState.IsValid)
                {
                    contact.Phone = model.Phone;
                    contact.Fax = model.Fax;

                    //Update(contact);

                    return RedirectToAction("Old");
                }

                return View(model);
            }

            if (ModelState.IsValid)
            {
                contact.Phone = model.Phone;
                contact.Fax = model.Fax;

                // mapping model to domain
                Domains.Entity entity = new Domains.Entity
                {
                    Name = bio.Name,
                    Family = bio.Family,
                    Age = old.Age,
                    Phone = contact.Phone,
                    Fax = contact.Fax
                };

                // Do any work

                // Clear session
                sessionManager.Remove<Bio>();
                sessionManager.Remove<Contact>();

                return RedirectToAction("Success");
            }

            return View(model);
        }
    }
}