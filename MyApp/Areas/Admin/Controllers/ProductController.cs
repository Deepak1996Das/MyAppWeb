using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using MyApp.Models.ViewModels;
using NuGet.Protocol.Plugins;

namespace MyApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostingEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }

        #region API CALL

        public IActionResult AllProducts()
        {
            var products = _unitOfWork.product.GetAll(inludeProperties:"Category");
            return Json(new {data=products});
         
        }
        #endregion


        public IActionResult Index()
        {
          
            return View();
        }


       


        //CreateUpdate Product-------------------------------------------------------


        public IActionResult CreateUpdate(int? id)
        {
            ProductVM productvm=new ProductVM()
            { 
                Product=new Product(),
                Categories=_unitOfWork.category.GetAll().Select(x=>
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Text = x.Name,
                    Value=x.id.ToString(),
                }),
            };
            if (id == null || id == 0)
            {
                return View(productvm);
            }
            else 
            {

               productvm.Product= _unitOfWork.product.GetById(u => u.Id == id);
                if (productvm.Product == null)
                {
                    return NotFound(); 
                }
                return View(productvm);
            }
           

            return View(productvm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(ProductVM productVM,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                // This need for file upload
                if(file!=null)
                {
                    string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "ProductImage");
                    string fileName = Guid.NewGuid().ToString()+"_"+file.FileName;
                    string filePath=Path.Combine(uploadDir,fileName);

                    if(productVM.Product.ImageUrl!=null)
                    {
                        var oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                      
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        
                    }

                    using(var fileStream=new FileStream(filePath,FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"/ProductImage/" + fileName;
                }
                if(productVM.Product.Id == 0)
                {
                    _unitOfWork.product.Add(productVM.Product);
                    TempData["success"] = "Product created Done!";
                }
                else
                {
                    _unitOfWork.product.Update(productVM.Product);
                    TempData["success"] = "Product update Done!";
                }
               
                
                _unitOfWork.Save();
                
                return RedirectToAction("Index");
            }

            return View(productVM);
        }

        //Delete----------------------------------------------------------


        #region DeleteAPICAll    
        [HttpDelete]
      
       
        public IActionResult Delete(int? id)
        {
            if (ModelState.IsValid)
            {
                Product product = _unitOfWork.product.GetById(u => u.Id == id);

                if(product==null)
                {
                    return Json(new { success = false, Error = "Error in fetching Data" });
                }
                else
                {
                    

                    var oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }


                    _unitOfWork.product.Delete(product);
                    _unitOfWork.Save();

                    return Json(new { success = true, Message = "Product Delete success!" });
                }

                
            }

            return View();
        }

        #endregion
    }
}
