-- Script para adicionar as colunas de localização que estão faltando
-- Execute este script no seu banco de dados MySQL

USE mydb;

-- Adicionar coluna 'local' 
ALTER TABLE `Publicacao` 
ADD COLUMN `local` VARCHAR(255) NULL;

-- Adicionar coluna 'cidade' 
ALTER TABLE `Publicacao` 
ADD COLUMN `cidade` VARCHAR(100) NULL;

-- Adicionar coluna 'estado' 
ALTER TABLE `Publicacao` 
ADD COLUMN `estado` VARCHAR(50) NULL;

-- Verificar se as colunas foram criadas
DESCRIBE `Publicacao`; 