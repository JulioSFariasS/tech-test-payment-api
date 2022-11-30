using Microsoft.AspNetCore.Mvc;
using tech_test_payment_api.Context;
using tech_test_payment_api.Entities;
using tech_test_payment_api.Enums;

namespace tech_test_payment_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VendaController : ControllerBase
    {
        private readonly LojaContext _context;

        public VendaController(LojaContext context)
        {
            _context = context;
        }

        [HttpPost("RegistrarVenda")]
        public IActionResult RegistrarVenda(int vendedorId, string itens)
        {
            var vendedor = _context.Vendedores.Find(vendedorId);

            if(vendedor==null)
                return Content("O vendedor não foi encontrado no banco de dados.");

            if(itens==null || itens=="")
                return Content("A venda deve possuir pelo menos 1 item.");

            Venda venda = new Venda();
            venda.VendedorId = vendedorId;
            venda.Data = DateTime.Now;
            venda.Itens = itens;
            venda.Status = StatusDePedido.Aguardando_Pagamento;
            
            _context.Vendas.Add(venda);
            _context.SaveChanges();

            return Content(
                "Pedido registrado com sucesso!\n\n"+
                "DADOS DO PEDIDO \n" +
                $"Identificador: {venda.Id}\n" +
                $"Itens: {venda.Itens}\n" +
                $"Data: {venda.Data.ToString("HH:mm dd/MM/yyyy")}\n" +
                $"Status: {venda.Status.ToString()}\n\n" +
                "DADOS DO VENDEDOR \n" +
                $"Identificador: {vendedor.Id}\n" +
                $"Nome: {vendedor.Nome}\n" +
                $"CPF: {vendedor.Cpf}\n" +
                $"Telefone: {vendedor.Telefone}\n" +
                $"Email: {vendedor.Email}"
            );
        }

        [HttpGet("BuscarVenda/{id}")]
        public IActionResult BuscarVenda(int id)
        {
            var venda = _context.Vendas.Find(id);

            if(venda==null)
                return Content("A venda não foi encontrada no banco de dados.");

            var vendedor = _context.Vendedores.Find(venda.VendedorId);

            if(vendedor==null)
                return Content("O vendedor não foi encontrado no banco de dados.");
            

            return Content(
                "DADOS DO PEDIDO \n" +
                $"Identificador: {venda.Id}\n" +
                $"Itens: {venda.Itens}\n" +
                $"Data: {venda.Data.ToString("HH:mm dd/MM/yyyy")}\n" +
                $"Status: {venda.Status.ToString()}\n\n" +
                "DADOS DO VENDEDOR \n" +
                $"Identificador: {vendedor.Id}\n" +
                $"Nome: {vendedor.Nome}\n" +
                $"CPF: {vendedor.Cpf}\n" +
                $"Telefone: {vendedor.Telefone}\n" +
                $"Email: {vendedor.Email}"
            );
        }

        [HttpPut("AtualizarVenda/{id}")]
        public IActionResult AtualizarVenda(int id ,StatusDePedido statusDoPedido)
        {
            var venda = _context.Vendas.Find(id);

            if(venda==null)
                return Content("O pedido não foi encontrado no banco de dados.");

            bool atualizacaoAutorizada = false;

            if(venda.Status == StatusDePedido.Aguardando_Pagamento)
            {
                if(statusDoPedido == StatusDePedido.Pagamento_Aprovado || statusDoPedido == StatusDePedido.Cancelada)
                    atualizacaoAutorizada = true;
            }
            else
            if(venda.Status == StatusDePedido.Pagamento_Aprovado)
            {
                if(statusDoPedido == StatusDePedido.Enviado_Para_Transportadora || statusDoPedido == StatusDePedido.Cancelada)
                    atualizacaoAutorizada = true;
            }
            else
            if(venda.Status == StatusDePedido.Enviado_Para_Transportadora && statusDoPedido==StatusDePedido.Entregue)
            {
                atualizacaoAutorizada = true;
            }

            if(!atualizacaoAutorizada)
                return Content($"Não é possível atualizar o status do pedido de '{venda.Status.ToString()}' para '{statusDoPedido}'.");

            venda.Status = statusDoPedido;

            _context.Vendas.Update(venda);
            _context.SaveChanges();

            return Content($"Status do pedido {venda.Id} atualizado para '{venda.Status}'.");
        }
    }
}