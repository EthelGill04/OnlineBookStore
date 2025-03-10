using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Models;
using OnlineBookStore.Services;
using System.Linq.Expressions;

namespace OnlineBookStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;

        public ProductController(AppDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index()
        {
            var products = context.Products.OrderByDescending(p => p.Id).ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AddProductDto addProductDto)
        {
            if (addProductDto.ImageFile == null) {
                ModelState.AddModelError("ImageFile", "Image file is required");
            }

            if (!ModelState.IsValid) {
                return View(addProductDto);
            }

            //saving the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(addProductDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/img/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                addProductDto.ImageFile.CopyTo(stream);
            }

            Product product = new Product()
            {
                Title = addProductDto.Title,
                Author = addProductDto.Author,
                DatePublished = DateTime.Now,
                Description = addProductDto.Description,
                Genre = addProductDto.Genre,
                Image = newFileName,
                Language = addProductDto.Language,
                Price = addProductDto.Price,
            };

            context.Products.Add(product);
            context.SaveChanges();

            return RedirectToAction("Index", "Product");
        }

        public IActionResult Edit(int id) { 
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Product");
            }

            var addProductDto = new AddProductDto()
            {
                Title = product.Title,
                Author = product.Author,
                Description = product.Description,
                Genre = product.Genre,
                Price = product.Price,
                Language = product.Language,
            };

            ViewData["ProductID"] = product.Id;
            ViewData["ImageFileName"] = product.Image;
            ViewData["CreatedAt"] = product.DatePublished.ToString("MM/dd/yyyy");

            return View(addProductDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, AddProductDto addProductDto)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Product");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProductID"] = product.Id;
                ViewData["ImageFileName"] = product.Image;
                ViewData["CreatedAt"] = product.DatePublished.ToString("MM/dd/yyyy");

                return View(addProductDto);
            }
            //update the image file if there is a new image file

            string newFileName = product.Image;
            if(addProductDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(addProductDto.ImageFile!.FileName);

                string imageFullPath = environment.WebRootPath + "/img/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    addProductDto.ImageFile.CopyTo(stream);
                }

                //delete the old image
                string oldImageFullPath = environment.WebRootPath + "/img/"+product.Image;
                System.IO.File.Delete(oldImageFullPath);
            }

            //upate the product in the database

            product.Image = newFileName;
            product.Title = addProductDto.Title;
            product.Price = addProductDto.Price;
            product.Genre = addProductDto.Genre;
            product.Author = addProductDto.Author;
            product.Description = addProductDto.Description;
            product.Language = addProductDto.Language;

            context.SaveChanges();
            return RedirectToAction("Index","Product");
        }
        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Product");
            }
            string imageFullPath = environment.WebRootPath +"/img/"+ product.Image;
            System.IO.File.Delete(imageFullPath);
            context.Products.Remove(product);
            context.SaveChanges(true);
            return RedirectToAction("Index", "Product");
        }
    }
}
