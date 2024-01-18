using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperSeller.Models;
using SuperSeller.Services;
using System.Data;

namespace SuperSeller.Pages.Admin.Products
{
    public class CreateModel : PageModel
    {
		private readonly IWebHostEnvironment environment;
		private readonly ApplicationDbContext context;

		[BindProperty]
        public ProductDto ProductDto { get; set; }=new ProductDto();

        public CreateModel(IWebHostEnvironment environment, ApplicationDbContext context)
        {
			this.environment = environment;
			this.context = context;
		}
        public void OnGet()
        {
        }

        public string errorMessage = "";
        public string successMessage = "";
        public void OnPost() { 

            if(ProductDto.ImageFile == null)
            {
                ModelState.AddModelError("ProductDto.ImageFiles", "The image file is required");
            }

            if(!ModelState.IsValid)
            {
                errorMessage = "please provide all the  required field";
                return;
            }

            // save the image files 

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(ProductDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
            using(var stream = System.IO.File.Create(imageFullPath))
            {
                ProductDto.ImageFile.CopyTo(stream);
            }


            // save the new product in the database 

            Product product = new Product()
            {
                Name = ProductDto.Name,
                Brand = ProductDto.Brand,
                Category = ProductDto.Category,
                Price = ProductDto.Price,
                Description = ProductDto.Description??"",
                ImageFileName = newFileName,
                CreateAt = DateTime.Now,
                
            };

            context.Products.Add(product);
            context.SaveChanges();
        


            // clear the form

            ProductDto.Name = "";
			ProductDto.Brand = "";
			ProductDto.Category = "";
			ProductDto.Price = 0;
			ProductDto.Description = "";
			ProductDto.ImageFile = null;

            ModelState.Clear();

            successMessage = "Product created successfully";

            Response.Redirect("/Admin/Products/Index");
		}
    }
}
