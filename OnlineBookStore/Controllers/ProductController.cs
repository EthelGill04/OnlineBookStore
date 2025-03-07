using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Models;
using OnlineBookStore.Services;

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
                Image = newFileName ,
                Language = addProductDto.Language,
                Price = addProductDto.Price,
            };

            context.Products.Add(product);
            context.SaveChanges();

            return RedirectToAction("Index", "Product");
        }
    }
}
