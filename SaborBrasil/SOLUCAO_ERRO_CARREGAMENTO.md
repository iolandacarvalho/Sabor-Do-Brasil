# Solução para Erro ao Carregar Pratos

## Problema Identificado

O erro ao carregar os pratos está relacionado às novas colunas de localização (`local`, `cidade`, `estado`) que foram adicionadas ao modelo `Publicacao` mas ainda não existem no banco de dados.

## Soluções

### Opção 1: Executar Script SQL (Recomendado)

Execute o seguinte script no seu banco de dados MySQL:

```sql
-- Adicionar colunas de localização na tabela Publicacao
ALTER TABLE `mydb`.`Publicacao` 
ADD COLUMN `local` VARCHAR(255) NULL AFTER `dataPublicao`;

ALTER TABLE `mydb`.`Publicacao` 
ADD COLUMN `cidade` VARCHAR(100) NULL AFTER `local`;

ALTER TABLE `mydb`.`Publicacao` 
ADD COLUMN `estado` VARCHAR(50) NULL AFTER `cidade`;
```

### Opção 2: Usar Script Automático

Execute o arquivo `criar_colunas_localizacao.sql` que foi criado.

### Opção 3: Verificar se as Colunas Existem

Execute este comando para verificar se as colunas já existem:

```sql
DESCRIBE `mydb`.`Publicacao`;
```

## Como Executar

1. **Abra seu cliente MySQL** (MySQL Workbench, phpMyAdmin, etc.)
2. **Conecte ao banco de dados** `mydb`
3. **Execute o script SQL** da Opção 1
4. **Reinicie a aplicação** se necessário

## Verificação

Após executar o script, você deve ver as novas colunas:
- `local` (VARCHAR(255), NULL)
- `cidade` (VARCHAR(100), NULL)  
- `estado` (VARCHAR(50), NULL)

## Fallback Implementado

O controller foi atualizado com um sistema de fallback que:
1. Tenta carregar com as colunas de localização
2. Se falhar, tenta sem as colunas (usando strings vazias)
3. Logs detalhados para identificar o problema

## Logs de Debug

Verifique os logs da aplicação para ver:
- "Iniciando listagem de publicações..."
- "Publicações encontradas: X"
- Ou mensagens de erro detalhadas

## Próximos Passos

1. Execute o script SQL
2. Teste a aplicação
3. Se ainda houver problemas, verifique os logs
4. Cadastre um novo prato para testar as funcionalidades de localização

## Arquivos Modificados

- `Controllers/PublicacaoController.cs` - Adicionado tratamento de erro e fallback
- `criar_colunas_localizacao.sql` - Script para criar colunas
- `verificar_colunas.sql` - Script para verificar colunas 