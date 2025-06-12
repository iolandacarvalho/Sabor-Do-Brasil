-- Script simples para adicionar colunas de localização
-- Execute este script no seu banco de dados MySQL

-- Adicionar coluna 'local' se não existir
ALTER TABLE `mydb`.`Publicacao` 
ADD COLUMN IF NOT EXISTS `local` VARCHAR(255) NULL AFTER `dataPublicao`;

-- Adicionar coluna 'cidade' se não existir
ALTER TABLE `mydb`.`Publicacao` 
ADD COLUMN IF NOT EXISTS `cidade` VARCHAR(100) NULL AFTER `local`;

-- Adicionar coluna 'estado' se não existir
ALTER TABLE `mydb`.`Publicacao` 
ADD COLUMN IF NOT EXISTS `estado` VARCHAR(50) NULL AFTER `cidade`;

-- Verificar se as colunas foram criadas
DESCRIBE `mydb`.`Publicacao`; 