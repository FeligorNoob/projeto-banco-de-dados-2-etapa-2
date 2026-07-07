// ============================================================================
//  BarberFlow — Seed / Dump para MongoDB
//  Equivalente ao dump-BarberFlow-202605070336.sql (MySQL), com os MESMOS dados.
//
//  Como executar (com o mongosh instalado e o MongoDB no ar):
//      mongosh "mongodb://localhost:27017" mongo-seed-BarberFlow.js
//
//  O script recria o banco "BarberFlow" do zero (dropa as coleções antes de inserir),
//  então pode ser rodado quantas vezes quiser para restaurar os dados de exemplo.
//
//  Observações de mapeamento (para bater com o BsonClassMap da aplicação C#):
//    - A chave primária de cada entidade é o campo _id (inteiro), como no AUTO_INCREMENT.
//    - preco usa NumberDecimal (Decimal128); data_hora usa ISODate.
//    - A coleção "counters" guarda o ÚLTIMO id usado; o próximo id gerado pela app é +1.
//    - profissional_especialidade não tem _id próprio (chave composta) — a unicidade
//      do par é garantida por índice único composto.
// ============================================================================

const barber = db.getSiblingDB('BarberFlow');

// ---- Limpa as coleções (recomeça do zero) --------------------------------
barber.usuario.drop();
barber.cargo.drop();
barber.especialidade.drop();
barber.servico.drop();
barber.profissional_especialidade.drop();
barber.atendimento.drop();
barber.counters.drop();

// ---- cargo ----------------------------------------------------------------
barber.cargo.insertMany([
    { _id: 1, nome: 'Administrador' },
    { _id: 2, nome: 'Profissional' },
    { _id: 3, nome: 'Cliente' }
]);

// ---- especialidade --------------------------------------------------------
barber.especialidade.insertMany([
    { _id: 1, nome: 'Cortes de Cabelo' },
    { _id: 2, nome: 'Barba' },
    { _id: 3, nome: 'Colorimetria' }
]);

