-- Adicionar coluna de foto de perfil na tabela Usuario
ALTER TABLE `mydb`.`Usuario` 
ADD COLUMN `foto_perfil` VARCHAR(255) NULL AFTER `created_at`;

-- Criar tabela de dislikes
CREATE TABLE IF NOT EXISTS `mydb`.`dislikes` (
  `iddislikes` INT NOT NULL AUTO_INCREMENT,
  `id_usuario` INT NOT NULL,
  `id_post` INT NOT NULL,
  PRIMARY KEY (`iddislikes`),
  UNIQUE INDEX `unique_user_post_dislike` (`id_usuario` ASC, `id_post` ASC) VISIBLE,
  INDEX `id_post_dislike_idx` (`id_post` ASC) VISIBLE,
  CONSTRAINT `fk_dislikes_id_usuario`
    FOREIGN KEY (`id_usuario`)
    REFERENCES `mydb`.`Usuario` (`idusuario`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_dislikes_id_post`
    FOREIGN KEY (`id_post`)
    REFERENCES `mydb`.`Publicacao` (`idpost`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
) ENGINE = InnoDB;

-- Adicionar colunas de localização na tabela Publicacao
ALTER TABLE `mydb`.`Publicacao` 
ADD COLUMN `local` VARCHAR(255) NULL AFTER `dataPublicao`,
ADD COLUMN `cidade` VARCHAR(100) NULL AFTER `local`,
ADD COLUMN `estado` VARCHAR(50) NULL AFTER `cidade`; 