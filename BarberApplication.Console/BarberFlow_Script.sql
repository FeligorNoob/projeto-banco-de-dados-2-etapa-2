CREATE DATABASE IF NOT EXISTS BarberFlow;
USE BarberFlow;

-- 2. TABELA CARGO (Precisa ser criada antes de usuario)
-- CARGO DA PERMISÃO E ATRIBUIÇÃO PARA CADA PERFIL LOGADO NA PLATAFORMA
CREATE TABLE cargo (
    id_cargo INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(30) NOT NULL
);
INSERT INTO cargo (nome) VALUES 
('Administrador'),
('Profissional'),
('Cliente');


-- 1. TABELA USUÁRIO (Base para Clientes e Profissionais)
CREATE TABLE usuario (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    cpf CHAR(11) NOT NULL UNIQUE,
    nome VARCHAR(100) NOT NULL,
    telefone VARCHAR(15),
    end_rua VARCHAR(100),
    end_bairro VARCHAR(50),
    end_cep CHAR(8),
    id_cargo INT NOT NULL,
    FOREIGN KEY (id_cargo) REFERENCES cargo(id_cargo)
);

-- Inserindo 50 usuários (1 Admin + 10 Profissionais + 39 Clientes)
INSERT INTO usuario (id_usuario, cpf, nome, telefone, id_cargo, end_bairro) VALUES
(1, '99999999999', 'Administrador Sistema', '47999999999', 1, NULL),
-- Profissionais (ID 2 ao 11)
(2, '11111111102', 'Ricardo Navalha', '47988000002', 2, NULL),
(3, '11111111103', 'Thiago Barba', '47988000003', 2, NULL),
(4, '11111111104', 'Carlos Colorista', '47988000004', 2, NULL),
(5, '11111111105', 'Marcos Visagista', '47988000005', 2, NULL),
(6, '11111111106', 'Paulo Tesoura', '47988000006', 2, NULL),
(7, '11111111107', 'Felipe Degradê', '47988000007', 2, NULL),
(8, '11111111108', 'João Estilo', '47988000008', 2, NULL),
(9, '11111111109', 'Lucas Barboterapia', '47988000009', 2, NULL),
(10, '11111111110', 'André Navalhete', '47988000010', 2, NULL),
(11, '11111111111', 'Bruno Fade', '47988000011', 2, NULL),
-- Clientes (ID 12 ao 20) - Exemplos mockados com telefone e bairro
(12, '22222222212', 'João Silva', '47911112212', 3, 'Bom Retiro'),
(13, '22222222213', 'Pedro Santos', '47911112213', 3, 'Paranaguamirim'),
(14, '22222222214', 'Lucas Oliveira', '47911112214', 3, 'Paranaguamirim'),
(15, '22222222215', 'Mateus Souza', '47911112215', 3, 'Bom Retiro'),
(16, '22222222216', 'Gabriel Lima', '47911112216', 3, 'Aventureiro'),
(17, '22222222217', 'Rafael Costa', '47911112217', 3, 'Aventureiro'),
(18, '22222222218', 'Daniel Rocha', '47911112218', 3, 'Iririú'),
(19, '22222222219', 'Bruno Alves', '47911112219', 3, 'Iririú'),
(20, '22222222220', 'Tiago Pereira', '47911112220', 3, 'Costa e Silva');

-- Clientes (ID 21 ao 50) - Dinamicamente (Com id_cargo = 3)
INSERT INTO usuario (cpf, nome, id_cargo, end_bairro) 
SELECT CONCAT('33333333', id), CONCAT('Cliente Mock ', id), 3, 
       ELT(1 + MOD(id, 5), 'Bom Retiro', 'Paranaguamirim', 'Aventureiro', 'Iririú', 'Costa e Silva')
FROM (SELECT (t1.a + t2.a*10 + 21) as id FROM (SELECT 0 AS a UNION SELECT 1 UNION SELECT 2 UNION SELECT 3) AS t1, (SELECT 0 AS a UNION SELECT 1 UNION SELECT 2) AS t2) AS sub WHERE id <= 50;


-- 4. TABELA ESPECIALIDADE
CREATE TABLE especialidade (
    id_especialidade INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(50) NOT NULL
);
INSERT INTO especialidade (nome) VALUES 
('Cortes de Cabelo'), -- ID 1
('Barba'),            -- ID 2
('Colorimetria');     -- ID 3


