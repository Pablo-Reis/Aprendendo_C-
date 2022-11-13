using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
namespace Pedidos_Lanches;

class Program
{
    static List<Ordem> Ordens = new();
    public static List<Produto> Produtos = new();
    enum OptionsMenu { Adicionar_Pedido = 1, Listar_Pedidos, Cadastrar_Produtos, Listar_Produtos, Remover_Produto, Sair }
    static void Main()
    {
        Carregar();
        bool decidiuSair = false;
        while (!decidiuSair)
        {
            Console.Clear();
            System.Console.WriteLine("Bem vindo ao gerenciador de pedidos!Selecione a opção que deseja para prosseguirmos:");
            System.Console.WriteLine("1 - Adicionar novo pedido\n2 - Listar pedidos\n3 - Cadastrar produto\n4 - Listar Produtos\n5 - Remover produto\n6 - Sair do programa");
            if (int.TryParse(Console.ReadLine(), out int opcaoSelecionada))
            {
                switch ((OptionsMenu)opcaoSelecionada)
                {
                    case OptionsMenu.Adicionar_Pedido:
                        AdicionarPedido();
                        break;
                    case OptionsMenu.Listar_Pedidos:
                        ListarPedidos();
                        break;
                    case OptionsMenu.Cadastrar_Produtos:
                        AdicionarProduto();
                        break;
                    case OptionsMenu.Listar_Produtos:
                        ListarProdutos();
                        break;
                    case OptionsMenu.Remover_Produto:
                        RemoverProduto();
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
                            Itens.Add(new Produto(produto.Nome, produto.Valor, quantidadeProduto));
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
                Ordens.Add(Ordem);
                Salvar();
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
        Console.Clear();
        System.Console.Write("Nome do produto: ");
        string nomeProduto = Console.ReadLine();
        System.Console.Write("Valor do produto: R$");
        if (decimal.TryParse(Console.ReadLine(), out decimal valorProduto))
        {
            Produto.Add(new Produto(nomeProduto, valorProduto));
            Salvar();
            System.Console.WriteLine("Produto adicionado com sucesso!");
            Console.ReadKey();
        }
    }
    static void ListarProdutos()
    {
        Console.Clear();
        if (Produto.GetAll().Count > 0)
        {
            foreach (Produto produto in Produto.GetAll())
            {
                System.Console.WriteLine($"Produto: {produto.Id}");
                System.Console.WriteLine($"Nome: {produto.Nome}");
                System.Console.WriteLine($"Valor: {produto.Valor.ToString("C")}");
                System.Console.WriteLine("==================================================");
            }
        }
        else
        {
            System.Console.WriteLine("Não existem produtos!");
        }
        Console.ReadKey();
    }
    static void RemoverProduto()
    {
        Console.Clear();
        System.Console.WriteLine("Digite o ID do produto que deseja remover:");
        if (int.TryParse(Console.ReadLine(), out int IdSelecionado))
        {
            if (Produto.Remove(IdSelecionado))
            {
                System.Console.WriteLine("Produto removido com sucesso!");
                Salvar();
            }
            else
            {
                System.Console.WriteLine("Produto não encontrado");
            }
            Console.ReadKey();
        }
    }

    static void Salvar()
    {
        try
        {
            string PathProduct = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}{Path.DirectorySeparatorChar}product.txt";
            string PathOrder = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}{Path.DirectorySeparatorChar}order.txt";
            string jsonListProdutos = JsonSerializer.Serialize(Produtos);
            File.WriteAllText(PathProduct, jsonListProdutos);
            string jsonListOrdens = JsonSerializer.Serialize(Ordens);
            File.WriteAllText(PathOrder, jsonListOrdens);
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"Erro conforme mensagem: {e.Message}");
            Console.ReadKey();
        }

    }
    static void Carregar()
    {
        string PathProduct = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}{Path.DirectorySeparatorChar}product.txt";
        string PathOrder = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}{Path.DirectorySeparatorChar}order.txt";
        try
        {
            var data = File.ReadAllText(PathProduct);
            var dataList = JsonSerializer.Deserialize<List<Produto>>(data);
            foreach (Produto produto in dataList)
            {
                Produto.Add(produto);
            }
            if (Produtos is null)
            {
                Produtos = new();
            }
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"Erro ao carregar produtos: {e.Message}");
            Console.ReadKey();
            Produtos = new();
        }
        try
        {
            var data = File.ReadAllText(PathOrder);
            var dataList = JsonSerializer.Deserialize<List<Ordem>>(data);
            foreach (Ordem ordem in dataList)
            {
                Ordens.Add(ordem);
            }
            if (Ordens is null)
            {
                Ordens = new();
            }
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"Erro ao carregar pedidos: {e.Message}");
            Console.ReadKey();
            Ordens = new();
        }
    }
}

