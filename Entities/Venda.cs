using tech_test_payment_api.Enums;

namespace tech_test_payment_api.Entities
{
    public class Venda
    {
        public int Id { get; set; }
        public int VendedorId { get; set; }
        public DateTime Data { get; set; }
        public string Itens { get; set; }
        public StatusDePedido Status { get; set; }
    }
}