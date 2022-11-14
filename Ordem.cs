namespace Pedidos_Lanches;

class Ordem
{
    public Ordem()
    {
        Data = DateTime.Now;
        Itens = new();
        Nr_pedido = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
    }
    public Ordem(string cliente, List<Produto> itens) : this()
    {
        this.Cliente = cliente;
        Itens = itens;
    }
    public string Nr_pedido { get; set; }
    public DateTime Data { get; set; }
    public List<Produto> Itens { get; set; }

    public string Cliente { get; set; } = string.Empty;

    public decimal GetTotal()
    {
        decimal total = 0m;
        if (Itens.Count > 0)
        {
            foreach (Produto item in Itens)
            {
                total += item.Valor * item.Quantidade;
            }
        }
        else
        {
            System.Console.WriteLine("Não há itens adicionados ao pedido!");
            Console.ReadKey();
            total = 0m;
        }
        return total;
    }
    public void ExibirNotaPedido()
    {
        decimal somaSubTotal = 0;
        int somaQuantidade = 0;
        decimal subtotal = 0;
        System.Console.WriteLine($"Horário do pedido: {Data.ToString().PadRight(15)} -  Código: {Nr_pedido}");
        System.Console.WriteLine($"Cliente: {this.Cliente}");
        foreach (Produto item in Itens)
        {
            subtotal = item.Valor * item.Quantidade;
            System.Console.WriteLine($"Produto: {item.Nome.PadRight(30)}|Valor: {item.Valor.ToString("C").PadRight(10)}|Quantidade: {item.Quantidade.ToString().PadRight(10)}|SubTotal: {subtotal.ToString("C").PadLeft(15)}|");
            somaSubTotal += item.Valor;
            somaQuantidade += item.Quantidade;
        }
        System.Console.WriteLine("-----------------------------------------------------------------------------------------");
        System.Console.WriteLine("|Total:".PadLeft(46) + $" {somaSubTotal.ToString("C").PadRight(10)}|Total: {somaQuantidade.ToString().PadRight(15)}|Valor total: {this.GetTotal().ToString("C").PadLeft(12)}|");
        // System.Console.WriteLine("TOTAL -->".PadRight(30) + this.GetTotal().ToString("C"));
    }
    public void ListarPedidos()
    {
        System.Console.WriteLine($"Ordem: {this.Nr_pedido}");
        System.Console.WriteLine($"Cliente: {this.Cliente}");
        System.Console.WriteLine($"Data do pedido: {this.Data.ToString()}");
        System.Console.WriteLine("Produtos:");
        foreach (Produto item in Itens)
        {
            System.Console.WriteLine($"Item: {item.Nome.PadRight(40)} | Quantidade - {item.Quantidade.ToString().PadRight(3)}| Valor - {item.Valor.ToString("C").PadLeft(10)}|");
        }
        System.Console.WriteLine($"Total -------- {this.GetTotal().ToString("C")}");
    }
}