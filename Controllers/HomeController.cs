using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace BootstrapMvcSample.Controllers
{
    public class HomeController : BootstrapBaseController
    {
        private static List<HomeInputModel> _models = ModelIntializer.CreateHomeInputModels();
        [Authorize]
        public ActionResult Index()
        {

            var homeInputModels = _models;
            return View(homeInputModels);
        }

        [HttpPost]
        public ActionResult Create(HomeInputModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = _models.Count == 0 ? 1 : _models.Select(x => x.Id).Max() + 1;
                _models.Add(model);
                Success("保存成功!");
                return RedirectToAction("Index");
            }
            Error("表单中存在一些错误.");
            return View(model);
        }

        public ActionResult Create()
        {
            return View(new HomeInputModel());
        }

        public ActionResult Delete(int id)
        {
            _models.Remove(_models.Get(id));
            Information("删除成功");
            if (_models.Count == 0)
            {
                Attention("您已经删除所有信息，请添加之后在操作.");
            }
            return RedirectToAction("index");
        }
        public ActionResult Edit(int id)
        {
            var model = _models.Get(id);
            return View("Create", model);
        }
        [HttpPost]
        public ActionResult Edit(HomeInputModel model, int id)
        {
            if (ModelState.IsValid)
            {
                _models.Remove(_models.Get(id));
                model.Id = id;
                _models.Add(model);
                Success("信息更新成功!");
                return RedirectToAction("index");
            }
            return View("Create", model);
        }

        public ActionResult Details(int id)
        {
            var model = _models.Get(id);
            return View(model);
        }

    }
}
