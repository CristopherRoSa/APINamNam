namespace NamNamAPI.Controllers
{
    using Domain;
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using NamNamAPI.Business;

    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private CategoryProvider categoryProvider;
        public CategoryController(CategoryProvider _categoryProvider)
        {
            categoryProvider = _categoryProvider;
        }

        [HttpGet("GetCategories")]
        public ActionResult GetUsertest()
        {
            (int code, List<CategoryDomain> categoriesList, string report) = categoryProvider.getCategories();
            if (code == 200)
            {
                return Ok(
                    categoriesList
                );
            }
            else
            {
                return NotFound();
            }


        }
    }
}