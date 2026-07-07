using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using BarberApplication.Console.Models;

namespace BarberApplication.Console.Data.Mongo;

/// <summary>
/// Registro dos BsonClassMap dos models para o driver do MongoDB.
/// Mantém os models limpos (sem atributos de Bson): o mapeamento fica todo aqui.
/// - A propriedade IdXxx vira o _id do documento.
/// - Os demais campos usam os mesmos nomes snake_case das colunas do MySQL.
/// - As propriedades de navegação (Cargo, Cliente, Profissional, Servico) NÃO são
///   persistidas — são preenchidas manualmente pelos services (equivalente ao Include do EF).
/// </summary>
public static class MongoMappings
{
    private static bool _registered;
    private static readonly object _lock = new();

    public static void Register()
    {
        if (_registered) return;
        lock (_lock)
        {
            if (_registered) return;

            BsonClassMap.RegisterClassMap<CargoModel>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.IdCargo);
                cm.GetMemberMap(c => c.Nome).SetElementName("nome");
            });

            BsonClassMap.RegisterClassMap<UsuarioModel>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(u => u.IdUsuario);
                cm.GetMemberMap(u => u.Cpf).SetElementName("cpf");
                cm.GetMemberMap(u => u.Nome).SetElementName("nome");
                cm.GetMemberMap(u => u.Telefone).SetElementName("telefone");
                cm.GetMemberMap(u => u.EndRua).SetElementName("end_rua");
                cm.GetMemberMap(u => u.EndBairro).SetElementName("end_bairro");
                cm.GetMemberMap(u => u.EndCep).SetElementName("end_cep");
                cm.GetMemberMap(u => u.IdCargo).SetElementName("id_cargo");
                cm.UnmapMember(u => u.Cargo);
            });

            BsonClassMap.RegisterClassMap<EspecialidadeModel>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(e => e.IdEspecialidade);
                cm.GetMemberMap(e => e.Nome).SetElementName("nome");
            });

            BsonClassMap.RegisterClassMap<ServicoModel>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(s => s.IdServico);
                cm.GetMemberMap(s => s.Nome).SetElementName("nome");
                cm.GetMemberMap(s => s.FkEspecialidade).SetElementName("fk_especialidade");
                cm.GetMemberMap(s => s.Preco).SetElementName("preco")
                    .SetSerializer(new DecimalSerializer(BsonType.Decimal128));
                cm.GetMemberMap(s => s.DuracaoEstimada).SetElementName("duracao_estimada");
            });

            BsonClassMap.RegisterClassMap<AtendimentoModel>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(a => a.IdAtendimento);
                cm.GetMemberMap(a => a.FkProfissional).SetElementName("fk_profissional");
                cm.GetMemberMap(a => a.FkCliente).SetElementName("fk_cliente");
                cm.GetMemberMap(a => a.FkServico).SetElementName("fk_servico");
                cm.GetMemberMap(a => a.DataHora).SetElementName("data_hora");
                cm.GetMemberMap(a => a.StatusAtendimento).SetElementName("status_atendimento");
                cm.UnmapMember(a => a.Cliente);
                cm.UnmapMember(a => a.Profissional);
                cm.UnmapMember(a => a.Servico);
            });

            // Tabela de junção (chave composta): sem _id próprio.
            // O _id (ObjectId) gerado pelo servidor é ignorado; a unicidade do par é
            // garantida por índice único composto (ver BarbeariaMongoContext.EnsureIndexesAsync).
            BsonClassMap.RegisterClassMap<ProfissionalEspecialidadeModel>(cm =>
            {
                cm.AutoMap();
                cm.GetMemberMap(pe => pe.FkProfissional).SetElementName("fk_profissional");
                cm.GetMemberMap(pe => pe.FkEspecialidade).SetElementName("fk_especialidade");
                cm.SetIgnoreExtraElements(true);
            });

            _registered = true;
        }
    }
}
