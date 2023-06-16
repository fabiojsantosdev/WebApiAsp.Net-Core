using ApiCatalago.Context;
using ApiCatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ApiCatalago.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        //Metodo Get buscar todos produtos.
        [HttpGet]

        public ActionResult<IEnumerable<Produto>> GetAllProdutct() {
            try
            {
                var produtos = _context.Produtos?.AsNoTracking().ToList();
                if (produtos is null)
                {
                    return NotFound("Produtos não encontrados!");
                }
                return Ok(produtos);

            }
            catch (Exception)
            {

                throw;
            }
        }




        //Metodo Get buscar produto pelo ID.
        [HttpGet("{id:int}", Name ="ObterProduto")]

        public ActionResult<Produto> GetProdutId(int id)
        {
            try
            {
                var produto = _context.Produtos?.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);

                if (produto is null)
                {
                    return NotFound("Produto não encontrado!");
                }
                return produto;
            }
            catch (Exception)
            {

                throw;
            }
        }



        //Metodo inserir novos produtos
        [HttpPost]
        public ActionResult CreateNewProdut(Produto produto)
        {
            try
            {
                if (produto is null)
                    return BadRequest();

                _context.Produtos?.Add(produto);
                _context.SaveChanges();

                return new CreatedAtRouteResult("ObterProduto",
                    new { id = produto.ProdutoId }, produto);
            }
            catch (Exception)
            {

                throw;
            }
        }



        //Metodo atualizar dados produto
        [HttpPut("{id:int}")]
        public ActionResult UpdateProduct(int id, Produto produto) 
        {
            try
            {

                var prod = _context.Produtos?.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id); 
                
                if (id != produto.ProdutoId || produto == null)
                {
                    return BadRequest("Id: "+ id + " diferente do ProdutoId: " + produto?.ProdutoId+"!");
                }
                else
                if (prod is null)
                {
                    return NotFound("Produto id: " + id + " não econtrado!");
                }


                _context.Entry(prod).CurrentValues.SetValues(produto);
                _context.SaveChanges();

                return Ok("Sucesso ao atualizar produto Id: "+id);
            }
            catch (Exception )
            {

                throw ;
            }
        }



        //Metodo atualizar dados produto
        [HttpDelete("{id:int}")]
        public ActionResult DeleteProduct(int id)
        {
            try
            {

                var prod = _context.Produtos?.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id); 

                if (prod is null)
                {
                    return NotFound("Produto id: " + id + " não econtrado!");
                }


                _context.Produtos?.Remove(prod);
                _context.SaveChanges();

                return Ok("Produto Id: " + id + " removido com sucesso!");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