// ---- usuario --------------------------------------------------------------
// Campos: _id, cpf, nome, telefone, end_rua, end_bairro, end_cep, id_cargo
barber.usuario.insertMany([
    { _id: 1,  cpf: '99999999999', nome: 'Administrador Sistema', telefone: '47999999999', end_rua: null, end_bairro: null,             end_cep: null, id_cargo: 1 },
    { _id: 2,  cpf: '11111111102', nome: 'Ricardo Navalha',       telefone: '47988000002', end_rua: null, end_bairro: null,             end_cep: null, id_cargo: 2 },
    { _id: 3,  cpf: '11111111103', nome: 'Thiago Barba',          telefone: '47988000003', end_rua: null, end_bairro: null,             end_cep: null, id_cargo: 2 },
    { _id: 4,  cpf: '11111111104', nome: 'Carlos Colorista',      telefone: '47988000004', end_rua: null, end_bairro: null,             end_cep: null, id_cargo: 2 },
    { _id: 5,  cpf: '11111111105', nome: 'Marcos Visagista',      telefone: '47988000005', end_rua: null, end_bairro: null,             end_cep: null, id_cargo: 2 },
    { _id: 6,  cpf: '11111111106', nome: 'Paulo Tesoura',         telefone: '47988000006', end_rua: null, end_bairro: null,             end_cep: null, id_cargo: 2 },
    { _id: 7,  cpf: '11111111107', nome: 'Felipe Degradê',        telefone: '47988000007', end_rua: null, end_bairro: null,             end_cep: null, id_cargo: 2 },
    { _id: 8,  cpf: '11111111108', nome: 'João Estilo',           telefone: '47988000008', end_rua: null, end_bairro: null,             end_cep: null, id_cargo: 2 },
    { _id: 9,  cpf: '11111111109', nome: 'Lucas Barboterapia',    telefone: '47988000009', end_rua: null, end_bairro: null,             end_cep: null, id_cargo: 2 },
    { _id: 10, cpf: '11111111110', nome: 'André Navalhete',       telefone: '47988000010', end_rua: null, end_bairro: null,             end_cep: null, id_cargo: 2 },
    { _id: 11, cpf: '11111111111', nome: 'Bruno Fade',            telefone: '47988000011', end_rua: null, end_bairro: null,             end_cep: null, id_cargo: 2 },
    { _id: 12, cpf: '22222222212', nome: 'João Silva',            telefone: '47911112212', end_rua: null, end_bairro: 'Bom Retiro',     end_cep: null, id_cargo: 3 },
    { _id: 13, cpf: '22222222213', nome: 'Pedro Santos',          telefone: '47911112213', end_rua: null, end_bairro: 'Paranaguamirim', end_cep: null, id_cargo: 3 },
    { _id: 14, cpf: '22222222214', nome: 'Lucas Oliveira',        telefone: '47911112214', end_rua: null, end_bairro: 'Paranaguamirim', end_cep: null, id_cargo: 3 },
    { _id: 15, cpf: '22222222215', nome: 'Mateus Souza',          telefone: '47911112215', end_rua: null, end_bairro: 'Bom Retiro',     end_cep: null, id_cargo: 3 },
    { _id: 16, cpf: '22222222216', nome: 'Gabriel Lima',          telefone: '47911112216', end_rua: null, end_bairro: 'Aventureiro',    end_cep: null, id_cargo: 3 },
    { _id: 17, cpf: '22222222217', nome: 'Rafael Costa',          telefone: '47911112217', end_rua: null, end_bairro: 'Aventureiro',    end_cep: null, id_cargo: 3 },
    { _id: 18, cpf: '22222222218', nome: 'Daniel Rocha',          telefone: '47911112218', end_rua: null, end_bairro: 'Iririú',         end_cep: null, id_cargo: 3 },
    { _id: 19, cpf: '22222222219', nome: 'Bruno Alves',           telefone: '47911112219', end_rua: null, end_bairro: 'Iririú',         end_cep: null, id_cargo: 3 },
    { _id: 20, cpf: '22222222220', nome: 'Tiago Pereira',         telefone: '47911112220', end_rua: null, end_bairro: 'Costa e Silva',  end_cep: null, id_cargo: 3 },
    { _id: 21, cpf: '3333333341',  nome: 'Cliente Mock 41',       telefone: null,          end_rua: null, end_bairro: 'Paranaguamirim', end_cep: null, id_cargo: 3 },
    { _id: 22, cpf: '3333333331',  nome: 'Cliente Mock 31',       telefone: null,          end_rua: null, end_bairro: 'Paranaguamirim', end_cep: null, id_cargo: 3 },
    { _id: 23, cpf: '3333333321',  nome: 'Cliente Mock 21',       telefone: null,          end_rua: null, end_bairro: 'Paranaguamirim', end_cep: null, id_cargo: 3 },
    { _id: 24, cpf: '3333333342',  nome: 'Cliente Mock 42',       telefone: null,          end_rua: null, end_bairro: 'Aventureiro',    end_cep: null, id_cargo: 3 },
    { _id: 25, cpf: '3333333332',  nome: 'Cliente Mock 32',       telefone: null,          end_rua: null, end_bairro: 'Aventureiro',    end_cep: null, id_cargo: 3 },
    { _id: 26, cpf: '3333333322',  nome: 'Cliente Mock 22',       telefone: null,          end_rua: null, end_bairro: 'Aventureiro',    end_cep: null, id_cargo: 3 },
    { _id: 27, cpf: '3333333343',  nome: 'Cliente Mock 43',       telefone: null,          end_rua: null, end_bairro: 'Iririú',         end_cep: null, id_cargo: 3 },
    { _id: 28, cpf: '3333333333',  nome: 'Cliente Mock 33',       telefone: null,          end_rua: null, end_bairro: 'Iririú',         end_cep: null, id_cargo: 3 },
    { _id: 29, cpf: '3333333323',  nome: 'Cliente Mock 23',       telefone: null,          end_rua: null, end_bairro: 'Iririú',         end_cep: null, id_cargo: 3 },
    { _id: 30, cpf: '3333333344',  nome: 'Cliente Mock 44',       telefone: null,          end_rua: null, end_bairro: 'Costa e Silva',  end_cep: null, id_cargo: 3 },
    { _id: 31, cpf: '3333333334',  nome: 'Cliente Mock 34',       telefone: null,          end_rua: null, end_bairro: 'Costa e Silva',  end_cep: null, id_cargo: 3 },
    { _id: 36, cpf: '12312312312', nome: 'Fulano teste',          telefone: null,          end_rua: null, end_bairro: null,             end_cep: null, id_cargo: 1 }
]);

// ---- servico --------------------------------------------------------------
// Campos: _id, nome, fk_especialidade, preco, duracao_estimada
barber.servico.insertMany([
    { _id: 1, nome: 'Corte Social',  fk_especialidade: 1, preco: NumberDecimal('50.00'), duracao_estimada: 40 },
    { _id: 2, nome: 'Corte Degradê', fk_especialidade: 1, preco: NumberDecimal('55.00'), duracao_estimada: 50 },
    { _id: 3, nome: 'Colorir Cabelo', fk_especialidade: 3, preco: NumberDecimal('75.00'), duracao_estimada: 90 },
    { _id: 4, nome: 'Aparar Barba',  fk_especialidade: 2, preco: NumberDecimal('40.00'), duracao_estimada: 30 }
]);

