using System.Web.Mvc;
using System.Web.Routing;
using StepByStep.Infrastructure;
using StepByStep.Models;

namespace StepByStep.Controllers
{
    public class HomeController : Controller
    {
        #region Fields

        private SessionManager sessionManager;

        #endregion

        #region Utilities

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            sessionManager = new SessionManager(Session);
        }

        #endregion

        #region Actions

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Success()
        {
            ViewBag.Message = "Success add entity.";

            return View();
        }

        #region Step-Bio (1)

        public ActionResult Bio()
        {
            var bio = sessionManager.Get<Bio>(null);

            ViewBag.Bio = Session.Serialize<Bio>();
            ViewBag.Old = Session.Serialize<Old>();
            ViewBag.Contact = Session.Serialize<Contact>();

            return View(bio);
        }

        [HttpPost]
        public ActionResult Bio(Bio model)
        {
            if (ModelState.IsValid)
            {
                var bio = sessionManager.Get(model);
                return RedirectToAction(nameof(Old));
            }

            return View(model);
        }

        #endregion

        #region Step-Old (2)

        public ActionResult Old()
        {
            var old = sessionManager.Get<Old>(null);

            ViewBag.Old = Session.Serialize<Old>();
            ViewBag.Bio = Session.Serialize<Bio>();
            ViewBag.Contact = Session.Serialize<Contact>();

            return View(old);
        }

        [HttpPost]
        public ActionResult Old(Old model, string prev, string next)
        {
            var old = sessionManager.Get(model);

            if (prev != null)
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction(nameof(Bio));
                }
            }

            if (next != null)
            {
                if (ModelState.IsValid)
                {
                    old.Age = model.Age;

                    return RedirectToAction(nameof(Contact));
                }
            }

            return View(model);
        }

        #endregion

        #region Step-Contact (3)

        public ActionResult Contact()
        {
            var bio = sessionManager.Get<Bio>(null);
            if (string.IsNullOrEmpty(bio.Name) || string.IsNullOrEmpty(bio.Family))
                return RedirectToAction(nameof(Bio));

            var contact = sessionManager.Get<Contact>(null);

            ViewBag.Bio = Session.Serialize<Bio>();
            ViewBag.Contact = Session.Serialize<Contact>();
            ViewBag.Old = Session.Serialize<Old>();

            return View(contact);
        }

        [HttpPost]
        public ActionResult Contact(Contact model, string prev)
        {
            var old = sessionManager.Get<Old>(null);
            var bio = sessionManager.Get<Bio>(null);
            var contact = sessionManager.Get(model);

            if (prev != null)
            {
                if (ModelState.IsValid)
                {
                    //Update(contact);

                    return RedirectToAction(nameof(Old));
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
                sessionManager.Remove<Old>();
                sessionManager.Remove<Contact>();

                return RedirectToAction(nameof(Success));
            }

            return View(model);
        }

        #endregion

        #endregion
    }
}