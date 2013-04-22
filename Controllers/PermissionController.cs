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
    public class PermissionController : BootstrapBaseController
    {
        private readonly string[] updateAttr = new string[] { "PermissionType" };
		private readonly IPermissionRepository permissionRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public PermissionController() : this(new PermissionRepository())
        {
        }

        public PermissionController(IPermissionRepository permissionRepository)
        {
			this.permissionRepository = permissionRepository;
         }

		//
		// Search Method

        private List<SearchCondition> BuildCondition()
        {
            List<SearchCondition> _sc = new List<SearchCondition>();
            return _sc;
        }

        //
        // GET: /Permission/

        public ViewResult Index(int? page)
        {
            var pageIndex = (page ?? 1) - 1; 
            var pageSize = 5;
            int totalCount; 

            Specification<Permission> c = SpecificationBuilder.BuildSpecification<Permission>(BuildCondition());

            var permissions = permissionRepository.AllMatching(c, pageIndex, pageSize, "CreatedOn", false, out totalCount);

            var permissionsAsIPagedList = new StaticPagedList<Permission>(permissions, pageIndex + 1, pageSize, totalCount);
            ViewBag.OnePageOfpermissions = permissionsAsIPagedList;

			return View();
        }

        //
        // GET: /Permission/Details/5

        public ViewResult Details(System.Guid id)
        {
            return View(permissionRepository.Find(id));
        }

        //
        // GET: /Permission/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Permission/Create

        [HttpPost]
        public ActionResult Create(Permission permission)
        {
            if (ModelState.IsValid) 
			{
                permissionRepository.InsertOrUpdate(permission);
                permissionRepository.Save();
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
        // GET: /Permission/Edit/5
 
        public ActionResult Edit(System.Guid id)
        {
             return View(permissionRepository.Find(id));
        }

        //
        // POST: /Permission/Edit/5

        [HttpPost]
        public ActionResult Edit(Permission permission)
        {
            if (ModelState.IsValid) 
			{
                permissionRepository.InsertOrUpdate(permission, updateAttr);
                permissionRepository.Save();
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
        // GET: /Permission/Delete/5
 
        public ActionResult Delete(System.Guid id)
        {
            return View(permissionRepository.Find(id));
        }

        //
        // POST: /Permission/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(System.Guid id)
        {
            permissionRepository.Delete(id);
            permissionRepository.Save();
            Success("\u5220\u9664\u6210\u529f\uff01");
            return RedirectToAction("Index");
        }
    }
}

