using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync([FromServices] IMemoryCache cache,[FromServices] BlogDataContext context)
        {
            try
            {
                var categories = cache.GetOrCreate("CategoriesCache", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return GetCategories(context);
                });
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("05X10 - Falha interna no servidor"));
            }
        }
        private List<Category> GetCategories(BlogDataContext context)
        {
            return context.Categories.AsNoTracking().ToList();
        }
        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] BlogDataContext context, [FromRoute] int id)
        {
            try
            {
                var category = await context
                    .Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                return Ok(new ResultViewModel<Category>(category));
            }

            catch 
            {
                return StatusCode(500, new ResultViewModel<Category>("05X10 - Falha interna no servidor"));
            }
        }
        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync([FromServices] BlogDataContext context, [FromBody] EditorCategoryViewModel model) 
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<List<Category>>(ModelState.GetErrors()));

            try
            {
                var category = new Category(model.Name, model.Slug.ToLower());

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("05XE9 - Não foi possível incluir a Categoria"));
            }
            catch  
            {
                return StatusCode(500, new ResultViewModel<Category>("05X10 - Falha interna no servidor"));
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync([FromServices] BlogDataContext context, [FromRoute] int id, [FromBody] EditorCategoryViewModel model) 
        {
            try
            {
                var categoryToUpdate = await context
                    .Categories
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (categoryToUpdate == null) 
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                categoryToUpdate.Name = model.Name;
                categoryToUpdate.Slug = model.Slug;

                context.Categories.Update(categoryToUpdate);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(categoryToUpdate));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("05XE8 - Não foi possível alterar a Categoria"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Category>("05X11 - Falha interna no servidor"));
            }
        }
        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] BlogDataContext context) 
        {
            try
            {
                var categoryToDelete = await context
                    .Categories
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (categoryToDelete == null) 
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                context.Categories.Remove(categoryToDelete);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(categoryToDelete));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("05XE7 - Não foi possível excluir a Categoria"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Category>("05X1 - Falha interna no servidor"));
            }
        }
    }
}
