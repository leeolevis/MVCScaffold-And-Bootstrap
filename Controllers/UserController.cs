using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp4.Specification;
using WebApp4.Condition;
using WebApp4.Entities;
using WebApp4.Models;
using PagedList;
using BootstrapMvcSample.Controllers;

namespace WebApp4.Controllers
{   
    public class UserController : BootstrapBaseController
    {
        private readonly string[] updateAttr = new string[] {  };
		private readonly IUserRepository userRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public UserController() : this(new UserRepository())
        {
        }

        public UserController(IUserRepository userRepository)
        {
			this.userRepository = userRepository;
         }

		//
		// Search Method

        private List<SearchCondition> BuildCondition()
        {
            List<SearchCondition> _sc = new List<SearchCondition>();
            return _sc;
        }

        public ActionResult Setting()
        {
            return View();
        }

        public ActionResult Password()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        //
        // GET: /User/

        public ViewResult Index(int? page)
        {
            var pageIndex = (page ?? 1) - 1; 
            var pageSize = 5;
            int totalCount; 

            Specification<User> c = SpecificationBuilder.BuildSpecification<User>(BuildCondition());

            var users = userRepository.AllMatching(c, pageIndex, pageSize, "CreatedOn", false, out totalCount);

            var usersAsIPagedList = new StaticPagedList<User>(users, pageIndex + 1, pageSize, totalCount);
            ViewBag.OnePageOfusers = usersAsIPagedList;

			return View();
        }

        //
        // GET: /User/Details/5

        public ViewResult Details(System.Guid id)
        {
            return View(userRepository.Find(id));
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid) 
			{
                userRepository.InsertOrUpdate(user);
                userRepository.Save();
                Success("\u4fdd\u5b58\u6210\u529f\uff01");
                return RedirectToAction("Index");
            } 
			else
			{
                Error("\u4fdd\u5b58\u5931\u8d25\uff0c\u8868\u5355\u4e2d\u5b58\u5728\u4e00\u4e9b\u9519\u8bef\uff01");
				return View();
			}
        }
        
        //
        // GET: /User/Edit/5
 
        public ActionResult Edit(System.Guid id)
        {
             return View(userRepository.Find(id));
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid) 
			{
                userRepository.InsertOrUpdate(user, updateAttr);
                userRepository.Save();
                Success("\u4fee\u6539\u6210\u529f\uff01");
                return RedirectToAction("Index");
            } 
			else 
			{
                Error("\u4fee\u6539\u5931\u8d25\uff0c\u8868\u5355\u4e2d\u5b58\u5728\u4e00\u4e9b\u9519\u8bef\uff01");
				return View();
			}
        }

        //
        // GET: /User/Delete/5
 
        public ActionResult Delete(System.Guid id)
        {
            return View(userRepository.Find(id));
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(System.Guid id)
        {
            userRepository.Delete(id);
            userRepository.Save();
            Success("\u5220\u9664\u6210\u529f\uff01");
            return RedirectToAction("Index");
        }
    }
}

