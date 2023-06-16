using ApiCatalago.Context;
using ApiCatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        //Metodo Get buscar todas categorias.
        [HttpGet]

        public ActionResult<IEnumerable<Categoria>> GetAllCategory()
        {
            var categorias = _context.Categorias?.AsNoTracking().ToList();

            if (categorias is null)
            {
                return NotFound();
            }

            return Ok(categorias);

        }


        //Metodo Get buscar todas categorias.
        [HttpGet("produtos")]

        public ActionResult<IEnumerable<Categoria>> GetAllCategoryProduct()
        {
            var categorias = _context.Categorias?.Include(p => p.Produtos).ToList();
            if (categorias is null)
            {
                return NotFound("Nenhum categoria localizada!");
            }
            return Ok(categorias);
        }




        //Metodo Get buscar categoria pelo ID.
        [HttpGet("{id:int}", Name = "ObterCategoria")]

        public ActionResult<Categoria> GetCategorytId(int id)
        {
            var categoria = _context.Categorias?.AsNoTracking().FirstOrDefault(p => p.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound("Nenhum categoria localizada!");
            }
            return categoria;
        }



        //Metodo inserir nova categloria
        [HttpPost]
        public ActionResult CreateNewCategory(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            _context.Categorias?.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoria);
        }



        //Metodo atualizar categoria
        [HttpPut("{id:int}")]
        public ActionResult UpdateCategory(int id, Categoria categoria)
        {
            try
            {

                var category = _context.Categorias?.AsNoTracking().FirstOrDefault(p => p.CategoriaId == id);

                if (id != categoria.CategoriaId)
                {
                    return BadRequest("Id: " + id + " diferente da CategoriaId: " + categoria.CategoriaId + "!");
                }
                else
                if (category is null)
                {
                    return NotFound("Categoria id: " + id + " não econtrado!");
                }


                _context.Entry(category).CurrentValues.SetValues(categoria);
                _context.SaveChanges();

                return Ok("Sucesso ao atualizar categoria Id: " + id);
            }
            catch (Exception)
            {

                throw;
            }
        }



        //Metodo deletar categoria
        [HttpDelete("{id:int}")]
        public ActionResult DeleteCategory(int id)
        {
            try
            {

                var category = _context.Categorias?.AsNoTracking().FirstOrDefault(p => p.CategoriaId == id);

                if (category is null)
                {
                    return NotFound("Categoria id: " + id + " não econtrado!");
                }


                _context.Categorias?.Remove(category);
                _context.SaveChanges();

                return Ok("Categoria Id: " + id + " removido com sucesso!");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
