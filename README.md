# Sabor do Brasil


## Descrição do Projeto

O **Sabor do Brasil** é uma plataforma colaborativa onde usuários podem compartilhar pratos típicos brasileiros, cadastrar novas receitas com fotos, curtir e comentar publicações de outros usuários. O sistema permite criar uma conta com foto de perfil, visualizar detalhes de cada prato, interagir com a comunidade e explorar a culinária nacional de forma interativa e social.

## Funcionalidades

- **Cadastro e Login de Usuário:** Crie sua conta com nome, CPF, e-mail, senha e foto de perfil.
- **Foto de Perfil:** Agora é possível adicionar uma foto de perfil no cadastro. Após login, a logo do site é substituída pela foto do usuário.
- **Adicionar Prato:** Usuários logados podem cadastrar novos pratos com imagem, nome e descrição.
- **Curtir Pratos:** Usuários logados podem curtir ou descurtir pratos.
- **Comentários:** Usuários logados podem comentar nos pratos.
- **Página Principal:** Lista de pratos cadastrados, com imagem, descrição, autor e data.
- **Detalhes do Prato:** Clique em um prato para ver mais informações e comentários.


---

## Estrutura de Pastas

```
wwwroot/
├── index.html                # Página principal (lista de pratos)
├── login.html                # Tela de login
├── Cadastro.html             # Tela de cadastro de usuário
├── Cadastro de Produto.html  # Tela de cadastro de prato
├── uploads/                  # Imagens de pratos e perfis
│   └── ...
└── ...
```

---



## Observações Técnicas

- O estado de login é salvo no localStorage (`isLoggedIn`, `userId`, `userName`, `userPhoto`).
- O botão "Adicionar Prato" só aparece para usuários logados.
- A logo do site é trocada pela foto de perfil do usuário após login.
- O upload de foto de perfil é feito no cadastro do usuário.
- O upload de imagem do prato é feito no cadastro de prato.
- As curtidas e comentários são integrados com a API.

---

## Requisitos

- Navegador moderno (Chrome, Edge, Firefox)
- Backend compatível com as rotas usadas no frontend (`/api/publicacoes`, `/api/comentarios`, `/Usuario/Login`, `/Usuario/Cadastrar` etc.)

---


**Sabor do Brasil** - Compartilhe o melhor da nossa culinária!


