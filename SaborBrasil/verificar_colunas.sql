-- Script para verificar e criar colunas de localização se necessário

-- Verificar se as colunas existem
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'mydb' 
AND TABLE_NAME = 'Publicacao' 
AND COLUMN_NAME IN ('local', 'cidade', 'estado');

-- Criar colunas se não existirem
SET @sql = '';

-- Verificar se coluna 'local' existe
SELECT COUNT(*) INTO @local_exists 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'mydb' 
AND TABLE_NAME = 'Publicacao' 
AND COLUMN_NAME = 'local';

IF @local_exists = 0 THEN
    SET @sql = CONCAT(@sql, 'ALTER TABLE `mydb`.`Publicacao` ADD COLUMN `local` VARCHAR(255) NULL AFTER `dataPublicao`; ');
END IF;

-- Verificar se coluna 'cidade' existe
SELECT COUNT(*) INTO @cidade_exists 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'mydb' 
AND TABLE_NAME = 'Publicacao' 
AND COLUMN_NAME = 'cidade';

IF @cidade_exists = 0 THEN
    SET @sql = CONCAT(@sql, 'ALTER TABLE `mydb`.`Publicacao` ADD COLUMN `cidade` VARCHAR(100) NULL AFTER `local`; ');
END IF;

-- Verificar se coluna 'estado' existe
SELECT COUNT(*) INTO @estado_exists 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'mydb' 
AND TABLE_NAME = 'Publicacao' 
AND COLUMN_NAME = 'estado';

IF @estado_exists = 0 THEN
    SET @sql = CONCAT(@sql, 'ALTER TABLE `mydb`.`Publicacao` ADD COLUMN `estado` VARCHAR(50) NULL AFTER `cidade`; ');
END IF;

-- Executar as alterações se necessário
IF LENGTH(@sql) > 0 THEN
    PREPARE stmt FROM @sql;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
    SELECT 'Colunas criadas com sucesso!' as resultado;
ELSE
    SELECT 'Todas as colunas já existem!' as resultado;
END IF; 