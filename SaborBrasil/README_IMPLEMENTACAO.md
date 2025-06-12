# Implementação de Foto de Perfil e Dislikes - Sabor do Brasil

## Resumo das Implementações

Este documento descreve as implementações realizadas para adicionar:
1. **Foto de perfil do usuário**
2. **Funcionalidade de dislikes**
3. **Logo dinâmico (foto do usuário quando logado)**
4. **Upload de foto no cadastro**
5. **Botão para cadastrar pratos quando logado**
6. **Campos de localização no cadastro de pratos**
7. **Exibição de localização nas publicações**
8. **Correção do email padrão**

## 1. Modificações no Banco de Dados

### Script SQL (database_updates.sql)
Execute os seguintes comandos no seu banco de dados MySQL:

```sql
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
```

## 2. Modificações nos Modelos

### Models/Usuario.cs
- Adicionada propriedade `FotoPerfil` com mapeamento para a coluna `foto_perfil`

### Models/Dislike.cs (NOVO)
- Criado modelo para representar a tabela de dislikes

### Models/Publicacao.cs
- Adicionadas propriedades `Local`, `Cidade` e `Estado` para localização

### Data/AppDbContext.cs
- Adicionado `DbSet<Dislike> Dislikes`

## 3. Modificações nos Controladores

### Controllers/UsuarioController.cs
- **Método Login**: Agora retorna `nome` e `fotoPerfil` do usuário
- **Método Cadastrar**: Atualizado para aceitar foto de perfil no cadastro
- **Método UploadFotoPerfil** (NOVO): Permite upload de foto de perfil
  - Cria diretório `/uploads/profiles/` se não existir
  - Remove foto anterior se existir
  - Gera nome único para a nova foto

### Controllers/PublicacaoController.cs
- **Método CriarPublicacao**: Atualizado para incluir campos de localização
- **Método Listar**: Agora retorna dados de localização
- **Método Dislike** (NOVO): Adiciona dislike e remove like se existir
- **Método RemoverDislike** (NOVO): Remove dislike
- **Método DislikesCount** (NOVO): Conta dislikes de um post
- **Método UserDisliked** (NOVO): Verifica se usuário deu dislike
- **Método TotalDislikes** (NOVO): Conta total de dislikes
- **Método Like**: Agora remove dislike se existir ao dar like

## 4. Modificações no Frontend

### wwwroot/index.html
- **Interface de login**: Mostra foto de perfil, nome e email quando logado
- **Botões de perfil e logout**: Adicionados na seção de login
- **Logo dinâmico**: Mostra foto do usuário quando logado, logo da empresa quando não logado
- **Botão cadastrar prato**: Aparece apenas quando usuário está logado
- **Funcionalidade de dislikes**: Implementada completamente
- **Estatísticas**: Agora carrega total de dislikes
- **Lógica exclusiva**: Like e dislike não funcionam simultaneamente
- **Exibição de localização**: Formata e exibe local, cidade e estado
- **Email dinâmico**: Não mostra email padrão quando não disponível

### wwwroot/login.html
- **Armazenamento**: Agora salva `userPhoto` no localStorage

### wwwroot/Cadastro.html
- **Campo de foto**: Adicionado campo para upload de foto de perfil
- **Pré-visualização**: Mostra preview da foto selecionada
- **Envio com foto**: Formulário envia foto junto com dados do usuário

### wwwroot/Cadastro de Produto.html
- **Campos de localização**: Adicionados campos para local, cidade e estado
- **Select de estados**: Lista completa dos estados brasileiros
- **Validação**: Campos opcionais mas bem estruturados

### wwwroot/perfil.html (NOVO)
- **Página de perfil**: Permite visualizar e alterar foto de perfil
- **Upload de foto**: Interface para enviar nova foto
- **Navegação**: Botões para voltar ao início e fazer logout

