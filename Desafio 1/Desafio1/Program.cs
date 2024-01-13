using System;
using System.Collections.Generic;

class Program
{
    static List<Paciente> pacientesCadastrados = new List<Paciente>();
    static List<Agendamento> agendamentos = new List<Agendamento>();

    static void Main()
    {
        int opcao;

        do
        {
            Console.WriteLine("=== Clínica de Consultas Ágil ===");
            Console.WriteLine("1. Cadastrar Paciente");
            Console.WriteLine("2. Marcações de Consultas");
            Console.WriteLine("3. Cancelamento de Consultas");
            Console.WriteLine("4. Sair");

            Console.Write("Escolha uma opção: ");
            if (int.TryParse(Console.ReadLine(), out opcao))
            {
                switch (opcao)
                {
                    case 1:
                        CadastrarPaciente();
                        break;
                    case 2:
                        MarcarConsulta();
                        break;
                    case 3:
                        CancelarConsulta();
                        break;
                    case 4:
                        Console.WriteLine("Programa encerrado. Até logo!");
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Opção inválida. Tente novamente.");
            }

        } while (opcao != 4);
    }

    static void CadastrarPaciente()
    {
        Console.Write("Digite o nome do paciente: ");
        string nome = Console.ReadLine();

        Console.Write("Digite o telefone do paciente: ");
        string telefone = Console.ReadLine();

        if (pacientesCadastrados.Any(p => p.Telefone == telefone))
        {
            Console.WriteLine("Paciente já cadastrado!");
        }
        else
        {
            Paciente novoPaciente = new Paciente { Nome = nome, Telefone = telefone };
            pacientesCadastrados.Add(novoPaciente);
            Console.WriteLine("Paciente cadastrado com sucesso!");
        }
    }


    static void MarcarConsulta()
    {
        if (pacientesCadastrados.Count == 0)
        {
            Console.WriteLine("Não há pacientes cadastrados. Cadastre um paciente antes de marcar uma consulta.");
            return;
        }

        Console.WriteLine("Lista de Pacientes Cadastrados:");
        for (int i = 0; i < pacientesCadastrados.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {pacientesCadastrados[i].Nome}");
        }

        Console.Write("Escolha o número correspondente ao paciente: ");
        if (int.TryParse(Console.ReadLine(), out int indicePaciente) && indicePaciente >= 1 && indicePaciente <= pacientesCadastrados.Count)
        {
            Paciente pacienteSelecionado = pacientesCadastrados[indicePaciente - 1];

            Console.Write("Digite a data da consulta (DD/MM/AAAA): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime dataConsulta) && dataConsulta >= DateTime.Today)
            {
                Console.Write("Digite a hora da consulta (HH:mm): ");
                if (TimeSpan.TryParse(Console.ReadLine(), out TimeSpan horaConsulta))
                {
                    Console.Write("Digite a especialidade desejada: ");
                    string especialidade = Console.ReadLine();

            
                    if (!ConsultaDisponivel(dataConsulta, horaConsulta))
                    {
                        Console.WriteLine("Horário já ocupado. Escolha outro horário para a consulta.");
                        return;
                    }

                    Agendamento novoAgendamento = new Agendamento
                    {
                        Paciente = pacienteSelecionado,
                        DataHora = new DateTime(dataConsulta.Year, dataConsulta.Month, dataConsulta.Day, horaConsulta.Hours, horaConsulta.Minutes, 0),
                        Especialidade = especialidade
                    };

                    agendamentos.Add(novoAgendamento);
                    Console.WriteLine("Consulta marcada com sucesso!");
                }
                else
                {
                    Console.WriteLine("Formato de hora inválido. Tente novamente.");
                }
            }
            else
            {
                Console.WriteLine("Data inválida ou consulta retroativa. Tente novamente.");
            }
        }
        else
        {
            Console.WriteLine("Número de paciente inválido. Tente novamente.");
        }
    }

    static bool ConsultaDisponivel(DateTime dataConsulta, TimeSpan horaConsulta)
    {
        return !agendamentos.Any(a => a.DataHora.Date == dataConsulta.Date && a.DataHora.TimeOfDay == horaConsulta);
    }


    static void CancelarConsulta()
    {
        if (agendamentos.Count == 0)
        {
            Console.WriteLine("Não há consultas agendadas para cancelar.");
            return;
        }

        Console.WriteLine("Lista de Consultas Agendadas:");
        for (int i = 0; i < agendamentos.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {agendamentos[i].Paciente.Nome} - {agendamentos[i].DataHora:dd/MM/yyyy HH:mm} - {agendamentos[i].Especialidade}");
        }

        Console.Write("Escolha o número correspondente à consulta que deseja cancelar: ");
        if (int.TryParse(Console.ReadLine(), out int indiceConsulta) && indiceConsulta >= 1 && indiceConsulta <= agendamentos.Count)
        {
            Agendamento consultaSelecionada = agendamentos[indiceConsulta - 1];

            Console.WriteLine($"Consulta marcada para {consultaSelecionada.DataHora:dd/MM/yyyy HH:mm} - {consultaSelecionada.Especialidade}");

            Console.Write("Deseja realmente cancelar a consulta? (S/N): ");
            string resposta = Console.ReadLine().ToUpper();

            if (resposta == "S")
            {
                agendamentos.Remove(consultaSelecionada);
                Console.WriteLine("Consulta cancelada com sucesso!");
            }
            else
            {
                Console.WriteLine("Operação de cancelamento cancelada pelo usuário.");
            }
        }
        else
        {
            Console.WriteLine("Número de consulta inválido. Tente novamente.");
        }
    }

}

class Paciente
{
    public string Nome { get; set; }
    public string Telefone { get; set; }
}

class Agendamento
{
    public Paciente Paciente { get; set; }
    public DateTime DataHora { get; set; }
    public string Especialidade { get; set; }
}