// ---- profissional_especialidade (chave composta, sem _id próprio) ---------
barber.profissional_especialidade.insertMany([
    { fk_profissional: 2,  fk_especialidade: 1 },
    { fk_profissional: 7,  fk_especialidade: 1 },
    { fk_profissional: 11, fk_especialidade: 1 },
    { fk_profissional: 2,  fk_especialidade: 2 },
    { fk_profissional: 3,  fk_especialidade: 2 },
    { fk_profissional: 4,  fk_especialidade: 3 }
]);

// ---- atendimento ----------------------------------------------------------
// Campos: _id, fk_profissional, fk_cliente, fk_servico, data_hora, status_atendimento
barber.atendimento.insertMany([
    { _id: 1,  fk_profissional: 2,  fk_cliente: 12, fk_servico: 1, data_hora: ISODate('2026-04-20T14:00:00Z'), status_atendimento: 'R' },
    { _id: 2,  fk_profissional: 3,  fk_cliente: 13, fk_servico: 4, data_hora: ISODate('2026-04-21T09:00:00Z'), status_atendimento: 'A' },
    { _id: 3,  fk_profissional: 2,  fk_cliente: 14, fk_servico: 2, data_hora: ISODate('2026-04-21T10:00:00Z'), status_atendimento: 'A' },
    { _id: 4,  fk_profissional: 4,  fk_cliente: 15, fk_servico: 3, data_hora: ISODate('2026-04-21T11:30:00Z'), status_atendimento: 'R' },
    { _id: 5,  fk_profissional: 7,  fk_cliente: 16, fk_servico: 1, data_hora: ISODate('2026-04-22T10:00:00Z'), status_atendimento: 'R' },
    { _id: 6,  fk_profissional: 11, fk_cliente: 17, fk_servico: 1, data_hora: ISODate('2026-04-22T14:00:00Z'), status_atendimento: 'A' },
    { _id: 7,  fk_profissional: 4,  fk_cliente: 18, fk_servico: 3, data_hora: ISODate('2026-04-23T15:00:00Z'), status_atendimento: 'R' },
    { _id: 8,  fk_profissional: 2,  fk_cliente: 19, fk_servico: 2, data_hora: ISODate('2026-04-23T16:30:00Z'), status_atendimento: 'R' },
    { _id: 9,  fk_profissional: 3,  fk_cliente: 20, fk_servico: 4, data_hora: ISODate('2026-04-24T09:00:00Z'), status_atendimento: 'R' },
    { _id: 10, fk_profissional: 2,  fk_cliente: 21, fk_servico: 1, data_hora: ISODate('2026-04-24T10:00:00Z'), status_atendimento: 'R' },
    { _id: 11, fk_profissional: 7,  fk_cliente: 22, fk_servico: 1, data_hora: ISODate('2026-04-24T11:00:00Z'), status_atendimento: 'R' },
    { _id: 12, fk_profissional: 4,  fk_cliente: 23, fk_servico: 3, data_hora: ISODate('2026-04-24T14:00:00Z'), status_atendimento: 'R' },
    { _id: 13, fk_profissional: 3,  fk_cliente: 24, fk_servico: 4, data_hora: ISODate('2026-04-24T15:30:00Z'), status_atendimento: 'C' },
    { _id: 14, fk_profissional: 11, fk_cliente: 25, fk_servico: 1, data_hora: ISODate('2026-04-25T09:00:00Z'), status_atendimento: 'R' },
    { _id: 17, fk_profissional: 11, fk_cliente: 12, fk_servico: 4, data_hora: ISODate('2026-02-12T12:00:00Z'), status_atendimento: 'A' }
]);

// ---- counters (último id usado por entidade → próximo gerado pela app = +1) ----
barber.counters.insertMany([
    { _id: 'cargo',         seq: 3 },
    { _id: 'usuario',       seq: 36 },
    { _id: 'especialidade', seq: 3 },
    { _id: 'servico',       seq: 4 },
    { _id: 'atendimento',   seq: 17 }
]);

// ---- índices (equivalentes às constraints do MySQL) -----------------------
barber.usuario.createIndex({ cpf: 1 }, { unique: true, name: 'ux_usuario_cpf' });
barber.profissional_especialidade.createIndex(
    { fk_profissional: 1, fk_especialidade: 1 },
    { unique: true, name: 'ux_prof_esp' }
);

print('');
print('===================================================');
print(' Seed do BarberFlow (MongoDB) concluído com sucesso!');
print('   usuarios:      ' + barber.usuario.countDocuments());
print('   cargos:        ' + barber.cargo.countDocuments());
print('   especialidades:' + barber.especialidade.countDocuments());
print('   servicos:      ' + barber.servico.countDocuments());
print('   vinculos:      ' + barber.profissional_especialidade.countDocuments());
print('   atendimentos:  ' + barber.atendimento.countDocuments());
print('===================================================');
