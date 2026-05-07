-- MySQL dump 10.13  Distrib 8.0.19, for Win64 (x86_64)
--
-- Host: localhost    Database: BarberFlow
-- ------------------------------------------------------
-- Server version	9.6.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
SET @MYSQLDUMP_TEMP_LOG_BIN = @@SESSION.SQL_LOG_BIN;
SET @@SESSION.SQL_LOG_BIN= 0;

--
-- GTID state at the beginning of the backup 
--

SET @@GLOBAL.GTID_PURGED=/*!80000 '+'*/ '414809e3-3c64-11f1-9874-dae55a063139:1-132';

--
-- Table structure for table `cargo`
--

DROP TABLE IF EXISTS `cargo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cargo` (
  `id_cargo` int NOT NULL,
  `nome` varchar(30) NOT NULL,
  PRIMARY KEY (`id_cargo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cargo`
--

LOCK TABLES `cargo` WRITE;
/*!40000 ALTER TABLE `cargo` DISABLE KEYS */;
INSERT INTO `cargo` VALUES (1,'Administrador'),(2,'Profissional'),(3,'Cliente');
/*!40000 ALTER TABLE `cargo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuario` (
  `id_usuario` int NOT NULL AUTO_INCREMENT,
  `cpf` char(11) NOT NULL,
  `nome` varchar(100) NOT NULL,
  `telefone` varchar(15) DEFAULT NULL,
  `end_rua` varchar(100) DEFAULT NULL,
  `end_bairro` varchar(50) DEFAULT NULL,
  `end_cep` char(8) DEFAULT NULL,
  `id_cargo` int NOT NULL,
  PRIMARY KEY (`id_usuario`),
  UNIQUE KEY `cpf` (`cpf`),
  KEY `id_cargo` (`id_cargo`),
  CONSTRAINT `usuario_ibfk_1` FOREIGN KEY (`id_cargo`) REFERENCES `cargo` (`id_cargo`)
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` VALUES (1,'99999999999','Administrador Sistema','47999999999',NULL,NULL,NULL,1),(2,'11111111102','Ricardo Navalha','47988000002',NULL,NULL,NULL,2),(3,'11111111103','Thiago Barba','47988000003',NULL,NULL,NULL,2),(4,'11111111104','Carlos Colorista','47988000004',NULL,NULL,NULL,2),(5,'11111111105','Marcos Visagista','47988000005',NULL,NULL,NULL,2),(6,'11111111106','Paulo Tesoura','47988000006',NULL,NULL,NULL,2),(7,'11111111107','Felipe Degradê','47988000007',NULL,NULL,NULL,2),(8,'11111111108','João Estilo','47988000008',NULL,NULL,NULL,2),(9,'11111111109','Lucas Barboterapia','47988000009',NULL,NULL,NULL,2),(10,'11111111110','André Navalhete','47988000010',NULL,NULL,NULL,2),(11,'11111111111','Bruno Fade','47988000011',NULL,NULL,NULL,2),(12,'22222222212','João Silva','47911112212',NULL,NULL,NULL,3),(13,'22222222213','Pedro Santos','47911112213',NULL,NULL,NULL,3),(14,'22222222214','Lucas Oliveira','47911112214',NULL,NULL,NULL,3),(15,'22222222215','Mateus Souza','47911112215',NULL,NULL,NULL,3),(16,'22222222216','Gabriel Lima','47911112216',NULL,NULL,NULL,3),(17,'22222222217','Rafael Costa','47911112217',NULL,NULL,NULL,3),(18,'22222222218','Daniel Rocha','47911112218',NULL,NULL,NULL,3),(19,'22222222219','Bruno Alves','47911112219',NULL,NULL,NULL,3),(20,'22222222220','Tiago Pereira','47911112220',NULL,NULL,NULL,3),(21,'3333333341','Cliente Mock 41',NULL,NULL,NULL,NULL,3),(22,'3333333331','Cliente Mock 31',NULL,NULL,NULL,NULL,3),(23,'3333333321','Cliente Mock 21',NULL,NULL,NULL,NULL,3),(24,'3333333342','Cliente Mock 42',NULL,NULL,NULL,NULL,3),(25,'3333333332','Cliente Mock 32',NULL,NULL,NULL,NULL,3),(26,'3333333322','Cliente Mock 22',NULL,NULL,NULL,NULL,3),(27,'3333333343','Cliente Mock 43',NULL,NULL,NULL,NULL,3),(28,'3333333333','Cliente Mock 33',NULL,NULL,NULL,NULL,3),(29,'3333333323','Cliente Mock 23',NULL,NULL,NULL,NULL,3),(30,'3333333344','Cliente Mock 44',NULL,NULL,NULL,NULL,3),(31,'3333333334','Cliente Mock 34',NULL,NULL,NULL,NULL,3),(32,'3333333324','Cliente Mock 24',NULL,NULL,NULL,NULL,3);
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `especialidade`
--

DROP TABLE IF EXISTS `especialidade`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `especialidade` (
  `id_especialidade` int NOT NULL AUTO_INCREMENT,
  `nome` varchar(50) NOT NULL,
  PRIMARY KEY (`id_especialidade`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `especialidade`
--

LOCK TABLES `especialidade` WRITE;
/*!40000 ALTER TABLE `especialidade` DISABLE KEYS */;
INSERT INTO `especialidade` VALUES (1,'Cortes de Cabelo'),(2,'Barba'),(3,'Colorimetria');
/*!40000 ALTER TABLE `especialidade` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `servico`
--

DROP TABLE IF EXISTS `servico`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `servico` (
  `id_servico` int NOT NULL AUTO_INCREMENT,
  `nome` varchar(50) NOT NULL,
  `fk_especialidade` int DEFAULT NULL,
  `preco` decimal(10,2) NOT NULL,
  `duracao_estimada` int NOT NULL,
  PRIMARY KEY (`id_servico`),
  KEY `fk_especialidade` (`fk_especialidade`),
  CONSTRAINT `servico_ibfk_1` FOREIGN KEY (`fk_especialidade`) REFERENCES `especialidade` (`id_especialidade`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `servico`
--

LOCK TABLES `servico` WRITE;
/*!40000 ALTER TABLE `servico` DISABLE KEYS */;
INSERT INTO `servico` VALUES (1,'Corte Social',1,50.00,40),(2,'Corte Degradê',1,55.00,50),(3,'Colorir Cabelo',3,75.00,90),(4,'Aparar Barba',2,40.00,30);
/*!40000 ALTER TABLE `servico` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `profissional_especialidade`
--

DROP TABLE IF EXISTS `profissional_especialidade`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `profissional_especialidade` (
  `fk_profissional` int NOT NULL,
  `fk_especialidade` int NOT NULL,
  PRIMARY KEY (`fk_profissional`,`fk_especialidade`),
  KEY `fk_especialidade` (`fk_especialidade`),
  CONSTRAINT `profissional_especialidade_ibfk_1` FOREIGN KEY (`fk_profissional`) REFERENCES `usuario` (`id_usuario`) ON DELETE CASCADE,
  CONSTRAINT `profissional_especialidade_ibfk_2` FOREIGN KEY (`fk_especialidade`) REFERENCES `especialidade` (`id_especialidade`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `profissional_especialidade`
--

LOCK TABLES `profissional_especialidade` WRITE;
/*!40000 ALTER TABLE `profissional_especialidade` DISABLE KEYS */;
INSERT INTO `profissional_especialidade` VALUES (2,1),(4,1),(7,1),(11,1),(2,2),(3,2),(4,3);
/*!40000 ALTER TABLE `profissional_especialidade` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `atendimento`
--

DROP TABLE IF EXISTS `atendimento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `atendimento` (
  `id_atendimento` int NOT NULL AUTO_INCREMENT,
  `fk_profissional` int DEFAULT NULL,
  `fk_cliente` int DEFAULT NULL,
  `fk_servico` int DEFAULT NULL,
  `data_hora` datetime NOT NULL,
  `status_atendimento` enum('A','R','C') DEFAULT 'A' COMMENT 'A=Agendado, R=Realizado, C=Cancelado',
  PRIMARY KEY (`id_atendimento`),
  KEY `fk_profissional` (`fk_profissional`),
  KEY `fk_cliente` (`fk_cliente`),
  KEY `fk_servico` (`fk_servico`),
  CONSTRAINT `atendimento_ibfk_1` FOREIGN KEY (`fk_profissional`) REFERENCES `usuario` (`id_usuario`),
  CONSTRAINT `atendimento_ibfk_2` FOREIGN KEY (`fk_cliente`) REFERENCES `usuario` (`id_usuario`),
  CONSTRAINT `atendimento_ibfk_3` FOREIGN KEY (`fk_servico`) REFERENCES `servico` (`id_servico`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `atendimento`
--

LOCK TABLES `atendimento` WRITE;
/*!40000 ALTER TABLE `atendimento` DISABLE KEYS */;
INSERT INTO `atendimento` VALUES (1,2,12,1,'2026-04-20 14:00:00','R'),(2,3,13,4,'2026-04-21 09:00:00','A'),(3,2,14,2,'2026-04-21 10:00:00','A'),(4,4,15,3,'2026-04-21 11:30:00','A');
/*!40000 ALTER TABLE `atendimento` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'BarberFlow'
--
SET @@SESSION.SQL_LOG_BIN = @MYSQLDUMP_TEMP_LOG_BIN;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-04-21 21:29:20
