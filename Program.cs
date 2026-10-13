using System;

namespace ProjetoRestaurante
{
    public class Item
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public double Preco { get; set; }

        public Item(int id, string descricao, double preco)
        {
            Id = id;
            Descricao = descricao;
            Preco = preco;
        }
    }

    public class Pedido
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public Item[] Itens { get; set; } = new Item[10];
        private int qtdItens = 0;

        public Pedido(int id, string cliente)
        {
            Id = id;
            Cliente = cliente;
        }

        public bool AdicionarItem(Item item)
        {
            if (qtdItens < 10)
            {
                Itens[qtdItens] = item;
                qtdItens++;
                return true;
            }
            return false;
        }

        public bool RemoverItem(Item item)
        {
            for (int i = 0; i < qtdItens; i++)
            {
                if (Itens[i] != null && Itens[i].Id == item.Id)
                {
                    for (int j = i; j < qtdItens - 1; j++)
                    {
                        Itens[j] = Itens[j + 1];
                    }
                    Itens[qtdItens - 1] = null;
                    qtdItens--;
                    return true;
                }
            }
            return false;
        }

        public double CalcularTotal()
        {
            double total = 0;
            for (int i = 0; i < qtdItens; i++)
            {
                if (Itens[i] != null)
                {
                    total += Itens[i].Preco;
                }
            }
            return total;
        }

        public string DadosDoPedido()
        {
            string dados = $"ID do Pedido: {Id} | Cliente: {Cliente}\nItens:\n";
            for (int i = 0; i < qtdItens; i++)
            {
                if (Itens[i] != null)
                {
                    dados += $" - [{Itens[i].Id}] {Itens[i].Descricao} : R$ {Itens[i].Preco:F2}\n";
                }
            }
            dados += $"Valor Total: R$ {CalcularTotal():F2}";
            return dados;
        }
    }

    public class Restaurante
    {
        private int proxPedido = 1;
        private Pedido[] pedidos = new Pedido[50];

        public bool NovoPedido(Pedido pedido)
        {
            for (int i = 0; i < pedidos.Length; i++)
            {
                if (pedidos[i] == null)
                {
                    pedidos[i] = pedido;
                    return true;
                }
            }
            return false;
        }

        public Pedido BuscarPedido(int id)
        {
            for (int i = 0; i < pedidos.Length; i++)
            {
                if (pedidos[i] != null && pedidos[i].Id == id)
                {
                    return pedidos[i];
                }
            }
            return null;
        }

        public bool CancelarPedido(Pedido pedido)
        {
            for (int i = 0; i < pedidos.Length; i++)
            {
                if (pedidos[i] != null && pedidos[i].Id == pedido.Id)
                {
                    pedidos[i] = null;
                    return true;
                }
            }
            return false;
        }

        public void ListarTodos()
        {
            double somaGeral = 0;
            Console.WriteLine("\n--- HISTÓRICO DE PEDIDOS DO DIA ---");
            bool encontrou = false;
            for (int i = 0; i < pedidos.Length; i++)
            {
                if (pedidos[i] != null)
                {
                    encontrou = true;
                    double total = pedidos[i].CalcularTotal();
                    somaGeral += total;
                    Console.WriteLine($"ID: {pedidos[i].Id} | Cliente: {pedidos[i].Cliente} | Total: R$ {total:F2}");
                }
            }
            if (!encontrou)
            {
                Console.WriteLine("Nenhum pedido registrado.");
            }
            else
            {
                Console.WriteLine($"\nSoma Geral do Dia: R$ {somaGeral:F2}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Restaurante restaurante = new Restaurante();
            int opcao;

            do
            {
                Console.WriteLine("\n=== OPÇÕES DO SELETOR ===");
                Console.WriteLine("0. Sair");
                Console.WriteLine("1. Criar novo pedido");
                Console.WriteLine("2. Adicionar item ao pedido");
                Console.WriteLine("3. Remover item do pedido");
                Console.WriteLine("4. Consultar pedido (mostrar id, cliente, itens e valor total)");
                Console.WriteLine("5. Cancelar pedido");
                Console.WriteLine("6. Listar todos os pedidos (com id, valores totais e soma geral do dia)");
                Console.Write("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Opção inválida! Digite um número.");
                    continue;
                }

                switch (opcao)
                {
                    case 0:
                        Console.WriteLine("Saindo do sistema...");
                        break;

                    case 1:
                        Console.Write("Nome do cliente: ");
                        string cliente = Console.ReadLine();
                        int idPedido = new Random().Next(100, 999);
                        Pedido novoPedido = new Pedido(idPedido, cliente);
                        if (restaurante.NovoPedido(novoPedido))
                        {
                            Console.WriteLine($"Pedido criado com sucesso! ID atribuído: {idPedido}");
                        }
                        else
                        {
                            Console.WriteLine("Erro: Limite diário de pedidos atingido.");
                        }
                        break;

                    case 2:
                        Console.Write("Digite o ID do pedido: ");
                        if (int.TryParse(Console.ReadLine(), out int idAdd))
                        {
                            Pedido pAdd = restaurante.BuscarPedido(idAdd);
                            if (pAdd != null)
                            {
                                Console.Write("ID do item: ");
                                int idItem = int.Parse(Console.ReadLine());
                                Console.Write("Descrição do item: ");
                                string descItem = Console.ReadLine();
                                Console.Write("Preço do item: ");
                                double precoItem = double.Parse(Console.ReadLine());

                                Item item = new Item(idItem, descItem, precoItem);
                                if (pAdd.AdicionarItem(item))
                                {
                                    Console.WriteLine("Item adicionado com sucesso!");
                                }
                                else
                                {
                                    Console.WriteLine("Erro: O pedido atingiu o limite máximo de 10 itens.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Pedido não encontrado.");
                            }
                        }
                        break;

                    case 3:
                        Console.Write("Digite o ID do pedido: ");
                        if (int.TryParse(Console.ReadLine(), out int idRem))
                        {
                            Pedido pRem = restaurante.BuscarPedido(idRem);
                            if (pRem != null)
                            {
                                Console.Write("Digite o ID do item que deseja remover: ");
                                int idItemRem = int.Parse(Console.ReadLine());
                                Item dummyItem = new Item(idItemRem, "", 0);

                                if (pRem.RemoverItem(dummyItem))
                                {
                                    Console.WriteLine("Item removido com sucesso!");
                                }
                                else
                                {
                                    Console.WriteLine("Item não encontrado no pedido.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Pedido não encontrado.");
                            }
                        }
                        break;

                    case 4:
                        Console.Write("Digite o ID do pedido que deseja consultar: ");
                        if (int.TryParse(Console.ReadLine(), out int idCons))
                        {
                            Pedido pCons = restaurante.BuscarPedido(idCons);
                            if (pCons != null)
                            {
                                Console.WriteLine("\n" + pCons.DadosDoPedido());
                            }
                            else
                            {
                                Console.WriteLine("Pedido não encontrado.");
                            }
                        }
                        break;

                    case 5:
                        Console.Write("Digite o ID do pedido que deseja cancelar: ");
                        if (int.TryParse(Console.ReadLine(), out int idCanc))
                        {
                            Pedido pCanc = restaurante.BuscarPedido(idCanc);
                            if (pCanc != null)
                            {
                                if (restaurante.CancelarPedido(pCanc))
                                {
                                    Console.WriteLine("Pedido cancelado com sucesso!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Pedido não encontrado.");
                            }
                        }
                        break;

                    case 6:
                        restaurante.ListarTodos();
                        break;

                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }

            } while (opcao != 0);
        }
    }
}
