namespace Pedidos_Lanches;


class Program
{
    static List<Ordem> Ordens = new();
    public static List<Produto> Produtos = new();
    enum OptionsMenu { Adicionar = 1, Listar, Sair }
    static void Main()
    {
        bool decidiuSair = false;
        while (!decidiuSair)
        {
            Console.Clear();
            System.Console.WriteLine("Bem vindo ao gerenciador de pedidos!Selecione a opção que deseja para prosseguirmos:");
            System.Console.WriteLine("1 - Adicionar novo pedido\n2 - Listar pedidos\n3 - Sair do programa");
            if (int.TryParse(Console.ReadLine(), out int opcaoSelecionada))
            {
                switch ((OptionsMenu)opcaoSelecionada)
                {
                    case OptionsMenu.Adicionar:
                        AdicionarPedido();
                        break;
                    case OptionsMenu.Listar:
                        ListarPedidos();
                        break;
                    case OptionsMenu.Sair:
                        Environment.Exit(0);
                        break;
                    default:
                        System.Console.WriteLine("Opção inválida, tente novamente!");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
    static void AdicionarPedido()
    {
        AdicionarProduto();
        try
        {
            bool finalizarPedido = false;
            List<Produto> Itens = new();
            while (!finalizarPedido)
            {
                Console.Clear();
                System.Console.WriteLine($"Quantidade de itens no pedido: {CalcularTotalItens(Itens).ToString()}");
                System.Console.Write("Código do produto (Digite 0 para finalizar): ");
                if (int.TryParse(Console.ReadLine(), out int codSelecionado))
                {
                    if (codSelecionado == 0) break;
                    Produto produto = Produto.Get(codSelecionado);
                    if (produto is null)
                    {
                        System.Console.WriteLine("Produto não existente!");
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        System.Console.WriteLine($"Id: {produto.Id}");
                        System.Console.WriteLine($"Nome: {produto.Nome}");
                        System.Console.WriteLine($"Valor: {produto.Valor.ToString("C")}");
                        System.Console.Write("Quantidade: ");
                        if (int.TryParse(Console.ReadLine(), out int quantidadeProduto))
                        {
                            produto.Quantidade = quantidadeProduto;
                            Produto newProduto = new(produto.Nome, produto.Valor, produto.Quantidade);
                            Itens.Add(newProduto);
                        }
                        else
                        {
                            System.Console.WriteLine("Tipo de dado incorreto! Tente novamente");
                            Console.ReadKey();
                            continue;
                        }
                    }
                }
                else
                {
                    System.Console.WriteLine("Tipo de dados incorreto, tente novamente.");
                    Console.ReadKey();
                    continue;
                }
                System.Console.Write("Deseja adicionar outro produto? (s/n):");
                string desejaContinuar = Console.ReadLine();
                if (desejaContinuar.ToLower() == "s") continue;
                else if (desejaContinuar.ToLower() == "n") finalizarPedido = true;
            }
            if (Itens.Count > 0)
            {
                System.Console.WriteLine("Qual o nome do cliente?");
                string cliente = Console.ReadLine();
                Ordem Ordem = new(cliente, Itens);
                Ordens.Add(Ordem);  //Adicionar pedido na lista de pedidos
                Console.Clear();
                Ordem.ExibirNotaPedido();
                Console.ReadKey();
            }
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"Erro conforme a mensagem '{e.Message}'");
            Console.ReadLine();
        }
    }
    static void ListarPedidos()
    {
        Console.Clear();
        foreach (var ordem in Ordens)
        {
            ordem.ListarPedidos();
            System.Console.WriteLine("====================================");
        }
        Console.ReadLine();
    }

    static int CalcularTotalItens(List<Produto> itens)
    {
        int total = 0;
        foreach (Produto item in itens)
        {
            total += item.Quantidade;
        }
        return total;
    }

    static void AdicionarProduto()
    {
        Produto bola = new("Bola", 29.90m);
        Produto.Add(bola);
        Produto biscoito = new("Biscoito", 3.99m);
        Produto.Add(biscoito);
        Produto refrigerante = new("Refrigerante", 6.99m);
        Produto.Add(refrigerante);
    }
}