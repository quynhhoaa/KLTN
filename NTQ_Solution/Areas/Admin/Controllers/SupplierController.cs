﻿using DataLayer.Dao;
using DataLayer.EF;
using DataLayer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NTQ_Solution.Areas.Admin.Controllers
{
    public class SupplierController : BaseController
    {
        SupplierDao supplierDao;
        ProductDao productDao;
        public SupplierController()
        {
            supplierDao = new SupplierDao();
            productDao = new ProductDao();
        }
        // GET: Admin/Supplier
        public ActionResult Index(string size, string color, int supplierID, string searchString, int page = 1, int pageSize = 5)
        {
            try
            {
                ViewBag.listColor = productDao.listcolor();
                ViewBag.listSize = productDao.listsize();
                ViewBag.SearchString = searchString;
                ViewBag.Size = size;
                ViewBag.Color = color;
                ViewBag.supplierID = supplierID;
                var model = supplierDao.GetProductOfSupplier(size, color, supplierID, searchString, page, pageSize);

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        [HttpGet]
        public ActionResult ImportProduct(int id)
        {
            try
            {

                var temp = productDao.GetProductById(id);
                bool status, checkTrending;
                if (temp.Status == 1) status = true;
                else status = false;
                if (temp.Trending == true) checkTrending = true;
                else checkTrending = false;
                var productModel = new ProductModel
                {
                    ProductName = temp.ProductName,
                    Status = status,
                    Trending = checkTrending,
                    Image = temp.Image,
                    ImportCount = 1,
                    SupplierID = temp.SupplierID,
                    ImportPrice = temp.ImportPrice,
                    Color = temp.Color,
                    Size = temp.Size,
                    Sale = temp.Sale,
                    Count = temp.Count
                };
                var listColor = productDao.listcolor();
                var listSize = productDao.listsize();
                foreach (var item in listColor)
                {
                    if (item.ID == temp.Color)
                    {
                        ViewBag.Color = item.ColorName;
                    }
                }
                foreach (var item in listSize)
                {
                    if (item.ID == temp.Size)
                    {
                        ViewBag.Size = item.SizeName;
                    }
                }
                return View(productModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        [HttpPost]
        public ActionResult ImportProduct(ProductModel productModel)
        {
            try
            {
                if(productModel.ImportPrice == null || productModel.ImportCount == 0)
                {
                    TempData["success"] = "Du lieu nhap hang chua dung";
                    return RedirectToAction("ImportProduct","Supplier");
                }
                var model = (NTQ_Solution.Common.UserLogin)Session[NTQ_Solution.Common.CommonConstant.USER_SESSION];
                int UserID = model.UserID;
                supplierDao.UpdateImport(productModel, UserID);
                TempData["success"] = "Nhap hang thanh cong";
                return RedirectToAction("Index", "ImportBill");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        [ChildActionOnly]
        public ActionResult ListSupplier()
        {
            var model = supplierDao.ListSupplier();
            return View(model);
        }
    }
}