### wwwroot/uploads/product.html
- **Botão de dislike**: Adicionado na página de detalhes do prato
- **Funcionalidade completa**: Like e dislike funcionando
- **Estados visuais**: Ícones mudam conforme estado (like/dislike)
- **Lógica exclusiva**: Like e dislike não funcionam simultaneamente
- **Exibição de localização**: Mostra localização completa na página de detalhes

## 5. Funcionalidades Implementadas

### Foto de Perfil
- ✅ Upload de foto de perfil no cadastro
- ✅ Upload de foto de perfil na página de perfil
- ✅ Exibição da foto na interface
- ✅ Logo dinâmico (foto do usuário quando logado)
- ✅ Remoção automática de foto anterior
- ✅ Armazenamento no localStorage
- ✅ Página dedicada para gerenciar perfil

### Dislikes
- ✅ Sistema completo de dislikes
- ✅ Contadores de dislikes por post
- ✅ Contador total de dislikes
- ✅ Verificação de estado (usuário já deu dislike?)
- ✅ Remoção de like ao dar dislike (e vice-versa)
- ✅ Interface visual com ícones
- ✅ Funcionalidade em todas as páginas
- ✅ Lógica exclusiva (não funcionam simultaneamente)

### Localização
- ✅ Campos de local, cidade e estado no cadastro
- ✅ Select com todos os estados brasileiros
- ✅ Exibição formatada da localização
- ✅ Integração com sistema existente
- ✅ Campos opcionais mas bem estruturados

### Interface Dinâmica
- ✅ Logo muda para foto do usuário quando logado
- ✅ Botão para cadastrar pratos aparece apenas quando logado
- ✅ Email não exibido quando não disponível
- ✅ Interface responsiva e intuitiva

## 6. Estrutura de Arquivos

```
📁 Models/
├── Usuario.cs (modificado)
├── Dislike.cs (novo)
├── Publicacao.cs (modificado)
└── ...

📁 Controllers/
├── UsuarioController.cs (modificado)
├── PublicacaoController.cs (modificado)
└── ...

📁 Data/
└── AppDbContext.cs (modificado)

📁 wwwroot/
├── index.html (modificado)
├── login.html (modificado)
├── Cadastro.html (modificado)
├── Cadastro de Produto.html (modificado)
├── perfil.html (novo)
├── uploads/product.html (modificado)
└── uploads/profiles/ (criado automaticamente)
```

## 7. Como Usar

### Para Usuários
1. **Fazer login** na aplicação
2. **Ver foto de perfil** na seção de login (canto direito)
3. **Ver logo personalizado** (sua foto aparece no lugar do logo)
4. **Clicar em "Meu Perfil"** para gerenciar foto
5. **Clicar em "Cadastrar Novo Prato"** para adicionar pratos
6. **Preencher localização** ao cadastrar pratos (opcional)
7. **Usar likes e dislikes** nas publicações (exclusivos)
8. **Ver localização** nas publicações
9. **Fazer logout** quando necessário

### Para Desenvolvedores
1. Execute o script SQL no banco de dados
2. Compile e execute a aplicação
3. Teste as funcionalidades de upload e dislikes
4. Teste o cadastro de pratos com localização
5. Verifique se as pastas de upload são criadas automaticamente

## 8. Observações Técnicas

- **Segurança**: Apenas usuários logados podem fazer upload de foto
- **Performance**: Fotos antigas são removidas automaticamente
- **UX**: Interface responsiva e intuitiva
- **Compatibilidade**: Funciona com o sistema existente
- **Escalabilidade**: Estrutura preparada para futuras expansões
- **Lógica exclusiva**: Like e dislike não podem estar ativos simultaneamente
- **Localização**: Campos opcionais mas bem estruturados

## 9. Próximos Passos Sugeridos

1. Adicionar validação de tipos de arquivo (apenas imagens)
2. Implementar redimensionamento automático de imagens
3. Adicionar limite de tamanho para uploads
4. Implementar cache para melhor performance
5. Adicionar notificações em tempo real
6. Implementar sistema de avaliações (estrelas)
7. Adicionar busca por localização
8. Implementar mapa para mostrar localização 