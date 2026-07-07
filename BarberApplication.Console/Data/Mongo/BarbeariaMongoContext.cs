using MongoDB.Bson;
using MongoDB.Driver;
using BarberApplication.Console.Models;

namespace BarberApplication.Console.Data.Mongo;

/// <summary>
/// Contexto de acesso ao MongoDB. Expõe as coleções (equivalente aos DbSets do EF)
/// e utilitários: gerador de ids sequenciais (emula AUTO_INCREMENT) e criação de índices.
/// </summary>
public class BarbeariaMongoContext(IMongoDatabase database)
{
    public IMongoCollection<UsuarioModel> Usuarios => database.GetCollection<UsuarioModel>("usuario");
    public IMongoCollection<CargoModel> Cargos => database.GetCollection<CargoModel>("cargo");
    public IMongoCollection<EspecialidadeModel> Especialidades => database.GetCollection<EspecialidadeModel>("especialidade");
    public IMongoCollection<ServicoModel> Servicos => database.GetCollection<ServicoModel>("servico");
    public IMongoCollection<ProfissionalEspecialidadeModel> ProfissionalEspecialidades => database.GetCollection<ProfissionalEspecialidadeModel>("profissional_especialidade");
    public IMongoCollection<AtendimentoModel> Atendimentos => database.GetCollection<AtendimentoModel>("atendimento");

    private IMongoCollection<BsonDocument> Counters => database.GetCollection<BsonDocument>("counters");

    /// <summary>
    /// Gera o próximo id inteiro sequencial para uma coleção (emula o AUTO_INCREMENT do MySQL).
    /// Usa a coleção "counters" com incremento atômico ($inc) + upsert.
    /// </summary>
    public async Task<int> GetNextSequenceAsync(string nome)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", nome);
        var update = Builders<BsonDocument>.Update.Inc("seq", 1);
        var options = new FindOneAndUpdateOptions<BsonDocument>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };
        var result = await Counters.FindOneAndUpdateAsync(filter, update, options);
        return result["seq"].AsInt32;
    }

    /// <summary>
    /// Garante os índices equivalentes às constraints do MySQL:
    /// CPF único em usuario e chave composta única em profissional_especialidade.
    /// </summary>
    public async Task EnsureIndexesAsync()
    {
        var cpfIndex = new CreateIndexModel<UsuarioModel>(
            Builders<UsuarioModel>.IndexKeys.Ascending(u => u.Cpf),
            new CreateIndexOptions { Unique = true, Name = "ux_usuario_cpf" });
        await Usuarios.Indexes.CreateOneAsync(cpfIndex);

        var profEspIndex = new CreateIndexModel<ProfissionalEspecialidadeModel>(
            Builders<ProfissionalEspecialidadeModel>.IndexKeys
                .Ascending(pe => pe.FkProfissional)
                .Ascending(pe => pe.FkEspecialidade),
            new CreateIndexOptions { Unique = true, Name = "ux_prof_esp" });
        await ProfissionalEspecialidades.Indexes.CreateOneAsync(profEspIndex);
    }
}
