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
    public class RoleController : BootstrapBaseController
    {
        private readonly string[] updateAttr = new string[] {  };
		private readonly IRoleRepository roleRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public RoleController() : this(new RoleRepository())
        {
        }

        public RoleController(IRoleRepository roleRepository)
        {
			this.roleRepository = roleRepository;
         }

		//
		// Search Method

        private List<SearchCondition> BuildCondition()
        {
            List<SearchCondition> _sc = new List<SearchCondition>();
            return _sc;
        }

        //
        // GET: /Role/

        public ViewResult Index(int? page)
        {
            var pageIndex = (page ?? 1) - 1; 
            var pageSize = 5;
            int totalCount; 

            Specification<Role> c = SpecificationBuilder.BuildSpecification<Role>(BuildCondition());

            var roles = roleRepository.AllMatching(c, pageIndex, pageSize, "CreatedOn", false, out totalCount);

            var rolesAsIPagedList = new StaticPagedList<Role>(roles, pageIndex + 1, pageSize, totalCount);
            ViewBag.OnePageOfroles = rolesAsIPagedList;

			return View();
        }

        //
        // GET: /Role/Details/5

        public ViewResult Details(System.Guid id)
        {
            return View(roleRepository.Find(id));
        }

        //
        // GET: /Role/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Role/Create

        [HttpPost]
        public ActionResult Create(Role role)
        {
            if (ModelState.IsValid) 
			{
                roleRepository.InsertOrUpdate(role);
                roleRepository.Save();
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
        // GET: /Role/Edit/5
 
        public ActionResult Edit(System.Guid id)
        {
             return View(roleRepository.Find(id));
        }

        //
        // POST: /Role/Edit/5

        [HttpPost]
        public ActionResult Edit(Role role)
        {
            if (ModelState.IsValid) 
			{
                roleRepository.InsertOrUpdate(role, updateAttr);
                roleRepository.Save();
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
        // GET: /Role/Delete/5
 
        public ActionResult Delete(System.Guid id)
        {
            return View(roleRepository.Find(id));
        }

        //
        // POST: /Role/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(System.Guid id)
        {
            roleRepository.Delete(id);
            roleRepository.Save();
            Success("\u5220\u9664\u6210\u529f\uff01");
            return RedirectToAction("Index");
        }
    }
}

