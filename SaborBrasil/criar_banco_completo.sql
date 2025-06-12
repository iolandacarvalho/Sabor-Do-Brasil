-- Script completo para criar o banco de dados 'mydb' com todas as alterações já aplicadas

CREATE DATABASE IF NOT EXISTS `mydb`;
USE `mydb`;

-- Tabela Usuario (apenas Email e Senha obrigatórios, demais campos opcionais)
CREATE TABLE IF NOT EXISTS `Usuario` (
  `idusuario` INT NOT NULL AUTO_INCREMENT,
  `email` VARCHAR(255) NOT NULL,
  `senha` VARCHAR(255) NOT NULL,
  `foto_perfil` VARCHAR(255) NULL,
  `created_at` DATETIME NULL,
  `nome` VARCHAR(255) NULL,
  `cpf` VARCHAR(20) NULL,
  `telefone` VARCHAR(20) NULL,
  PRIMARY KEY (`idusuario`),
  UNIQUE KEY `unique_email` (`email`)
);

-- Tabela Publicacao com campos de localização
CREATE TABLE IF NOT EXISTS `Publicacao` (
  `idpost` INT NOT NULL AUTO_INCREMENT,
  `nome` VARCHAR(255) NOT NULL,
  `descricao` TEXT NOT NULL,
  `Imagem` VARCHAR(255),
  `UsuarioId` INT,
  `dataPublicao` DATETIME,
  `local` VARCHAR(255) NULL,
  `cidade` VARCHAR(100) NULL,
  `estado` VARCHAR(50) NULL,
  `excluido` BOOLEAN DEFAULT FALSE,
  `dataExclusao` DATETIME NULL,
  `excluidoPor` INT NULL,
  PRIMARY KEY (`idpost`),
  FOREIGN KEY (`UsuarioId`) REFERENCES `Usuario`(`idusuario`)
);

-- Tabela de Likes
CREATE TABLE IF NOT EXISTS `Likes` (
  `idlikes` INT NOT NULL AUTO_INCREMENT,
  `id_usuario` INT NOT NULL,
  `id_post` INT NOT NULL,
  PRIMARY KEY (`idlikes`),
  UNIQUE KEY `unique_user_post_like` (`id_usuario`, `id_post`),
  FOREIGN KEY (`id_usuario`) REFERENCES `Usuario`(`idusuario`),
  FOREIGN KEY (`id_post`) REFERENCES `Publicacao`(`idpost`)
);

-- Tabela de Dislikes
CREATE TABLE IF NOT EXISTS `dislikes` (
  `iddislikes` INT NOT NULL AUTO_INCREMENT,
  `id_usuario` INT NOT NULL,
  `id_post` INT NOT NULL,
  PRIMARY KEY (`iddislikes`),
  UNIQUE KEY `unique_user_post_dislike` (`id_usuario`, `id_post`),
  FOREIGN KEY (`id_usuario`) REFERENCES `Usuario`(`idusuario`),
  FOREIGN KEY (`id_post`) REFERENCES `Publicacao`(`idpost`)
);

-- Tabela de Comentarios
CREATE TABLE IF NOT EXISTS `comentarios` (
  `idcomentario` INT NOT NULL AUTO_INCREMENT,
  `descricao` TEXT NOT NULL,
  `id_usuario` INT,
  `data` DATETIME,
  `post_id` INT,
  `created_at` DATETIME DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`idcomentario`),
  FOREIGN KEY (`id_usuario`) REFERENCES `Usuario`(`idusuario`),
  FOREIGN KEY (`post_id`) REFERENCES `Publicacao`(`idpost`)
);
