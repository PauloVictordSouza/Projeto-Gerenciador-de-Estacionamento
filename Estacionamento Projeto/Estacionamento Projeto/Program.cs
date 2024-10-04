using System;
using System.Collections.Generic;
using System.Linq;

namespace Estacionamento_Projeto
{
    public class Vaga
    {
        public string Numero { get; set; }
        public bool Ocupada { get; set; }
    }

    public class Cliente
    {
        public string nome { get; set; }
        public string horaEntrada { get; set; }
        public string horaSaida { get; set; }
        public string vaga { get; set; }
    }

    internal class Program
    {
        static List<Cliente> clientes = new List<Cliente>();
        static List<Vaga> vagas = new List<Vaga>();

        static void RegistrarSaida(Cliente cliente)
        {
            Console.Clear();
            Console.WriteLine($"Registrar saída para o cliente: {cliente.nome}");

            string horaSaida;
            while (true)
            {
                Console.Write("Hora de saída (HH:mm): ");
                horaSaida = Console.ReadLine();
                if (DateTime.TryParse(horaSaida, out _))
                    break;
                else
                    Console.WriteLine("Formato de hora inválido! Tente novamente.");
            }

            if (DateTime.Parse(horaSaida) <= DateTime.Parse(cliente.horaEntrada))
            {
                Console.WriteLine("A hora de saída deve ser posterior à hora de entrada.");
                return;
            }

            
            cliente.horaSaida = horaSaida;

            
            Vaga vagaDoCliente = vagas.FirstOrDefault(v => v.Numero == cliente.vaga);
            if (vagaDoCliente != null)
            {
                vagaDoCliente.Ocupada = false;
            }

            
            DateTime entrada = DateTime.Parse(cliente.horaEntrada);
            DateTime saida = DateTime.Parse(horaSaida);
            double duracaoHoras = (saida - entrada).TotalHours;
            double taxaPorHora = 5.00; //<- isso pode ser alterado de user pra user, é apenas o valor
            double valorTotal = Math.Ceiling(duracaoHoras) * taxaPorHora; 

            Console.WriteLine($"Saída registrada com sucesso para {cliente.nome} às {cliente.horaSaida}.");
            Console.WriteLine($"Valor total a ser pago: R$ {valorTotal:F2}.");
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu principal...");
            Console.ReadKey();
            Console.Clear();
        }

        static void ListarClientes()
        {
            if (clientes.Count == 0)
            {
                Console.WriteLine("Nenhum cliente registrado.");
                Console.WriteLine("Pressione qualquer tecla para voltar ao menu principal...");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            Console.Clear();
            Console.WriteLine("Lista de Clientes:");

            var clientesOrdenados = clientes.OrderBy(c => c.nome).ToList();

            for (int i = 0; i < clientesOrdenados.Count; i++)
            {
                Console.WriteLine($"{i + 1} - Cliente: {clientesOrdenados[i].nome}, Vaga: {clientesOrdenados[i].vaga}");
            }

            Console.WriteLine("Escolha o número do cliente para registrar a saída ou pressione 0 para voltar ao menu principal:");

            int escolha = Convert.ToInt32(Console.ReadLine());

            if (escolha == 0)
            {
                Console.Clear();
                return;
            }

            if (escolha > 0 && escolha <= clientesOrdenados.Count)
            {
                RegistrarSaida(clientesOrdenados[escolha - 1]);
            }
            else
            {
                Console.WriteLine("Escolha inválida! Tente novamente.");
            }
        }

        static void RegistrarCliente()
        {
            Console.Clear();
            Console.WriteLine("Registro de Cliente");

            Console.Write("Nome completo: ");
            string nome = Console.ReadLine();

            string horaEntrada;
            while (true)
            {
                Console.Write("Hora de entrada (HH:mm): ");
                horaEntrada = Console.ReadLine();
                if (DateTime.TryParse(horaEntrada, out _))
                    break;
                else
                    Console.WriteLine("Formato de hora inválido! Tente novamente.");
            }

            Console.Write("Vaga a ser ocupada: ");
            string vagaNumero = Console.ReadLine();

            Vaga vagaEscolhida = vagas.FirstOrDefault(v => v.Numero == vagaNumero);
            if (vagaEscolhida == null)
            {
                Console.WriteLine("Vaga inválida! Tente novamente.");
                return;
            }

            if (vagaEscolhida.Ocupada)
            {
                Console.WriteLine("A vaga já está ocupada! Tente outra vaga.");
                return;
            }

            vagaEscolhida.Ocupada = true;

            Cliente novoCliente = new Cliente
            {
                nome = nome,
                horaEntrada = horaEntrada,
                vaga = vagaNumero,
                horaSaida = null 
            };

            clientes.Add(novoCliente);
            Console.WriteLine($"Cliente {novoCliente.nome} registrado na vaga {novoCliente.vaga} com entrada às {novoCliente.horaEntrada}.");

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu principal...");
            Console.ReadKey();
            Console.Clear();
        }

        static void InicializarVagas()
        {
            for (int i = 1; i <= 60; i++)
            {
                vagas.Add(new Vaga { Numero = i.ToString(), Ocupada = false });
            }
        }

        static void ListarVagasDisponiveis()
        {
            Console.Clear();
            Console.WriteLine("Vagas Disponíveis:");
            foreach (var vaga in vagas)
            {
                if (!vaga.Ocupada)
                {
                    Console.WriteLine($"Vaga {vaga.Numero} está disponível.");
                }
                else
                {
                    Console.WriteLine($"Vaga {vaga.Numero} está ocupada");
                }
            }
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu principal...");
            Console.ReadKey();
            Console.Clear();
        }

        static void Main(string[] args)
        {
            InicializarVagas();
            while (true)
            {
                Console.WriteLine("Bem-vindo ao Gerenciador de Estacionamento!");
                Console.WriteLine("Por favor, selecione a opção desejada:");
                Console.WriteLine();
                Console.WriteLine("1 - Registrar um cliente");
                Console.WriteLine("2 - Verificar vaga");
                Console.WriteLine("3 - Registrar saída de cliente");
                Console.WriteLine("4 - Verificar histórico de clientes");
                Console.WriteLine("5 - Encerrar sistema");
                Console.WriteLine();

                int opcaoSelecionada = Convert.ToInt32(Console.ReadLine());

                switch (opcaoSelecionada)
                {
                    case 1:
                        RegistrarCliente();
                        break;
                    case 2:
                        ListarVagasDisponiveis();
                        break;
                    case 3:
                        ListarClientes();
                        break;
                    case 5:
                        Environment.Exit(0); 
                        break;
                    default:
                        Console.WriteLine("Opção inválida! Tente novamente.");
                        break;
                }
            }
        }
    }
}