-- 5. ASSOCIAÇÃO PROFISSIONAL_ESPECIALIDADE
CREATE TABLE profissional_especialidade (
    fk_profissional INT,
    fk_especialidade INT,
    PRIMARY KEY (fk_profissional, fk_especialidade),
    FOREIGN KEY (fk_profissional) REFERENCES usuario(id_usuario) ON DELETE CASCADE,
    FOREIGN KEY (fk_especialidade) REFERENCES especialidade(id_especialidade)
);
INSERT INTO profissional_especialidade (fk_profissional, fk_especialidade) VALUES
(2, 1), (2, 2), -- Ricardo faz corte e barba
(3, 2),         -- Thiago foca em barba
(4, 3), (4, 1), -- Carlos faz cor e corte
(7, 1), (11, 1); -- Especialistas em corte


-- 6. TABELA SERVIÇO
CREATE TABLE servico (
    id_servico INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(50) NOT NULL,
    fk_especialidade INT,
    preco DECIMAL(10, 2) NOT NULL,
    duracao_estimada INT NOT NULL, 
    FOREIGN KEY (fk_especialidade) REFERENCES especialidade(id_especialidade)
);
INSERT INTO servico (nome, fk_especialidade, preco, duracao_estimada) VALUES 
('Corte Social', 1, 50.00, 40),
('Corte Degradê', 1, 55.00, 50),
('Colorir Cabelo', 3, 75.00, 90),
('Aparar Barba', 2, 40.00, 30);


-- 7. TABELA ATENDIMENTO
CREATE TABLE atendimento (
    id_atendimento INT AUTO_INCREMENT PRIMARY KEY,
    fk_profissional INT,
    fk_cliente INT,
    fk_servico INT,
    data_hora DATETIME NOT NULL,
    status_atendimento ENUM('A', 'R', 'C') DEFAULT 'A' COMMENT 'A=Agendado, R=Realizado, C=Cancelado',
    FOREIGN KEY (fk_profissional) REFERENCES usuario(id_usuario),
    FOREIGN KEY (fk_cliente) REFERENCES usuario(id_usuario),
    FOREIGN KEY (fk_servico) REFERENCES servico(id_servico)
);

-- Mock de atendimentos (Agendados e Realizados)
INSERT INTO atendimento (fk_profissional, fk_cliente, fk_servico, data_hora, status_atendimento) VALUES
(2, 12, 1, '2026-04-20 14:00:00', 'R'), -- Realizado - João Silva (Bom Retiro) - Corte
(3, 13, 4, '2026-04-21 09:00:00', 'A'), -- Agendado - Pedro Santos (Paranaguamirim) - Barba
(2, 14, 2, '2026-04-21 10:00:00', 'A'), -- Agendado - Lucas Oliveira (Paranaguamirim) - Corte Degradê
(4, 15, 3, '2026-04-21 11:30:00', 'R'), -- Realizado - Mateus Souza (Bom Retiro) - Colorir
(7, 16, 1, '2026-04-22 10:00:00', 'R'), -- Realizado - Gabriel Lima (Aventureiro) - Corte
(11, 17, 1, '2026-04-22 14:00:00', 'A'), -- Agendado - Rafael Costa (Aventureiro) - Corte
(4, 18, 3, '2026-04-23 15:00:00', 'R'), -- Realizado - Daniel Rocha (Iririú) - Colorir
(2, 19, 2, '2026-04-23 16:30:00', 'R'), -- Realizado - Bruno Alves (Iririú) - Corte Degradê
(3, 20, 4, '2026-04-24 09:00:00', 'R'), -- Realizado - Tiago Pereira (Costa e Silva) - Barba
(2, 21, 1, '2026-04-24 10:00:00', 'R'), -- Realizado - Cliente Mock 21 (Paranaguamirim) - Corte
(7, 22, 1, '2026-04-24 11:00:00', 'R'), -- Realizado - Cliente Mock 22 (Aventureiro) - Corte
(4, 23, 3, '2026-04-24 14:00:00', 'R'), -- Realizado - Cliente Mock 23 (Iririú) - Colorir
(3, 24, 4, '2026-04-24 15:30:00', 'C'), -- Cancelado - Cliente Mock 24 (Costa e Silva) - Barba
(11, 25, 1, '2026-04-25 09:00:00', 'R'), -- Realizado - Cliente Mock 25 (Bom Retiro) - Corte
(2, 26, 2, '2026-04-25 10:30:00', 'R'); -- Realizado - Cliente Mock 26 (Paranaguamirim) - Corte Degradê
