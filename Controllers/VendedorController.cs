using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tech_test_payment_api.Context;
using tech_test_payment_api.Entities;

namespace tech_test_payment_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VendedorController : ControllerBase
    {
        private readonly LojaContext _context;

        public VendedorController(LojaContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Criar(Vendedor vendedor)
        {
            _context.Vendedores.Add(vendedor);
            _context.SaveChanges();

            return Content("Vendedor criado com sucesso!\n" +
            $"Identificador: {vendedor.Id}\n"+
            $"Nome: {vendedor.Nome}\n" +
            $"CPF: {vendedor.Cpf}\n" +
            $"Email: {vendedor.Email}\n" +
            $"Telefone: {vendedor.Telefone}\n"
            );
        }

        [HttpGet("ObterPorId/{id}")]
        public IActionResult ObterPorId(int id)
        {
            var vendedor = _context.Vendedores.Find(id);

            if(vendedor==null)
                return Content("O vendedor não foi encontrado no banco de dados.");

            return Ok(vendedor);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ListarTodos()
        {
            var vendedores = _context.Vendedores.ToList();

            return Ok(vendedores);
        }

        [HttpPut("Modificar/{id}")]
        public IActionResult Modificar(int id, Vendedor vendedor)
        {
            var vendedorBanco = _context.Vendedores.Find(id);

            if(vendedorBanco==null)
                return Content("O vendedor não foi encontrado no banco de dados.");

            vendedorBanco.Nome = vendedor.Nome;
            vendedorBanco.Cpf = vendedor.Cpf;
            vendedorBanco.Telefone = vendedor.Telefone;
            vendedorBanco.Email = vendedor.Email;

            _context.Vendedores.Update(vendedorBanco);
            _context.SaveChanges();

            return Content($"O vendedor {id} foi atualizado com sucesso!");
        }
    }
}